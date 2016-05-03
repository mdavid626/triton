using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.VisualFoundation.Behaviors
{
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }

        protected void AssociatedObject_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            Extensions.SetPassword(passwordBox, passwordBox.Password);
        }
    }
}
