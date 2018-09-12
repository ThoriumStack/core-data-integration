using MyBucks.Core.DataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyBucks.Core.DataIntegration
{
    public class OutputBuilder : IOutputBuilder
    {
        private readonly MemoryStream _memory;
        private IIntegrationDataSerializer _serializer;

        private readonly List<Action> _buildActions = new List<Action>();
        private List<IPreprocessor> _preProcessors = new List<IPreprocessor>();

        private List<object> _allItems = new List<object>();
        private Boolean _resetMemoryBufferOnAdd = false;

        public OutputBuilder()
        {
            _memory = new MemoryStream();
        }

        /// <summary>
        /// Specify the serializer to use when serializing data.
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <remarks>You can change the serializer at any point</remarks>
        public IOutputBuilder SetSerializer(IIntegrationDataSerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public IOutputBuilder AddPreProcessor(IPreprocessor preProcessor)
        {
            _preProcessors.Add(preProcessor);
            return this;
        }

        /// <summary>
        /// Add a data set to the output data.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOutputBuilder AddListData<TData>(IEnumerable<TData> data)
        {
            var enumerable = data as IList<TData> ?? data.ToList();
            _allItems.AddRange(enumerable.Cast<object>());
            _buildActions.Add(() =>
            {
                var rawData = _serializer.GenerateRawData(enumerable);
                rawData.Seek(0, SeekOrigin.Begin);
                rawData.Position = 0;

                if (_resetMemoryBufferOnAdd)
                {
                    _memory.Position = 0;
                }

                rawData.CopyTo(_memory);
            });
            return this;
        }

        /// <summary>
        /// Add a data set to the output data.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOutputBuilder AddData<TData>(TData data)
        {
            _allItems.Add(data);
            _buildActions.Add(() =>
            {
                var rawData = _serializer.GenerateRawData(new List<TData> { data });
                rawData.Seek(0, SeekOrigin.Begin);
                rawData.Position = 0;


                if (_resetMemoryBufferOnAdd)
                {
                    _memory.Position = 0;
                }

                rawData.CopyTo(_memory);
            });
            return this;
        }

        public IOutputBuilder SetOptionResetMemoryBufferOnAdd(bool option)
        {
            _resetMemoryBufferOnAdd = option;
            return this;
        }

        public MemoryStream Build()
        {
            _allItems.ForEach(item => _preProcessors.ForEach(proc => proc.ProcessObject(item)));
            _buildActions.ForEach(action => action());
            return _memory;
        }
    }
}
