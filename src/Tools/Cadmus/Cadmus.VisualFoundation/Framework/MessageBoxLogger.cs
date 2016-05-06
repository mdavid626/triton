using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework
{
    public class MessageBoxLogger : LoggerBase
    {
        public string Title { get; set; }

        public override void LogInfo(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public override void LogSuccess(string msg)
        {
            LogInfo(msg);
        }

        public override void LogWarning(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public override void LogError(string msg)
        {
            MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
