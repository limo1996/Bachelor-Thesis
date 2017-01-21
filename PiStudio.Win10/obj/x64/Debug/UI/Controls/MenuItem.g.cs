﻿#pragma checksum "C:\Users\Jakub Lichman\Source\Repos\Bachelor-Thesis\PiStudio.Win10\UI\Controls\MenuItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9E424708F9BC1CB44B390CAEA7B95806"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PiStudio.Win10.UI.Controls
{
    partial class MenuItem : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_Panel_Background(global::Windows.UI.Xaml.Controls.Panel obj, global::Windows.UI.Xaml.Media.Brush value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.Brush) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.Brush), targetNullValue);
                }
                obj.Background = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_SymbolIcon_Symbol(global::Windows.UI.Xaml.Controls.SymbolIcon obj, global::Windows.UI.Xaml.Controls.Symbol value)
            {
                obj.Symbol = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_IconElement_Foreground(global::Windows.UI.Xaml.Controls.IconElement obj, global::Windows.UI.Xaml.Media.Brush value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.Brush) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.Brush), targetNullValue);
                }
                obj.Foreground = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Foreground(global::Windows.UI.Xaml.Controls.TextBlock obj, global::Windows.UI.Xaml.Media.Brush value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.Brush) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.Brush), targetNullValue);
                }
                obj.Foreground = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        private class MenuItem_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMenuItem_Bindings
        {
            private global::PiStudio.Win10.UI.Controls.MenuItem dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Grid obj3;
            private global::Windows.UI.Xaml.Controls.SymbolIcon obj5;
            private global::Windows.UI.Xaml.Controls.TextBlock obj6;

            private MenuItem_obj1_BindingsTracking bindingsTracking;

            public MenuItem_obj1_Bindings()
            {
                this.bindingsTracking = new MenuItem_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3:
                        this.obj3 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 5:
                        this.obj5 = (global::Windows.UI.Xaml.Controls.SymbolIcon)target;
                        break;
                    case 6:
                        this.obj6 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    default:
                        break;
                }
            }

            // IMenuItem_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // MenuItem_obj1_Bindings

            public void SetDataRoot(global::PiStudio.Win10.UI.Controls.MenuItem newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::PiStudio.Win10.UI.Controls.MenuItem obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ApplicationTheme(obj.ApplicationTheme, phase);
                        this.Update_Symbol(obj.Symbol, phase);
                        this.Update_Text(obj.Text, phase);
                    }
                }
            }
            private void Update_ApplicationTheme(global::PiStudio.Win10.Data.Theme obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ApplicationTheme(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ApplicationTheme_PanelItemFocused(obj.PanelItemFocused, phase);
                        this.Update_ApplicationTheme_PanelForeground(obj.PanelForeground, phase);
                    }
                }
            }
            private void Update_ApplicationTheme_PanelItemFocused(global::Windows.UI.Color obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Panel_Background(this.obj3, (global::Windows.UI.Xaml.Media.Brush)this.LookupConverter("ColorToBrushConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.Brush), null, null), null);
                }
            }
            private void Update_Symbol(global::Windows.UI.Xaml.Controls.Symbol obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_SymbolIcon_Symbol(this.obj5, obj);
                }
            }
            private void Update_ApplicationTheme_PanelForeground(global::Windows.UI.Color obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_IconElement_Foreground(this.obj5, (global::Windows.UI.Xaml.Media.Brush)this.LookupConverter("ColorToBrushConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.Brush), null, null), null);
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Foreground(this.obj6, (global::Windows.UI.Xaml.Media.Brush)this.LookupConverter("ColorToBrushConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.Brush), null, null), null);
                }
            }
            private void Update_Text(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj6, obj, null);
                }
            }

            private class MenuItem_obj1_BindingsTracking
            {
                global::System.WeakReference<MenuItem_obj1_Bindings> WeakRefToBindingObj; 

                public MenuItem_obj1_BindingsTracking(MenuItem_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<MenuItem_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_(null);
                    UpdateChildListeners_ApplicationTheme(null);
                }

                public void DependencyPropertyChanged_Symbol(global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop)
                {
                    MenuItem_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::PiStudio.Win10.UI.Controls.MenuItem obj = sender as global::PiStudio.Win10.UI.Controls.MenuItem;
        if (obj != null)
        {
            bindings.Update_Symbol(obj.Symbol, DATA_CHANGED);
        }
                    }
                }
                public void DependencyPropertyChanged_Text(global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop)
                {
                    MenuItem_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::PiStudio.Win10.UI.Controls.MenuItem obj = sender as global::PiStudio.Win10.UI.Controls.MenuItem;
        if (obj != null)
        {
            bindings.Update_Text(obj.Text, DATA_CHANGED);
        }
                    }
                }
                private long tokenDPC_Symbol = 0;
                private long tokenDPC_Text = 0;
                public void UpdateChildListeners_(global::PiStudio.Win10.UI.Controls.MenuItem obj)
                {
                    MenuItem_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        if (bindings.dataRoot != null)
                        {
                            bindings.dataRoot.UnregisterPropertyChangedCallback(global::PiStudio.Win10.UI.Controls.MenuItem.SymbolProperty, tokenDPC_Symbol);
                            bindings.dataRoot.UnregisterPropertyChangedCallback(global::PiStudio.Win10.UI.Controls.MenuItem.TextProperty, tokenDPC_Text);
                        }
                        if (obj != null)
                        {
                            bindings.dataRoot = obj;
                            tokenDPC_Symbol = obj.RegisterPropertyChangedCallback(global::PiStudio.Win10.UI.Controls.MenuItem.SymbolProperty, DependencyPropertyChanged_Symbol);
                            tokenDPC_Text = obj.RegisterPropertyChangedCallback(global::PiStudio.Win10.UI.Controls.MenuItem.TextProperty, DependencyPropertyChanged_Text);
                        }
                    }
                }
                public void PropertyChanged_ApplicationTheme(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MenuItem_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::PiStudio.Win10.Data.Theme obj = sender as global::PiStudio.Win10.Data.Theme;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_ApplicationTheme_PanelItemFocused(obj.PanelItemFocused, DATA_CHANGED);
                                    bindings.Update_ApplicationTheme_PanelForeground(obj.PanelForeground, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "PanelItemFocused":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ApplicationTheme_PanelItemFocused(obj.PanelItemFocused, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "PanelForeground":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ApplicationTheme_PanelForeground(obj.PanelForeground, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::PiStudio.Win10.Data.Theme cache_ApplicationTheme = null;
                public void UpdateChildListeners_ApplicationTheme(global::PiStudio.Win10.Data.Theme obj)
                {
                    if (obj != cache_ApplicationTheme)
                    {
                        if (cache_ApplicationTheme != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ApplicationTheme).PropertyChanged -= PropertyChanged_ApplicationTheme;
                            cache_ApplicationTheme = null;
                        }
                        if (obj != null)
                        {
                            cache_ApplicationTheme = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ApplicationTheme;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.Grid element2 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 12 "..\..\..\..\UI\Controls\MenuItem.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).PointerEntered += this.OnPointEnter;
                    #line 12 "..\..\..\..\UI\Controls\MenuItem.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).PointerExited += this.OnPointExit;
                    #line 13 "..\..\..\..\UI\Controls\MenuItem.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).Tapped += this.OnTapped;
                    #line 13 "..\..\..\..\UI\Controls\MenuItem.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).PointerPressed += this.OnPointPress;
                    #line 13 "..\..\..\..\UI\Controls\MenuItem.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).PointerReleased += this.OnPointReleased;
                    #line default
                }
                break;
            case 3:
                {
                    this.FocusedRect = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4:
                {
                    this.Wrapper = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.UserControl element1 = (global::Windows.UI.Xaml.Controls.UserControl)target;
                    MenuItem_obj1_Bindings bindings = new MenuItem_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    bindings.SetConverterLookupRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

