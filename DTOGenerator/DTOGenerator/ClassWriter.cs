using System.IO;

namespace DTOGenerator
{
    internal class ClassWriter
    {
        private readonly string _outputDirectoryPath;

        internal ClassWriter(string outputDirectoryPath)
        {
            _outputDirectoryPath = outputDirectoryPath;
            Directory.CreateDirectory(outputDirectoryPath);
        }

        internal void Write(string className, string classCode)
        {
            var dtoFileName = Path.Combine(_outputDirectoryPath, className + ".cs");
            using (var streamWriter = new StreamWriter(dtoFileName))
            {
                streamWriter.Write(classCode);
            }
        }
    }
}
