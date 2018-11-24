using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thorium.Core.DataIntegration.Interfaces
{
    public interface IOutputBuilder
    {
        IOutputBuilder SetSerializer(IIntegrationDataSerializer serializer);
        IOutputBuilder AddListData<TData>(IEnumerable<TData> data);
        IOutputBuilder AddData<TData>(TData data);

        MemoryStream Build();
        IOutputBuilder AddPreProcessor(IPreprocessor preProcessor);

        IOutputBuilder SetOptionResetMemoryBufferOnAdd(bool option);
    }
}
