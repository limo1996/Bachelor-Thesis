﻿using PiStudio.Shared;
using PiStudio.Shared.Data;
using PiStudio.Win10.UI.Pages;
using PiStudio.Win10.Voice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PiStudio.Win10.UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            await HandleLaunch();
            Frame rootFrame = (Frame)Window.Current.Content;

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(WelcomePage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            WinAppResources.Instance.CopyTo(AppSettings.Instance);
            await AppSettings.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when application was chosen as handler for file activation.
        /// </summary>
        /// <param name="args"></param>
        protected override async void OnFileActivated(FileActivatedEventArgs args)
        {
            // TODO: Handle file activation
            // The number of files received is args.Files.Size
            // The name of the first file is args.Files[0].Name
            await HandleLaunch();

            Frame rootFrame = (Frame)Window.Current.Content;
            if (args.Files.Count == 0)
                rootFrame.Navigate(typeof(WelcomePage));
            else
            {
                var file = args.Files[0] as StorageFile;
                if (file == null)   //cannot handle folders
                    rootFrame.Navigate(typeof(WelcomePage));
                ImageEditor editor;
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var decoder = await WinBitmapDecoder.CreateAsync(stream.AsStream());
                    editor = new ImageEditor(decoder, file.Path);
                }

                var t = FileServer.SaveTempAsync(editor);
                WinAppResources.Instance.LoadedFile = file.Path;
                var param = new NavigationParameter()
                {
                    Extra = editor,
                    PrevPage = EnumPage.AppStart,
                    Source = NavigationSource.FileOpenRequest
                };
                rootFrame.Navigate(typeof(HomePage), param);
            }
            Window.Current.Activate();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            await HandleLaunch();
            if(args.Kind == ActivationKind.VoiceCommand)
            {
                VoiceRecognizer.Instance.HandleLaunch((VoiceCommandActivatedEventArgs)args);
            }
            Window.Current.Activate();
        }

        private async Task HandleLaunch()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            await AppSettings.CreateAsync();
            WinAppResources.Instance.LoadFrom(AppSettings.Instance);
            //start loading
            var recognizer = VoiceRecognizer.Instance;

            Application.Current.Resources["ColorToBrushConverter"] = new Data.ColorToBrushConverter();
            Application.Current.Resources["LanguageToBoolConverter"] = new Data.LanguageToBoolConverter();
        }
    }
}