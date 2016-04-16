using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class ApplicationArguments : IApplicationArguments
    {
        public bool Help { get; set; }

        public bool Silent { get; set; }

        public bool Create { get; set; }

        public bool Drop { get; set; }

        public bool Upgrade { get; set; }

        public bool Version { get; set; }

        public int Timeout { get; set; }

        public TransactionOption TransactionOption { get; set; }
    }
}
