﻿#pragma checksum "..\..\..\Menus\WarehousesMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "443D6D323A20416BEE08D8111D7E3B04"
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
    /// WarehousesMenu
    /// </summary>
    public partial class WarehousesMenu : WMS.Client.Menus.BaseMenu, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Menus\WarehousesMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddNewButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Menus\WarehousesMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainMenuButton;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Menus\WarehousesMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid WarehousesGrid;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Menus\WarehousesMenu.xaml"
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
            System.Uri resourceLocater = new System.Uri("/WMS.Client;component/menus/warehousesmenu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Menus\WarehousesMenu.xaml"
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
            this.AddNewButton = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\Menus\WarehousesMenu.xaml"
            this.AddNewButton.Click += new System.Windows.RoutedEventHandler(this.AddNewButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainMenuButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Menus\WarehousesMenu.xaml"
            this.MainMenuButton.Click += new System.Windows.RoutedEventHandler(this.MainMenuButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.WarehousesGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 27 "..\..\..\Menus\WarehousesMenu.xaml"
            this.WarehousesGrid.SizeChanged += new System.Windows.SizeChangedEventHandler(this.MenuSizeChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LoadingLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

