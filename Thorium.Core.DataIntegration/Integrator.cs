using System;
using System.Collections.Generic;
using System.IO;
using Thorium.Core.DataIntegration.Interfaces;

namespace Thorium.Core.DataIntegration
{
    public class Integrator
    {
        public (bool, string) SendData(IOutputBuilder outputBuilder, IIntegrationTransport transport)
        {
            return transport.SendData(outputBuilder.Build());
        }

        public void ReceiveData(IInputBuilder builder, IIntegrationTransport transport)
        {
            MemoryStream rawData = transport.CollectRawData();
            builder.SetData(rawData);
            builder.Build();
            rawData.Close();
            rawData.Dispose();
        }
    }
}
