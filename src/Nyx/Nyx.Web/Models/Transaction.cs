using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nyx.Web.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Purpose { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Note { get; set; }

        public DateTime Created { get; set; }
    }
}