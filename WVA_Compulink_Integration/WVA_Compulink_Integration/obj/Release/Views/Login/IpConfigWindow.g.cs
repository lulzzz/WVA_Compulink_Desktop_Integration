﻿#pragma checksum "..\..\..\..\Views\Login\IpConfigWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9DAE96E6047528B9263992F6D2DB86F51665A489F3B1DACDF2727A137E600462"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using WVA_Compulink_Integration.Views.Login;


namespace WVA_Compulink_Integration.Views.Login {
    
    
    /// <summary>
    /// IpConfigWindow
    /// </summary>
    public partial class IpConfigWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 155 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label IpConfigLabel;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IpConfigTextBox;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ApiKeyLabel;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ApiKeyTextBox;
        
        #line default
        #line hidden
        
        
        #line 191 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SubmitButton;
        
        #line default
        #line hidden
        
        
        #line 202 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
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
            System.Uri resourceLocater = new System.Uri("/WVA_Compulink_Integration;component/views/login/ipconfigwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.IpConfigLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.IpConfigTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 169 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
            this.IpConfigTextBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.IpConfigTextBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ApiKeyLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.ApiKeyTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 185 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
            this.ApiKeyTextBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.ApiKeyTextBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SubmitButton = ((System.Windows.Controls.Button)(target));
            
            #line 200 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
            this.SubmitButton.Click += new System.Windows.RoutedEventHandler(this.SubmitButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 211 "..\..\..\..\Views\Login\IpConfigWindow.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

