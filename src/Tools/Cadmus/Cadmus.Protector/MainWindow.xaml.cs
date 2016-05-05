using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Cadmus.Foundation;

namespace Cadmus.Protector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PasswordProtector _protector = new PasswordProtector();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptMachineBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DecryptedValueTxt.Text))
            {
                EncryptedValueTxt.Text =
                    _protector.Protect(DecryptedValueTxt.Text, PasswordProtectionScope.LocalMachine);
            }
        }

        private void EncryptUserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DecryptedValueTxt.Text))
            {
                EncryptedValueTxt.Text =
                    _protector.Protect(DecryptedValueTxt.Text, PasswordProtectionScope.CurrentUser);
            }
        }

        private void DecryptUserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EncryptedValueTxt.Text))
            {
                DecryptedValueTxt.Text = _protector.UnProtect(EncryptedValueTxt.Text);
            }
        }
    }
}
