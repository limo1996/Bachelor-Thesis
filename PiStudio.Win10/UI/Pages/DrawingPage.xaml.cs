﻿using PiStudio.Shared.Data;
using PiStudio.Win10.Data;
using PiStudio.Win10.Navigation;
using PiStudio.Win10.UI.Controls;
using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PiStudio.Win10.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrawingPage : Page
    {
        public DrawingPage()
        {
            ApplicationTheme = new Theme();
            LanguagePack = new LanguagePack();
            WinAppResources.Instance.ApplicationTheme.CopyTo(ApplicationTheme);
            WinAppResources.Instance.ApplicationLanguage.CopyTo(LanguagePack);
            this.InitializeComponent();
            WinAppResources.Instance.InitializePage();
            ToolBox.BrushThicknessChanged += (o, e) => DrawingCanvas.BrushThickness = (uint)e;
            ToolBox.BrushColorChanged += (o, e) => DrawingCanvas.BrushColor = e;
            ToolBox.ClearClicked += (o, e) => DrawingCanvas.Clear();
            ToolBox.UndoClicked += (o, e) => DrawingCanvas.Undo();
            DrawingCanvas.BrushThickness = 1;
        }

        public Theme ApplicationTheme { get; set; }
        public LanguagePack LanguagePack { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ImgPresenter.SizeChanged += (o, args) =>
            {
                DrawingCanvas.Width = args.NewSize.Width;
                DrawingCanvas.Height = args.NewSize.Height;
            };

            PRing.IsActive = true;
            var image = await WinAppResources.Instance.GetWorkingImage();
            ImgPresenter.Source = image;
            WinAppResources.Instance.SetImageStretch(ImgPresenter);
            PRing.IsActive = false;
        }

        private void Hamburger_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainMenu.IsPaneOpen = !MainMenu.IsPaneOpen;
        }

        private async void MenuItem_Click(object sender, System.EventArgs e)
        {
            var tmp = sender as MenuItem;
            if (tmp != null && !tmp.IsSelectionEnabled)
                return;

            NavigationParameter parameter = new NavigationParameter()
            {
                PrevPage = EnumPage.DrawingPage,
                Source = NavigationSource.Click
            };

            Type pageType = typeof(SettingsPage);
            PageNavigator navigator = new PageNavigator(this.Frame, DrawingCanvas);

            if (tmp == HomeItem)
                pageType = typeof(HomePage);
            else if (tmp == FilterItem)
                pageType = typeof(FiltersPage);
            else if (tmp == BrightnessItem)
                pageType = typeof(BrightnessPage);
            else if (tmp == DrawItem)
                pageType = typeof(DrawingPage);
            else if (tmp == SaveItem)
            {
                //save and continue
                Progress.IsActive = true;
                await FileServer.SaveTempAsync(DrawingCanvas);
                var image = await WinAppResources.Instance.GetWorkingImage();
                ImgPresenter.Source = image;
                DrawingCanvas.Clear();
                Progress.IsActive = false;
                return;
            }
            else if (tmp == SpeakItem)
            {
                //recognize and continue

                return;
            }
            else if (tmp == ShareItem)
            {
                navigator.Share();
                return;
            }
            await navigator.NavigateTo(pageType, parameter);
        }

        private void Grid_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            WinAppResources.Instance.SetImageStretch(ImgPresenter);
        }
    }
}
