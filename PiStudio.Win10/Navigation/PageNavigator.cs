﻿using PiStudio.Shared;
using PiStudio.Shared.Data;
using PiStudio.Win10.UI.Controls;
using PiStudio.Win10.UI.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace PiStudio.Win10.Navigation
{
    public class PageNavigator : IPageNavigator
    {
        private Frame m_frame;
        private Type m_page;
        private object m_args;
        private ISaveable m_editor;

        /// <summary>
        /// Creates new instance of <see cref="PageNavigator"/>
        /// </summary>
        /// <param name="frame"><see cref="Frame"/> used for navigation. If is <see cref="null"/> navigation will not be performed.</param>
        /// <param name="editor"><see cref="ISaveable"/> image editor. If is <see cref="null"/> it's state will not be saved.</param>
        public PageNavigator(Frame frame, ISaveable editor)
        {
            m_frame = frame;
            m_editor = editor;
        }

        /// <summary>
        /// Displays file picker and loads new image into application resources.
        /// </summary>
        public async Task LoadNewImageWithUIAsync()
        {
            if (m_editor.HasUnsavedChange)
            {
                if(!await CreateAndDisplayChangesDialog())
                    return;
            }
            await DisplayDialogAndSaveToTmpFile();
        }

        /// <summary>
        /// Provides intialization, shows file picker and stores image into application resources.
        /// </summary>
        public async Task GetStartedButtonClick()
        {
            await DisplayDialogAndSaveToTmpFile();
            m_frame.Navigate(typeof(HomePage));
        }

        /// <summary>
        /// Displays <see cref="FileOpenPicker"/>, picks a file and copies it into app local folder
        /// </summary>
        private async Task DisplayDialogAndSaveToTmpFile()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.CommitButtonText = "Select";
            foreach (var item in AppSettings.Instance.SupportedImageTypes)
                picker.FileTypeFilter.Add(item);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var file = await picker.PickSingleFileAsync();
            if (file == null)
                return;
            var newFile = await file.CopyAsync(ApplicationData.Current.LocalFolder, WinAppResources.Instance.TmpImageName, NameCollisionOption.ReplaceExisting);
            WinAppResources.Instance.LoadedFile = file.Path;
        }

        /// <summary>
        /// Navigates to given page with given navigation parameter.
        /// </summary>
        /// <param name="pageType">Type of the target page</param>
        /// <param name="args">Optional navigation parameters</param>
        public async Task<bool> NavigateTo(Type pageType, NavigationParameter args)
        {
            m_page = pageType;
            m_args = args;
            if (m_editor != null && m_editor.HasUnsavedChange)
            {
                if (AppSettings.Instance.AutoSave)
                    await AutoSave();
                else
                    return await CreateAndDisplayChangesDialog();
            }

            if (m_frame != null)
                m_frame.Navigate(pageType, args);
            return true;
        }

        //decides whether store m_editor into temp file or into the one, chosen by user
        private async Task AutoSave()
        {
            m_editor.SaveChanges();
            var file = WinAppResources.Instance.FinalStorage;
            if (file == null)
                await FileServer.SaveTempAsync(m_editor);
            else
                await FileServer.SaveToFileAsync(file, m_editor);
        }

        /// <summary>
        /// Shares currently edited image to external apps.
        /// </summary>
        /// <remarks>
        /// Handles and share activators are located in <see cref="WinAppResources"/>.
        /// </remarks>
        public async void Share()
        {
            m_frame = null;
            if (m_editor != null && m_editor.HasUnsavedChange)
            {
                if (!AppSettings.Instance.AutoSave)
                    await CreateAndDisplayChangesDialog();
                else
                    await AutoSave();
            }
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
            //frame will remain null because we don't have handler for end of sharing
        }

        /// <summary>
        /// Displays dialog that warns user about unsaved changes. User has 3 options: 
        /// {save and continue, dismiss and continue, cancel}
        /// </summary>
        /// <returns>False if navigation was cancelled, true if not.</returns>
        private async Task<bool> CreateAndDisplayChangesDialog()
        {
            MessageDialog dialog = new MessageDialog("Do you want to save the unsaved changes?");
            dialog.Title = "PiStudio";
            dialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay;
            var save = new UICommand("Save", new UICommandInvokedHandler(SaveAndContinue), 0);
            var dismiss = new UICommand("Dismiss", new UICommandInvokedHandler(DismissAndContinue), 1);
            var cancel = new UICommand("Cancel", null, 2); 

            dialog.Commands.Add(save);
            dialog.Commands.Add(dismiss);
            dialog.Commands.Add(cancel);

            dialog.DefaultCommandIndex = 2;
            await dialog.ShowAsync();
            return m_result;
        }

        private bool m_result = false;

        private async void SaveAndContinue(IUICommand command)
        {
            if(WinAppResources.Instance.FinalStorage == null)
                await PickFinalStorage();

            var finalStorage = WinAppResources.Instance.FinalStorage;
            if (finalStorage == null)
                return;

            if (m_editor is PiCanvas)
                await FileServer.SaveToFileAsync(finalStorage, await WinAppResources.Instance.GetImageEditorAsync());
            m_editor.SaveChanges();
            await FileServer.SaveToFileAsync(finalStorage, m_editor);

            m_result = true;
            if(m_frame != null)
                m_frame.Navigate(m_page, m_args);
        }

        private void DismissAndContinue(IUICommand command)
        {
            m_editor.Dismiss();
            if(m_frame != null)
                m_frame.Navigate(m_page, m_args);
            m_result = true;
        }

        /// <summary>
        /// Picks file where will be final image saved and saves it into WinAppResources.FinalStorage property
        /// </summary>
        public static async Task PickFinalStorage()
        {
            var picker = new FileSavePicker();
            picker.CommitButtonText = WinAppResources.Instance.ApplicationLanguage.MenuItem5;

            picker.SuggestedFileName = GetSuggestedFileName();
            foreach (var item in AppSettings.Instance.SupportedImageTypes)
                picker.FileTypeChoices.Add(item, new List<string>() { item });
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            StorageFile file = await picker.PickSaveFileAsync();
            WinAppResources.Instance.FinalStorage = file;
        }

        private static string GetSuggestedFileName()
        {
            var loadedFile = WinAppResources.Instance.LoadedFile;
            if (string.IsNullOrEmpty(loadedFile))
                return "";
            var index = loadedFile.LastIndexOf("/");
            if (index != -1)
                loadedFile = loadedFile.Substring(index);
            index = loadedFile.LastIndexOf(".");
            if (index != -1)
                loadedFile = loadedFile.Insert(index, "(1)");
            return loadedFile;
        }
    }
}
