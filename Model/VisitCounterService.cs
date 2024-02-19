using System.IO;
using System.Threading.Tasks;

namespace Portfolio.Model
{
    public class VisitCounterService
    {
        private readonly string _filePath;

        public VisitCounterService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<int> GetVisitCountAsync()
        {
            if (!File.Exists(_filePath))
            {
                return 0;
            }

            var content = await File.ReadAllTextAsync(_filePath);
            if (int.TryParse(content, out int count))
            {
                return count;
            }

            return 0;
        }

        public async Task IncrementVisitCountAsync()
        {
            var count = await GetVisitCountAsync();
            count++;
            await File.WriteAllTextAsync(_filePath, count.ToString());
        }
    }
}
