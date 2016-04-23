using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Data.Models
{
    public class DatabaseInfo
    {
        [Key]
        public Guid DatabaseId { get; set; }
    }
}
