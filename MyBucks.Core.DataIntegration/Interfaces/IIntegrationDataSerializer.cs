using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBucks.Core.DataIntegration.Interfaces
{
    public interface IIntegrationDataSerializer
    {
        IEnumerable<TData> GetData<TData>(MemoryStream rawData) where TData : new();

        MemoryStream GenerateRawData<TData>(IEnumerable<TData> data);
        void ReadSingle<TData, TDiscriminator>(Action<TData> assignAction, Func<TDiscriminator, bool> discriminator, MemoryStream rawData) where TData : new() where TDiscriminator : new();
        void ReadMany<TData, TDiscriminator>(IList<TData> destination, Func<TDiscriminator, bool> discriminator, MemoryStream stream) where TData : new() where TDiscriminator : new();
    }
}
