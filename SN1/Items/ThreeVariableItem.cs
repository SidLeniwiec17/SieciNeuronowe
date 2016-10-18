using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN1.Items
{
    public class ThreeVariableItem : ItemInterface
    {
        public ThreeVariableItem()
        {

        }
        public double x { get; set; }
        public double y { get; set; }
        public int cls { get; set; }
    }
}
