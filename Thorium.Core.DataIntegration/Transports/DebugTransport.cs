using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Thorium.Core.DataIntegration.Interfaces;

namespace Thorium.Core.DataIntegration.Transports
{
    public class DebugTransport : IIntegrationTransport
    {
        private MemoryStream _lastRawData;

        public MemoryStream CollectRawData()
        {
            return new MemoryStream();
        }

        public (bool, string) SendData(MemoryStream rawData)
        {
            _lastRawData = rawData;
            rawData.Position = 0;
            using (StreamReader sr = new StreamReader(rawData))
            {
                while (sr.Peek() >= 0)
                {
                    Debug.WriteLine(sr.ReadLine() ?? "");
                }
            }
            return (true, "");
        }

        public Action AfterSend { get; set; }
        public Action<Exception> ErrorAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MemoryStream GetLastRawData()
        {
            return _lastRawData;
        }
    }
}
