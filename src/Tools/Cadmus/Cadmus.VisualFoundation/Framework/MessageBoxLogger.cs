using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework
{
    public class MessageBoxLogger : ILogger
    {
        public string Title { get; set; }

        public void Log(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Log(string msg, ConsoleColor color)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LogLine(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LogLine(string msg, ConsoleColor color)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LogSuccess(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LogWarning(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void LogError(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Clear()
        {
            
        }
    }
}
