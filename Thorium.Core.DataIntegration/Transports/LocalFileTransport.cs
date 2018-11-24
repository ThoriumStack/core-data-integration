using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Thorium.Core.DataIntegration.Interfaces;

namespace Thorium.Core.DataIntegration.Transports
{
    public class LocalFileTransport : IFileIntegrationTransport
    {
        private MemoryStream _lastRawData;

        public LocalFileTransport()
        {
        }

        public string FilePath { get; set; }
        public string CurrentTransportMethod => "localfile";
        public string PathSeperator => "" + Path.DirectorySeparatorChar;

        public MemoryStream CollectRawData()
        {
            MemoryStream mem = new MemoryStream();
            using (var textReader = File.OpenRead(FilePath))
            {
                textReader.CopyTo(mem);
                mem.Seek(0, SeekOrigin.Begin);
            }
            return mem;
        }

        public (bool, string) SendData(MemoryStream rawData)
        {
            _lastRawData = rawData;
            try
            {
                using (var fileStream = File.Create(FilePath))
                {
                    rawData.Seek(0, SeekOrigin.Begin);
                    rawData.Position = 0;
                    rawData.CopyTo(fileStream);
                    rawData.Close();
                }
                AfterSend?.Invoke();
                return (true, $"'{FilePath}' Created successfully");
            }
            catch (Exception e)
            {
                ErrorAction?.Invoke(e);
                return (false, e.Message);
            }
        }

        public Action AfterSend { get; set; }
        public Action<Exception> ErrorAction { get; set; }

        public MemoryStream GetLastRawData()
        {
            return _lastRawData;
        }

        public void RenameFile(string newFilePath)
        {
            try
            {
                File.Move(FilePath, newFilePath);
            }
            catch (Exception e)
            {
                ErrorAction?.Invoke(e);
            }
            
        }

        public void DeleteFile()
        {
            try
            {
                File.Delete(FilePath);
            }
            catch (Exception e)
            {
                ErrorAction?.Invoke(e);
            }
           
        }

        public void EnsureDirectory(string directoryPath)
        {
            try
            {
                var fileInfo = new FileInfo(directoryPath);
                if (fileInfo.Directory != null && !Directory.Exists(fileInfo.Directory.ToString()))
                {
                    fileInfo.Directory.Create();
                }
            }
            catch (Exception e)
            {
                ErrorAction?.Invoke(e);
            }
            
        }

        public List<string> ListFiles(string path, string matchPattern = null)
        {
            var files = Directory.GetFiles(path).ToList();
            return files.Where(c => matchPattern == null || Regex.IsMatch(c, matchPattern)).ToList();
        }

        public bool FileExists(string targetFileNameAndPath)
        {
            return File.Exists(targetFileNameAndPath);
        }
    }
}
