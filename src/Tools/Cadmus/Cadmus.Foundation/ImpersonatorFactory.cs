using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class ImpersonatorFactory
    {
        public bool IsEnabled { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        public IDisposable Create()
        {
            if (IsEnabled)
                return new Impersonator(Username, Domain, Password);
            return new EmptyDisposable();
        }
    }
}
