using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBucks.Core.DataIntegration.Interfaces
{
    public interface IIntegrationTransport
    {
        MemoryStream CollectRawData();

        (bool, string) SendData(MemoryStream rawData);

        Action AfterSend { get; set; }
        Action<Exception> ErrorAction { get; set; }

        MemoryStream GetLastRawData();
    }
}
