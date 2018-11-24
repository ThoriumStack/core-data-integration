using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thorium.Core.DataIntegration.Interfaces
{
    public interface IInputBuilder
    {
        IInputBuilder SetSerializer(IIntegrationDataSerializer serializer);
        IInputBuilder SetData(MemoryStream stream);
        IInputBuilder ReadOnce<TData, TDiscriminator>(Action<TData> assignAction, Func<TDiscriminator, bool> discriminator) where TData : new() where TDiscriminator : new();
        IInputBuilder ReadMany<TData, TDiscriminator>(IList<TData> destination, Func<TDiscriminator, bool> discriminator) where TData : new() where TDiscriminator : new();
        IInputBuilder ReadAll<TData>(IList<TData> destination) where TData : new();
        IInputBuilder WithErrorAction(Action<Exception> errorAction);
        void Build();
    }
}
