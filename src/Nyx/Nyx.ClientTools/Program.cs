using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nyx.Foundation;

namespace Nyx.ClientTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = new AppVersionInfo().GetCurrentVersion();
            var msg = "Message from Nyx.ClientTools";
            if (args.Any())
                msg += ": " + String.Join(", ", args);
            msg += "\nVersion: " + version;
            MessageBox.Show(msg, "Nyx", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
