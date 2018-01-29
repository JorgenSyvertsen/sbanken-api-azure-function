using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace jas.bank.functions.api
{
    public static class IdentityServer
    {
        public static async Task<IdentityServerToken> GetToken(string userid, string clientid, string secret, string host)
        {
            var identityServerUrl = $"https://{host}/IdentityServer/connect/token";
            var credentials = $"{clientid}:{secret}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            var encodedText = System.Convert.ToBase64String(bytes);

            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=utf-8";
                wc.Headers[HttpRequestHeader.Authorization] = $"Basic {encodedText}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                var htmlResult = await wc.UploadStringTaskAsync(identityServerUrl, "grant_type=client_credentials");
                return JsonConvert.DeserializeObject<IdentityServerToken>(htmlResult);
            }
        }
    }
}