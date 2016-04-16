using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp.Interfaces
{
    public interface IInfoLogger
    {
        void ShowInfo();

        void ShowVersion();

        bool AreYouSure(string message);
    }
}
