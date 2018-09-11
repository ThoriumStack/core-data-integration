using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.DataIntegration.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ExcelAttribute : System.Attribute
    {
        public bool ShowHeader { get; } = true;

        public String Column { get; } = "A";
        public int ColumnNumber { get; }

        public int Row { get; } = 1;

        public ExcelAttribute(String column, int row, Boolean showHeader = true)
        {
            Column = column;
            Row = row;
            ShowHeader = showHeader;
            ColumnNumber = ExcelAttribute.ExcelColumnNameToNumber(column);
        }

        private static int ExcelColumnNameToNumber(String columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }
    }
}
