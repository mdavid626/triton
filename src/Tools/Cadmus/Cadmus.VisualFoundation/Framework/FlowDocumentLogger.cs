using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Cadmus.Foundation;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.VisualFoundation.Framework
{
    public class FlowDocumentLogger : ILogger
    {
        public FlowDocument FlowDocument { get; set; }

        public Paragraph CurrentParagraph { get; protected set; }

        public FlowDocumentLogger(FlowDocument flowDocument)
        {
            FlowDocument = flowDocument;
        }

        protected void LogInternal(string msg, ConsoleColor? color = null, bool newLine = false)
        {
            Caliburn.Micro.Execute.OnUIThread(() =>
            {
                LogInternalRaw(msg, color, newLine);
            });
        }

        protected void LogInternalRaw(string msg, ConsoleColor? color = null, bool newLine = false)
        {
            if (CurrentParagraph == null)
            {
                CurrentParagraph = new Paragraph();
                FlowDocument.Blocks.Add(CurrentParagraph);
            }

            var endOfLine = newLine ? Environment.NewLine : String.Empty;
            var mediaColor = color?.ToMediaColor() ?? Colors.White;
            var run = new Run(msg + endOfLine) { Foreground = new SolidColorBrush(mediaColor) };
            CurrentParagraph.Inlines.Add(run);
        }

        public void Log(string msg)
        {
            LogInternal(msg);
        }

        public void Log(string msg, ConsoleColor color)
        {
            LogInternal(msg, color, false);
        }

        public void LogLine(string msg)
        {
            LogInternal(msg, null, true);
        }

        public void LogLine(string msg, ConsoleColor color)
        {
            LogInternal(msg, color, true);
        }

        public void LogSuccess(string msg)
        {
            LogLine(msg, ConsoleColor.Green);
        }

        public void LogWarning(string msg)
        {
            LogLine(msg, ConsoleColor.Yellow);
        }

        public void LogError(string msg)
        {
            LogLine(msg, ConsoleColor.Red);
        }

        public void Clear()
        {
            CurrentParagraph?.Inlines.Clear();
        }
    }
}
