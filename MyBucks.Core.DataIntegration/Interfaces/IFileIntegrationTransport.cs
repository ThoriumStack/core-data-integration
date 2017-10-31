using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.DataIntegration.Interfaces
{
    public interface IFileIntegrationTransport : IIntegrationTransport
    {
        string FilePath { get; set; }
        void DeleteFile();
        List<string> ListFiles(string directory = ".", string matchExpression = null);
        void RenameFile(string newPath);
        string PathSeperator { get; }
        void EnsureDirectory(string DirectoryPath);
        bool FileExists(string targetFileNameAndPath);
        string CurrentTransportMethod { get; }
    }
}
