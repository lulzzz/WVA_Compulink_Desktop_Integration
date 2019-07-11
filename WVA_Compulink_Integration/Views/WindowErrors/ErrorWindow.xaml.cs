using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WVA_Compulink_Desktop_Integration.Views.WindowErrors
{
    public partial class ErrorWindow : Window
    {
        public string Error { get; set; }

        public ErrorWindow(string error)
        {
            Error = error;
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            MessagesTextBox.Document.Blocks.Add(new Paragraph(new Run(Error)));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ReportErrorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Report this error
                string error = new TextRange(MessagesTextBox.Document.ContentStart, MessagesTextBox.Document.ContentEnd).Text.Trim();
                Thread.Sleep(1000);
                Close();
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
