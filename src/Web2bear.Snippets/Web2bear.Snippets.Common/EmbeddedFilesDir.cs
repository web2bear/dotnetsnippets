using System.IO;
using System.Reflection;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Web2bear.Snippets.Common
{
    public class EmbeddedFilesDir
    {
        private readonly Assembly _sourceAssembly;
        private readonly string _sourceNamespace;

        public static EmbeddedFilesDir Create<T>() where T : class
        {
            var sorceType = typeof(T);
            return new EmbeddedFilesDir(sorceType.Assembly, sorceType.Namespace);
        }


        public EmbeddedFilesDir(Assembly sourceAssembly, string sourceNamespace)
        {
            _sourceAssembly = sourceAssembly;
            _sourceNamespace = sourceNamespace;
        }

        public string ReadTextFile(string fileName)
        {
            using (var stream = _sourceAssembly.GetManifestResourceStream($"{_sourceNamespace}.{fileName}"))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T ReadJsonObject<T>(string fileName)
        {
            var jsonText = ReadTextFile(fileName);
            return JsonSerializer.Deserialize<T>(jsonText, StandardResolver.Default);
        }
    }
}