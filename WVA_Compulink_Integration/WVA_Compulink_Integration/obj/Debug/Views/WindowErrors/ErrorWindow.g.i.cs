﻿#pragma checksum "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7C1FE0555D7F3F8F6920ED7ECCAC570227BCBC5F0B602AF1ADA743902D63D9EE"
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
using WVA_Compulink_Desktop_Integration.Views.WindowErrors;


namespace WVA_Compulink_Desktop_Integration.Views.WindowErrors {
    
    
    /// <summary>
    /// ErrorWindow
    /// </summary>
    public partial class ErrorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WVA_Compulink_Desktop_Integration.Views.WindowErrors.ErrorWindow F8Help;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox MessagesTextBox;
        
        #line default
        #line hidden
        
        
        #line 275 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReportErrorButton;
        
        #line default
        #line hidden
        
        
        #line 297 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
        #line default
        #line hidden
        
        
        #line 299 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image CloseImage;
        
        #line default
        #line hidden
        
        
        #line 308 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image WarningImageLeft;
        
        #line default
        #line hidden
        
        
        #line 326 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image WarningImageRight;
        
        #line default
        #line hidden
        
        
        #line 344 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackButton;
        
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
            System.Uri resourceLocater = new System.Uri("/WVA_Compulink_Desktop_Integration;component/views/windowerrors/errorwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
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
            this.F8Help = ((WVA_Compulink_Desktop_Integration.Views.WindowErrors.ErrorWindow)(target));
            return;
            case 2:
            this.MessagesTextBox = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 3:
            this.ReportErrorButton = ((System.Windows.Controls.Button)(target));
            
            #line 283 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
            this.ReportErrorButton.Click += new System.Windows.RoutedEventHandler(this.ReportErrorButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 296 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CloseImage = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.WarningImageLeft = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.WarningImageRight = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.BackButton = ((System.Windows.Controls.Button)(target));
            
            #line 352 "..\..\..\..\Views\WindowErrors\ErrorWindow.xaml"
            this.BackButton.Click += new System.Windows.RoutedEventHandler(this.BackButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

