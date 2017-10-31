using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBucks.Core.DataIntegration
{
    public class InputBuilder : IInputBuilder
    {
        private IIntegrationDataSerializer _serializer;
        private MemoryStream _stream;

        private readonly List<Action> _buildActions = new List<Action>();
        private string _identifier = "unknown";
        private long _country = 0;
        private Action<Exception> _errorAction;

        public IInputBuilder SetSerializer(IIntegrationDataSerializer serializer)
        {
            _buildActions.Add(() => _serializer = serializer);
            return this;
        }

        public IInputBuilder SetData(MemoryStream stream)
        {
            _stream = stream;
            return this;
        }

        public IInputBuilder WithErrorAction(Action<Exception> errorAction)
        {
            _errorAction = errorAction;
            return this;
        }

        public IInputBuilder ReadOnce<TData, TDiscriminator>(Action<TData> assignAction, Func<TDiscriminator, bool> discriminator) where TDiscriminator : new() where TData : new()
        {
            _buildActions.Add(() => _serializer.ReadSingle(assignAction, discriminator, _stream));
            return this;
        }

        public IInputBuilder ReadMany<TData, TDiscriminator>(IList<TData> destination, Func<TDiscriminator, bool> discriminator) where TData : new() where TDiscriminator : new()
        {
            _buildActions.Add(() => _serializer.ReadMany(destination, discriminator, _stream));
            return this;
        }

        public void Build()
        {

            _buildActions.ForEach(action =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {

                    _errorAction?.Invoke(e);
                }
            }
            );

        }
    }
}
