using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.DataIntegration.Attributes
{
    public class ColumnHeaderAttribute : System.Attribute
    {
        public ColumnHeaderAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
