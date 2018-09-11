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
        public Stream OutputStream { get; set; }

        private MemoryStream _stream;


        public StreamTransport()

        {
            OutputStream = new MemoryStream();
        }

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
            _stream = rawData;
            try
            {
                rawData.Seek(0, SeekOrigin.Begin);
                rawData.Position = 0;
                rawData.CopyTo(OutputStream);
                rawData.Close();

                AfterSend?.Invoke();
                return (true, $"Created successfully");
            }
            catch (Exception ex)
            {
                ErrorAction?.Invoke(ex);
                return (false, ex.Message);
            }
        }
    }
}
