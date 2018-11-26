using System;
using System.Collections.Generic;
using System.IO;
using Thorium.Core.DataIntegration;
using Thorium.Core.DataIntegration.PreProcessors;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var transport = new Thorium.Core.DataIntegration.Transports.MockFileSystemTransport();
            //var serializer = new MyBucks.Core.DataIntegration.se
            
            var outputBuilder = new OutputBuilder()
                .AddPreProcessor(new DiacriticRemover())
                .AddPreProcessor(new RegexRemover("."))
                
        }

        public class FakeSerializer : Thorium.Core.DataIntegration.Interfaces.IIntegrationDataSerializer
        {
            public MemoryStream GenerateRawData<TData>(IEnumerable<TData> data)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TData> GetData<TData>(MemoryStream rawData) where TData : new()
            {
                throw new NotImplementedException();
            }

            public void ReadMany<TData, TDiscriminator>(IList<TData> destination, Func<TDiscriminator, bool> discriminator, MemoryStream stream)
                where TData : new()
                where TDiscriminator : new()
            {
                throw new NotImplementedException();
            }

            public void ReadSingle<TData, TDiscriminator>(Action<TData> assignAction, Func<TDiscriminator, bool> discriminator, MemoryStream rawData)
                where TData : new()
                where TDiscriminator : new()
            {
                throw new NotImplementedException();
            }
        }
    }
}
