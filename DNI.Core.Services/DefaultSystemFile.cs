﻿using DNI.Core.Contracts;
using System.IO;

namespace DNI.Core.Services
{
    internal sealed class DefaultSystemFile : IFile
    {
        private FileStream fileStream;
        public string Path { get; }
        public string Name { get; }

        public string FullPath => System.IO.Path.Combine(Path, Name);

        public bool Exists => File.Exists(FullPath);

        public void Dispose()
        {
            fileStream?.Dispose();
        }

        public FileStream GetFileStream()
        {
            if(Exists)
                return fileStream = File.OpenRead(FullPath);

            return default;
        }

        public DefaultSystemFile(string fileName)
        {
            Name = System.IO.Path.GetFileName(fileName);
            Path = fileName.Replace(Name, string.Empty);
        }
    }
}