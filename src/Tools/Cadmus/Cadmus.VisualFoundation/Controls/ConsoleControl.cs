using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Cadmus.Foundation;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.VisualFoundation.Controls
{
    public class ConsoleControl : Control
    {
        public static readonly DependencyProperty LoggerProperty = DependencyProperty.Register(
            "Logger", typeof(ILogger), typeof(ConsoleControl), new PropertyMetadata(default(ILogger)));

        public ILogger Logger
        {
            get { return (ILogger) GetValue(LoggerProperty); }
            set { SetValue(LoggerProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var tb = (RichTextBox) GetTemplateChild("RichTextBox");
            if (tb != null)
            {
                tb.Document = new FlowDocument();
                Logger = new FlowDocumentLogger(tb.Document);
            }
        }
    }
}
