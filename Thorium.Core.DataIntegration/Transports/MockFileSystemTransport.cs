using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thorium.Core.DataIntegration.Interfaces;

namespace Thorium.Core.DataIntegration.Transports
{
    /// <summary>
    /// Used for unit testing
    /// </summary>
    public class MockFileSystemTransport : IIntegrationTransport
    {
        private MemoryStream _lastRawData;
        public static List<ImaginaryFile> Files { get; set; } = new List<ImaginaryFile>();

        public string Filename { get; set; }

        public MemoryStream CollectRawData()
        {
            return Files.FirstOrDefault(c => c.Name == Filename)?.FileContents ?? new MemoryStream();
        }

        public (bool, string) SendData(MemoryStream rawData)
        {
            _lastRawData = rawData;
            var file = Files.FirstOrDefault(c => c.Name == Filename);
            if (file != null)
            {
                file.FileContents = rawData;
            }
            else
            {
                Files.Add(new ImaginaryFile()
                {
                    Name = Filename,
                    FileContents = rawData
                });
            }
            AfterSend?.Invoke();
            return (true,"");
        }

        public Action AfterSend { get; set; }
        public Action<Exception> ErrorAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string PathSeperator => throw new NotImplementedException();

        public string CurrentTransportMethod => throw new NotImplementedException();

        public MemoryStream GetLastRawData()
        {
            return _lastRawData;
        }

        public void DeleteFile()
        {
            throw new NotImplementedException();
        }

        public List<string> ListFiles(string directory = ".", string matchExpression = null)
        {
            throw new NotImplementedException();
        }

        public void RenameFile(string newPath)
        {
            throw new NotImplementedException();
        }

        public void EnsureDirectory(string DirectoryPath)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string targetFileNameAndPath)
        {
            throw new NotImplementedException();
        }
    }

    public class ImaginaryFile
    {
        public string Name { get; set; }
        public MemoryStream FileContents { get; set; }
    }
}
