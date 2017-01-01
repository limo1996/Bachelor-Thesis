﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using PiStudio.Shared.Data;
using Windows.UI.Xaml.Media.Imaging;
using PiStudio.Win10.Data;
using PiStudio.Win10.UI.Controls;
using PiStudio.Win10.Navigation;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PiStudio.Win10.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FiltersPage : Page
    {
        public FiltersPage()
        {
            ApplicationTheme = new Theme();
            WinAppResources.Instance.ApplicationTheme.CopyTo(ApplicationTheme);
            this.InitializeComponent();
            WinAppResources.Instance.InitializePage();
        }

        public Theme ApplicationTheme { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PRing.IsActive = true);

            ImageEditor editor = (ImageEditor)WinAppResources.Instance.Editor;
            ImageContent.Source = WinAppResources.Instance.WorkingImage;

            

            await LoadItems(editor);

           

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PRing.IsActive = false);
        }

        private async Task LoadItems(ImageEditor editor)
        {
            ObservableCollection<FilterItem> items = new ObservableCollection<FilterItem>();

            FilterGridView.ItemsSource = items;
            foreach (var filter in WinAppResources.Instance.Filters)
            {
                var item = new FilterItem();
                item.Text = filter.Name;
                item.Source = await editor.ApplyFilterAsync(filter);
                items.Add(item);
            }

            FilterGridView.ItemClick += (o, ee) =>
            {
                var filterItem = (FilterItem)ee.ClickedItem;
                ImageContent.Source = (WriteableBitmap)filterItem.Source;
            };
        }

        private void Hamburger_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainMenu.IsPaneOpen = !MainMenu.IsPaneOpen;
        }

        private void MenuItem_Click(object sender, System.EventArgs e)
        {
            var tmp = sender as MenuItem;
            if (tmp != null && !tmp.IsSelectionEnabled)
                return;
            foreach (var item in ItemsWrapper.Children)
            {
                var menuItem = item as MenuItem;
                if (menuItem != null && menuItem != sender)
                    menuItem.IsSelected = false;
            }

            NavigationParameter parameter = new NavigationParameter()
            {
                PrevPage = EnumPage.HomePage,
                Source = NavigationSource.Click
            };

            Type pageType = typeof(SettingsPage);
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

                return;
            }
            else if(tmp == SpeakItem)
            {
                //recognize and continue

                return;
            }
            PageNavigator navigator = new PageNavigator(this.Frame, WinAppResources.Instance.Editor);
            navigator.NavigateTo(pageType, parameter);
        }
    }
}
