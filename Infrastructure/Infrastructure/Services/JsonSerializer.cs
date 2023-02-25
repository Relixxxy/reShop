using Infrastructure.Services.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
        {
           return JsonConvert.SerializeObject(data);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value) !;
        }
    }
}