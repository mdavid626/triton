﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp.Interfaces
{
    public interface IConnectionStringBuilder
    {
        void ShowInfo();

        string ConnectionString { get; }
    }
}