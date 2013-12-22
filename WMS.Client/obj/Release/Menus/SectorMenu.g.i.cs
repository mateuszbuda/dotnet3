﻿#pragma checksum "..\..\..\Menus\SectorMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F3F9BC6CC019849501451CD3DE2D3BC0"
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
    /// SectorMenu
    /// </summary>
    public partial class SectorMenu : WMS.Client.Menus.BaseMenu, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 24 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditButton;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteButton;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WarehouseSectorLabel;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label CountLabel;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewGroupButton;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainMenuButton;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SectorsButton;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LoadingLabel;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Menus\SectorMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid GroupsGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/WMS.Client;component/menus/sectormenu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Menus\SectorMenu.xaml"
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
            this.EditButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Menus\SectorMenu.xaml"
            this.EditButton.Click += new System.Windows.RoutedEventHandler(this.EditButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DeleteButton = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Menus\SectorMenu.xaml"
            this.DeleteButton.Click += new System.Windows.RoutedEventHandler(this.DeleteButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.WarehouseSectorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.CountLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.NewGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\Menus\SectorMenu.xaml"
            this.NewGroupButton.Click += new System.Windows.RoutedEventHandler(this.NewGroupButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MainMenuButton = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\Menus\SectorMenu.xaml"
            this.MainMenuButton.Click += new System.Windows.RoutedEventHandler(this.MainMenuButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SectorsButton = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Menus\SectorMenu.xaml"
            this.SectorsButton.Click += new System.Windows.RoutedEventHandler(this.SectorsButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.LoadingLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.GroupsGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 10:
            
            #line 49 "..\..\..\Menus\SectorMenu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.IdButtonClick);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 57 "..\..\..\Menus\SectorMenu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SenderButtonClick);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 64 "..\..\..\Menus\SectorMenu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SendButtonClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

