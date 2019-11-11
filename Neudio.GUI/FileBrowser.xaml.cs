using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Neudio.GUI
{
    /// <summary>
    /// FileBrowser.xaml の相互作用ロジック
    /// </summary>
    public partial class FileBrowser : UserControl
    {
        public enum Mode
        {
            File, Folder
        }
        //public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", null, typeof(FileBrowser));

        public FileBrowser()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
