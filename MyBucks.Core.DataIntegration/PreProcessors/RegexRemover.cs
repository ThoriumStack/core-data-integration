using System;
using System.Linq;
using System.Text.RegularExpressions;
using MyBucks.Core.DataIntegration.Interfaces;

namespace Atlas.Modules.DataIntegration.Service.PreProcessors
{
    /// <summary>
    /// Removes anything that matches a specific regex pattern.
    /// </summary>
    public class RegexRemover : IPreprocessor
    {
        private readonly string _pattern;

        public RegexRemover(string pattern)
        {
            _pattern = pattern;
        }

        public void ProcessObject(object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var prop in props.Where(c => c.PropertyType == typeof(string)))
            {
                var str = (string)prop.GetValue(obj);

                if (str == null) continue;

                try
                {
                    str = Regex.Replace(str, _pattern, string.Empty);
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