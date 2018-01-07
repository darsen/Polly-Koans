using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Polly;
using Polly.Timeout;
using PollyKoans.Framework;

namespace PollyKoans
{
    public class _3_Timeout_Koans
    {
        [Test]
        public void Pessimistic_Timeout()
        {
            var result = "not timed out";
            var policy = Policy
                .Timeout(TimeSpan.FromMilliseconds(FILL.__IN + 1000), TimeoutStrategy.Pessimistic);
            try
            {
                policy.Execute(() => Thread.Sleep(500));
            }
            catch
            {
                result = "timed out";
            }

            Assert.That(result, Is_.Equal_To("timed out"));
        }

        [Test]
        public async Task Optimistic_Timeout_with_Cancellation_Token()
        {
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource();
            var timeoutPolicy = Policy.TimeoutAsync(500, TimeoutStrategy.Optimistic);
            var whatHappened = "nothing yet";
            cancellationTokenSource.CancelAfter(FILL.__IN + 1000);
            try
            {
                var httpResponse = await timeoutPolicy
                    .ExecuteAsync(async ct =>
                    {
                        var httpResponseMessage = await httpClient.GetAsync("http://tempuri.org", ct);
                        Thread.Sleep(100);
                        return httpResponseMessage;
                    }, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                whatHappened = "cancelled";
            }
            catch (TimeoutException)
            {
                whatHappened = "timeout";
            }
            catch (Exception e)
            {
                var y = e.InnerException;
                whatHappened = "unexpected";
            }

            Assert.That(whatHappened, Is_.Equal_To("cancelled"));
        }
    }
}