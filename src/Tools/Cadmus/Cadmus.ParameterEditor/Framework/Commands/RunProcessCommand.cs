using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.Foundation;
using Cadmus.Parametrizer;
using Cadmus.VisualFoundation.Framework;
using Cadmus.VisualFoundation.Framework.Commands;

namespace Cadmus.ParameterEditor.Framework.Commands
{
    public class RunProcessCommand : CommandBase
    {
        public string ExecutablePath { get; protected set; }

        public string Arguments { get; protected set; }

        public Process Process { get; protected set; }

        public string WorkingFolder { get; protected set; }

        public string OriginalTitle { get; protected set; }

        public ILogger Logger { get; protected set; }

        public bool IsRunning => Process != null && !Process.HasExited;

        public bool DontClearLog { get; protected set; }

        public override string Title => IsRunning ? $"Stop {OriginalTitle}" : OriginalTitle;

        public ConfigManager Config { get; protected set; }

        public bool Confirmation { get; protected set; }

        public override void Execute()
        {
            if (!IsRunning)
                StartProcess();
            else
                StopProcess();
            OnPropertyChanged(nameof(Title));
        }

        public static RunProcessCommand FromOperation(RunOperation operation, ILogger logger, ConfigManager config)
        {
            return new RunProcessCommand()
            {
                Title = operation.Title,
                OriginalTitle = operation.Title,
                ExecutablePath = operation.ExecutablePath,
                Arguments = operation.Arguments,
                WorkingFolder = operation.WorkingFolder,
                Logger = logger,
                DontClearLog = operation.DontClearLog,
                Config = config,
                Confirmation = operation.Confirmation
            };
        }

        private void StopProcess()
        {
            if (Process != null)
            {
                if (!Process.CloseMainWindow())
                    Process.Kill();
                Process.WaitForExit(3000);
            }
        }

        private void StartProcess()
        {
            if (Confirmation)
            {
                var msg = MessageBox.Show($"Are you sure you want to run operation \"{Title}\"?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (msg != MessageBoxResult.Yes)
                    return;
            }

            if (!DontClearLog)
                Logger.Clear();

            var arguments = PrepareArguments();
            Logger.LogInfo($"Starting {ExecutablePath} {arguments}");
            Logger.LogInfo($"Working folder is {WorkingFolder}");

            var si = new ProcessStartInfo();
            si.FileName = ExecutablePath;
            si.Arguments = arguments;
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            //si.RedirectStandardInput = true;
            si.UseShellExecute = false;
            si.CreateNoWindow = true;
            si.StandardOutputEncoding = Encoding.GetEncoding(852);
            si.StandardErrorEncoding = si.StandardOutputEncoding;
            si.WorkingDirectory = WorkingFolder;

            if (Process != null)
            {
                Process.OutputDataReceived -= Process_OutputDataReceived;
                Process.ErrorDataReceived -= Process_OnErrorDataReceived;
                Process.Exited -= Process_Exited;
            }

            Process = new Process();
            Process.StartInfo = si;
            Process.EnableRaisingEvents = true;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_OnErrorDataReceived;
            Process.Exited += Process_Exited;
            Process.Start();

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        private string PrepareArguments()
        {
            return Arguments.Replace("{config}", $"\"{Config.ConfigPath}\"");
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Title));
            Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            if (Process.ExitCode == 0)
                Logger.LogSuccess("Process successfully completed");
            else
                Logger.LogError($"Process exited with code {Process.ExitCode}");
        }

        private void Process_OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.LogError(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogRouter.Route(Logger, e.Data);
        }
    }
}
