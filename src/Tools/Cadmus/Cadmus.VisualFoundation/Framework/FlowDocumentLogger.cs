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
    public class FlowDocumentLogger : LoggerBase
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

        public override void LogInfo(string msg)
        {
            LogInternal(msg, null, true);
        }

        private void LogInfo(string msg, ConsoleColor color)
        {
            LogInternal(msg, color, true);
        }

        public override void LogSuccess(string msg)
        {
            LogInfo(msg, ConsoleColor.Green);
        }

        public override void LogWarning(string msg)
        {
            LogInfo(msg, ConsoleColor.Yellow);
        }

        public override void LogError(string msg)
        {
            LogInfo(msg, ConsoleColor.Red);
        }

        public override void LogHeader(string msg)
        {
            LogInfo(msg, ConsoleColor.Cyan);
        }

        public override void Clear()
        {
            CurrentParagraph?.Inlines.Clear();
        }
    }
}
