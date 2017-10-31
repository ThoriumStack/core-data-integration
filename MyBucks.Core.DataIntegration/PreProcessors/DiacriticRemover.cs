using System;
using System.Linq;
using MyBucks.Core.DataIntegration.Interfaces;
using System.Text;
using System.Globalization;

namespace Atlas.Modules.DataIntegration.Service.PreProcessors
{

    /// <summary>
    /// Removes accents (diacritics) from all string properties
    /// </summary>
    public class DiacriticRemover : IPreprocessor
    {
        public void ProcessObject(object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var prop in props.Where(c => c.PropertyType == typeof(string)))
            {
                var str = (string)prop.GetValue(obj);

                if (str == null) continue;
                    
                str = RemoveDiacritics(str);

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

        /// <summary>
        /// Removes accents (diacritics) from a string and replaces it with it's root letter.
        /// </summary>
        /// <see cref="http://archives.miloush.net/michkap/archive/2007/05/14/2629747.html"/>
        /// <param name="text"></param>
        /// <returns></returns>
        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


    }
}