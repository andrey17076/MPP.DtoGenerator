namespace GenerationLibrary
{
    public class DtoClass
    {

        public DtoClass(string name)
        {
            FileName = name + ".cs";
        }

        public DtoClass(string name, string code) : this(name)
        {
            Code = code;
        }

        public string FileName { get; private set; }
        public string Code { get; private set; }
    }
}
