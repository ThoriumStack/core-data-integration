using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyBucks.Core.DataIntegration
{
    public class InputBuilder : IInputBuilder
    {
        private IIntegrationDataSerializer _serializer;
        private MemoryStream _stream;

        private readonly List<Action> _buildActions = new List<Action>();
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

        /// <summary>
        /// Add an error action if an error occurs on read
        /// </summary>
        /// <param name="errorAction"></param>
        /// <returns></returns>
        public IInputBuilder WithErrorAction(Action<Exception> errorAction)
        {
            _errorAction = errorAction;
            return this;
        }

        /// <summary>
        /// Read one of a record type. i.e. headers and footers
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TDiscriminator"></typeparam>
        /// <param name="assignAction"></param>
        /// <param name="discriminator"></param>
        /// <returns></returns>
        public IInputBuilder ReadOnce<TData, TDiscriminator>(Action<TData> assignAction, Func<TDiscriminator, bool> discriminator) where TDiscriminator : new() where TData : new()
        {
            _buildActions.Add(() => _serializer.ReadSingle(assignAction, discriminator, _stream));
            return this;
        }

        /// <summary>
        /// Read multiple records by a discriminating factor
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TDiscriminator"></typeparam>
        /// <param name="destination"></param>
        /// <param name="discriminator"></param>
        /// <returns></returns>
        public IInputBuilder ReadMany<TData, TDiscriminator>(IList<TData> destination, Func<TDiscriminator, bool> discriminator) where TData : new() where TDiscriminator : new()
        {
            _buildActions.Add(() => _serializer.ReadMany(destination, discriminator, _stream));
            return this;
        }

        /// <summary>
        /// Read a data stream with only one type of record
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="destination"></param>
        /// <returns></returns>
        public IInputBuilder ReadAll<TData>(IList<TData> destination) where TData : new()
        {
            _buildActions.Add(() =>
            {
                var result = _serializer.GetData<TData>(_stream);
                result.ToList().ForEach(c => destination.Add(c));
            });
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
