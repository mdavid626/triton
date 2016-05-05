using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows;

namespace Cadmus.ParameterEditor.Framework
{
    public class PowerShellExecutor
    {
        public string ComputerName { get; protected set; }

        public string Result { get; protected set; }

        public PowerShellExecutor(string computerName)
        {
            ComputerName = computerName;
        }

        public bool Execute()
        {
            var connectionInfo = new WSManConnectionInfo {ComputerName = ComputerName};
            using (var runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                try
                {
                    runspace.Open();
                }
                catch (System.Management.Automation.Remoting.PSRemotingTransportException ex)
                {
                    MessageBox.Show("Could not connect. Please try run editor as administrator: \n" + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = runspace;
                    ps.AddScript("'aaa'");
                    var results = ps.Invoke();
                    Result = string.Join("\n", results);
                }
                return true;
            }
        }
    }
}
