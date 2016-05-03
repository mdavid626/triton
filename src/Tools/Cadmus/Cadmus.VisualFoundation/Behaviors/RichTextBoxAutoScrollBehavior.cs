using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Cadmus.VisualFoundation.Behaviors
{
    public class RichTextBoxAutoScrollBehavior : Behavior<RichTextBox>
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
            "Enabled", typeof(bool), typeof(RichTextBoxAutoScrollBehavior), new PropertyMetadata(true));

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Enabled)
            {
                AssociatedObject.ScrollToEnd();
            }
        }
    }
}
