using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using jas.bank.functions.api;
using jas.bank.functions.model;

namespace jas.bank.functions
{
    public static class AutoBalancer
    {
        private const string Userid = "<censored>";
        private const string Clientid = "<censored>";
        private const string Secret = "<censored>";
        private const string Hostname = "api.sbanken.no";

        private const int DebetThreshold = 1000;
        private const int SalaryThreshold = 1000;
        private const string DebetAccount = "<censored>";
        private const string SalaryAccount = "<censored>";

        [FunctionName("AutoBalancer")]
        public static async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                var bank = new Bank(Userid, Clientid, Secret, Hostname);
                var debetAccount = await bank.GetAccount(DebetAccount);
                if (debetAccount.available < DebetThreshold)
                {
                    var missing = DebetThreshold - debetAccount.available;

                    var salaryAccount = await bank.GetAccount(SalaryAccount);
                    if (salaryAccount.available - missing > SalaryThreshold)
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