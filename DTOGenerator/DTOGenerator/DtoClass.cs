using System.IO;

namespace DTOGenerator
{
    internal class DtoClass
    {
        private readonly string _fileName;
        private readonly string _code;

        internal DtoClass(string name, string code)
        {
            _fileName = name + ".cs";
            _code = code;
        }

        internal void Write(string outputDirectoryPath)
        {
            Directory.CreateDirectory(outputDirectoryPath);
            var dtoFilePath = Path.Combine(outputDirectoryPath, _fileName);
            using (var streamWriter = new StreamWriter(dtoFilePath))
            {
                streamWriter.Write(_code);
            }
        }
    }
}
