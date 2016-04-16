using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class OperationWrapper : IOperation
    {
        private readonly IInfoLogger _logger;

        public OperationWrapper(IOperation operation, IInfoLogger logger)
        {
            _logger = logger;
            Operation = operation;
            ShowInfo = true;
        }

        public void Execute()
        {
            if (ShowInfo)
            {
                _logger.ShowInfo();
                Console.WriteLine("Operation: " + Name);
                Operation.ShowInfo();
            }

            if (!Silent)
            {
                var sure = _logger.AreYouSure("Are you sure you want to continue?");
                if (!sure)
                    return;
            }

            Operation.Execute();

            if (!Silent)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
        }

        public IOperation Operation { get; }

        public bool Silent { get; set; }

        public bool ShowInfo { get; set; }

        public string Name => Operation.Name;

        void IOperation.ShowInfo()
        {
            Operation.ShowInfo();
        }
    }
}
