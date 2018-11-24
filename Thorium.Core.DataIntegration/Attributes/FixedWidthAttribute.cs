using System;
using System.Collections.Generic;
using System.Text;
using Thorium.Core.DataIntegration.Constants;

namespace Thorium.Core.DataIntegration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FixedWidthFieldAttribute : System.Attribute
    {
        public FixedWidthFieldAttribute(int width, char paddingCharacter = ' ', PaddingDirection paddingDirection = PaddingDirection.Right)
        {
            PaddingDirection = paddingDirection;
            PaddingCharacter = paddingCharacter;
            Width = width;
        }

        public int Width { get; set; }

        public char PaddingCharacter { get; set; }

        public PaddingDirection PaddingDirection { get; set; }
    }
}
