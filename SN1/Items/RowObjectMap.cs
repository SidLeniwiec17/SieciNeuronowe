using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN1.Items
{
    public sealed class RowObjectMap : CsvClassMap<RowObject>
    {
        public RowObjectMap()
        {
            Map(m => m.x).Index(0);
            Map(m => m.y).Index(1).Default(null);
            Map(m => m.cls).Index(2).Default(null);       
        }
    }
}
