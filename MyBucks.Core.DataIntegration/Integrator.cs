using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyBucks.Core.DataIntegration
{
    public class Integrator
    {
        //public (bool, string) SendData<TData>(IEnumerable<TData> data, IIntegrationDataSerializer serializer, IIntegrationTransport transport) where TData : class
        //{
        //    MemoryStream rawData = serializer.GenerateRawData(data);
        //    return transport.SendData(rawData);
        //}

        public (bool, string) SendData(IOutputBuilder outputBuilder, IIntegrationTransport transport)
        {
            return transport.SendData(outputBuilder.Build());
        }

        //public IEnumerable<TData> ReceiveData<TData>(IIntegrationDataSerializer deserialiser, IIntegrationTransport transport) where TData : class, new()
        //{
        //    MemoryStream rawData = transport.CollectRawData();
        //    var result = deserialiser.GetData<TData>(rawData);
        //    rawData.Close();
        //    rawData.Dispose();
        //    return result;
        //}

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
