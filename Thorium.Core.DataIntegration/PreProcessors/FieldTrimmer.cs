using System;
using System.Linq;
using Thorium.Core.DataIntegration.Interfaces;

namespace Thorium.Core.DataIntegration.PreProcessors
{
    public class FieldTrimmer : IPreprocessor
    {
        public void ProcessObject(object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var prop in props.Where(c => c.PropertyType == typeof(string)))
            {
                var str = (string)prop.GetValue(obj);

                if (str == null) continue;

                str = str.Trim();

                try
                {
                    prop.SetValue(obj, str, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }
}
