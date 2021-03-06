﻿#pragma checksum "..\..\..\Menus\ProductMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "25210ED6D70EFCACDD743E211E22438C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WMS.Client.Menus;


namespace WMS.Client.Menus {
    
    
    /// <summary>
    /// ProductMenu
    /// </summary>
    public partial class ProductMenu : WMS.Client.Menus.BaseMenu, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ProductLabel;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditButton;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainMenuButton;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ProductsButton;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ProductGrid;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PriceLabel;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label DateLabel;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\Menus\ProductMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LoadingLabel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WMS.Client;component/menus/productmenu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Menus\ProductMenu.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ProductLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.EditButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Menus\ProductMenu.xaml"
            this.EditButton.Click += new System.Windows.RoutedEventHandler(this.EditButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.MainMenuButton = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\Menus\ProductMenu.xaml"
            this.MainMenuButton.Click += new System.Windows.RoutedEventHandler(this.MainMenuButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ProductsButton = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\Menus\ProductMenu.xaml"
            this.ProductsButton.Click += new System.Windows.RoutedEventHandler(this.ProductsButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ProductGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.PriceLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.DateLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.LoadingLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

