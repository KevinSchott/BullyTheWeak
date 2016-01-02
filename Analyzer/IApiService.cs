using Newtonsoft.Json.Linq;

namespace Analyzer
{
    public enum ApiRegion
    {
        NA,
        EUW
    }

    public interface IApiService
    {
        JObject GetData(ApiRegion region, string requestParams);
    }
}
