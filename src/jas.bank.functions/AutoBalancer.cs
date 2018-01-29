using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using jas.bank.functions.api;

namespace jas.bank.functions
{
    public static class AutoBalancer
    {
        static string _userid = "<censored>";
        static string _clientid = "<censored>";
        static string _secret = "<censored>";
        static string _hostname = "api.sbanken.no";

        static int _debetThreshold = 1000;
        static int _salaryThreshold = 1000;
        static string _debetAccount = "<censored>";
        static string _salaryAccount = "<censored>";

        [FunctionName("AutoBalancer")]
        public static async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                var bank = new Bank(_userid, _clientid, _secret, _hostname);
                var debetAccount = await bank.GetAccount(_debetAccount);
                if (debetAccount.available < _debetThreshold)
                {
                    var missing = _debetThreshold - debetAccount.available;

                    var salaryAccount = await bank.GetAccount(_salaryAccount);
                    if (salaryAccount.available - missing > _salaryThreshold)
                    {
                        var remainingInAccount = await bank.Transfer(new TransferRequest
                        {
                            fromAccount = salaryAccount.accountNumber,
                            toAccount = debetAccount.accountNumber,
                            amount = missing,
                            message = "Autobalancing account"
                        });
                        log.Info($"Remaining in account: {remainingInAccount}");
                    }
                    else
                    {
                        log.Info("Insufficient funds in account to autobalance");
                    }
                }
                else
                {
                    log.Info("Sufficient funds in account");
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
                throw;
            }
            log.Info($"Account autobalancer function executed at: {DateTime.Now}");
        }
    }
}