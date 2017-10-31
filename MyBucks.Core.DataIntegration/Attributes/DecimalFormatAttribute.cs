﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.DataIntegration.Attributes
{
    public class DecimalFormatAttribute : Attribute
    {
        public string DecimalFormat { get; set; }

        public DecimalFormatAttribute(string decimalFormat)
        {
            DecimalFormat = decimalFormat;
        }
    }
}
