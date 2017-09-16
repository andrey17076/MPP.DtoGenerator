using System.IO;
using GenerationLibrary;

namespace DTOGenerator
{
    internal class DtoWriter
    {
        private readonly string _outputDirectoryPath;

        internal DtoWriter(string outputDirectoryPath)
        {
            _outputDirectoryPath = outputDirectoryPath;
            Directory.CreateDirectory(outputDirectoryPath);
        }

        internal void Write(DtoClass dto)
        {
            var dtoFilePath = Path.Combine(_outputDirectoryPath, dto.FileName);
            using (var streamWriter = new StreamWriter(dtoFilePath))
            {
                streamWriter.Write(dto.Code);
            }
        }
    }
}
