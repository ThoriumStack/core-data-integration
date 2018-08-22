using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBucks.Core.DataIntegration.Transports
{
    public class StreamTransport : IIntegrationTransport
    {
        public Action AfterSend { get; set; }
        public Action<Exception> ErrorAction { get; set; }

        public Stream InputStream { get; set; }

        private MemoryStream _stream;

        public MemoryStream CollectRawData()
        {
            _stream = new MemoryStream();
            InputStream.Position = 0;
            InputStream.CopyTo(_stream);
            _stream.Position = 0;
            return _stream;
        }

        public MemoryStream GetLastRawData()
        {
            return _stream;
        }

        public (bool, string) SendData(MemoryStream rawData)
        {
            throw new NotImplementedException();
        }
    }
}
