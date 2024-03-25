using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace server.Utils
{
  public static class JsonUtils
  {
    public static string ParseJson(object obj)
    {
      var jsonSerialize = new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
      };
      var json = JsonConvert.SerializeObject(obj, jsonSerialize);
      return json;
    }
  }
}