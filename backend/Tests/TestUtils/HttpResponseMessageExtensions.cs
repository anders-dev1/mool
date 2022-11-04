using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tests.TestUtils
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ReadAsType<T>(this HttpResponseMessage message)
        {
            var responseContent = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}