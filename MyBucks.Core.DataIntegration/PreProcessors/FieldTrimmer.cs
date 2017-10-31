using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Modules.DataIntegration.Service.PreProcessors
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
