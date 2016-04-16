using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp
{
    public enum TransactionOption
    {
        NoTransaction,
        Transaction,
        TransactionPerScript
    }
}
