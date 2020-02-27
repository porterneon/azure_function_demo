using System.IO;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class FileReader
    {
        public static async Task<string> ReadFromFileAsync(string requestBodyPath)
        {
            var directory = Directory.GetCurrentDirectory();
            var filepath = $"{directory}\\{requestBodyPath}";

            var reader = new StreamReader(filepath);
            return await reader.ReadToEndAsync();
        }
    }
}