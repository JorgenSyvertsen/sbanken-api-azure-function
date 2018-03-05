using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System;
using System.Text;
using jas.bank.functions.model;

namespace jas.bank.functions.api
{
    public class Bank
    {
        private readonly string _userId;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _hostName;

        public Bank(string userId, string clientId, string secret, string hostName)
        {
            _userId = userId;
            _clientId = clientId;
            _secret = secret;
            _hostName = hostName;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accountUrl = $"https://{_hostName}/Bank/Api/v1/Accounts/{_userId}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                var result = await wc.DownloadStringTaskAsync(accountUrl);
                var list = JsonConvert.DeserializeObject<ListResult<Account>>(result);
                return list.items;
            }
        }

        public async Task<Account> GetAccount(string accountNumber)
        {
            var accountUrl = $"https://{_hostName}/Bank/Api/v1/Accounts/{_userId}/{accountNumber}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                var result = await wc.DownloadStringTaskAsync(accountUrl);
                var list = JsonConvert.DeserializeObject<ItemResult<Account>>(result);
                return list.item;
            }
        }

        public async Task<ListResult<Transaction>> GetTransactions(string accountNumber, int? index = null, int? length = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var sb = new StringBuilder();
            sb.Append($"https://{_hostName}/Bank/Api/v2/Transactions/{_userId}/{accountNumber}");

            if (index != null)
            {
                sb.Append($"?index={index}");
            }
            if (length != null)
            {
                sb.Append($"{QueryCharacter(sb.ToString())}length={length}");
            }
            if (startDate != null)
            {
                sb.Append($"{QueryCharacter(sb.ToString())}startDate={startDate.Value:yyyy-MM-dd}");
            }
            if (endDate != null)
            {
                sb.Append($"{QueryCharacter(sb.ToString())}endDate={endDate.Value:yyyy-MM-dd}");
            }

            var transactionsUrl = sb.ToString();
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                var result = await wc.DownloadStringTaskAsync(transactionsUrl);
                var list = JsonConvert.DeserializeObject<ListResult<Transaction>>(result);
                return list;
            }
        }

        public async Task<decimal> Transfer(TransferRequest request)
        {
            var transferUrl = $"https://{_hostName}/Bank/Api/v1/Transfers/{_userId}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            var json = JsonConvert.SerializeObject(request);
            var encodedValue = Encoding.UTF8.GetBytes(json);

            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                await wc.UploadDataTaskAsync(transferUrl, encodedValue);
            }

            var account = await GetAccount(request.fromAccount);
            return account.available;
        }

        private static string QueryCharacter(string url)
        {
            return url.Contains("?") ? "&" : "?";
        }
    }
}