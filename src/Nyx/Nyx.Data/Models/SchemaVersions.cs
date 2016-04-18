using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Data.Models
{
    public class SchemaVersions
    {
        public int Id { get; set; }

        public string ScriptName { get; set; }

        public DateTime Applied { get; set; }
    }
}
