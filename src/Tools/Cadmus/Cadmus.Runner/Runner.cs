using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.Runner
{
    public class Runner
    {
        public string ExecutablePath { get; set; }

        public string Arguments { get; set; }

        public string WorkingFolder { get; set; }

        public Process Process { get; protected set; }

        public ILogger Logger { get; protected set; }

        public Runner(ILogger logger)
        {
            Logger = logger;
        }

        public void Start()
        {
            var si = new ProcessStartInfo();
            si.FileName = ExecutablePath;
            si.Arguments = Arguments;
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            //si.RedirectStandardInput = true;
            si.UseShellExecute = false;
            si.CreateNoWindow = true;
            si.StandardOutputEncoding = Encoding.GetEncoding(852);
            si.StandardErrorEncoding = si.StandardOutputEncoding;
            si.WorkingDirectory = WorkingFolder;

            Process = new Process();
            Process.StartInfo = si;
            Process.EnableRaisingEvents = true;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_OnErrorDataReceived;
            Process.Start();

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
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
