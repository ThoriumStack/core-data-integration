using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.DataIntegration.Interfaces
{

    /// <summary>
    /// A preprocessor allows you to manipulate data across datasets that are sent via the data integration service. 
    /// </summary>
    public interface IPreprocessor
    {
        void ProcessObject(object obj);
    }
}
