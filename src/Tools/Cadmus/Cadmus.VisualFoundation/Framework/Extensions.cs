using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Cadmus.VisualFoundation.Framework
{
    public static class Extensions
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", typeof(string), typeof(Extensions), new PropertyMetadata(default(string), PasswordPropertyChangedCallback));

        private static void PasswordPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var pbox = sender as PasswordBox;
            var newPassword = e.NewValue as string;
            if (pbox.Password != newPassword)
                pbox.Password = newPassword;
        }

        public static void SetPassword(DependencyObject element, string value)
        {
            element.SetValue(PasswordProperty, value);
        }

        public static string GetPassword(DependencyObject element)
        {
            return (string)element.GetValue(PasswordProperty);
        }


        public static readonly DependencyProperty FlowDocumentProperty = DependencyProperty.RegisterAttached(
            "FlowDocument", typeof(FlowDocument), typeof(Extensions), new PropertyMetadata(default(FlowDocument), FlowDocumentPropertyChangedCallback));

        private static void FlowDocumentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tb = sender as RichTextBox;
            var doc = e.NewValue as FlowDocument;
            if (tb.Document != doc)
                tb.Document = doc;
        }

        public static void SetFlowDocument(DependencyObject element, FlowDocument value)
        {
            element.SetValue(FlowDocumentProperty, value);
        }

        public static FlowDocument GetFlowDocument(DependencyObject element)
        {
            return (FlowDocument)element.GetValue(FlowDocumentProperty);
        }

        public static Color ToMediaColor(this ConsoleColor cc)
        {
            var colorCode = "#000000";
            switch (cc)
            {
                case ConsoleColor.Black: colorCode = "#000000"; break;
                case ConsoleColor.DarkGreen: colorCode = "#008000"; break;
                case ConsoleColor.DarkCyan: colorCode = "#008080"; break;
                case ConsoleColor.DarkRed: colorCode = "#800000"; break;
                case ConsoleColor.DarkMagenta: colorCode = "#800080"; break;
                case ConsoleColor.DarkYellow: colorCode = "#808000"; break;
                case ConsoleColor.Gray: colorCode = "#C0C0C0"; break;
                case ConsoleColor.DarkGray: colorCode = "#808080"; break;
                case ConsoleColor.Blue: colorCode = "#0000FF"; break;
                case ConsoleColor.Green: colorCode = "#00FF00"; break;
                case ConsoleColor.Cyan: colorCode = "#00FFFF"; break;
                case ConsoleColor.Red: colorCode = "#FF0000"; break;
                case ConsoleColor.Magenta: colorCode = "#FF00FF"; break;
                case ConsoleColor.Yellow: colorCode = "#FFFF00"; break;
                case ConsoleColor.White: colorCode = "#FFFFFF"; break;
            }

            return colorCode.HexToColor();
        }

        public static Color HexToColor(this string hex)
        {
            var color = ColorConverter.ConvertFromString(hex);
            if (color == null)
                return Colors.White;
            return (Color)color;
        }
    }
}
