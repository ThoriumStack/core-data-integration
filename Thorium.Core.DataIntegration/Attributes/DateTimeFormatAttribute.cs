using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.DataIntegration.Attributes
{
    public class DateTimeFormatAttribute : Attribute
    {
        public string DateTimeFormat { get; set; }

        public DateTimeFormatAttribute(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
    }
}
