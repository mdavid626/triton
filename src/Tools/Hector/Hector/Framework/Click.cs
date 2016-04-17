using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hector.Framework
{
    public class Click
    {
        public static string GetAttach(DependencyObject obj)
        {
            return (string)obj.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject obj, string value)
        {
            obj.SetValue(AttachProperty, value);
        }

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(string), typeof(Click), new PropertyMetadata(AttachPropertyChangedCallback));

        public static void AttachPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            var methodName = e.NewValue as string;
            if (button != null && !String.IsNullOrEmpty(methodName))
            {
                button.Click += (sender, ev) =>
                {
                    try
                    {
                        var target = button.DataContext;
                        var method = target.GetType().GetMethod(methodName);
                        method.Invoke(target, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };
            }
        }
    }
}
