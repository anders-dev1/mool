using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Tests.TestUtils
{
    public static class ObjectToStringContentExtensions
    {
        public static StringContent ToJson(this object o)
        {
            var json = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            return json;
        }
    }
}