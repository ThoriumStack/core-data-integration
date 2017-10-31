using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.DataIntegration.Interfaces
{

    /// <summary>
    /// A preprocessor allows you to manipulate data accross datasets that are sent via the data integration service. 
    /// </summary>
    public interface IPreprocessor
    {
        void ProcessObject(object obj);
    }
}
