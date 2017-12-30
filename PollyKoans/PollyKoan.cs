using System;
using System.Linq;
using NUnit.Framework;
using Polly;
using System.Net.Http;
using System.Net;
using System.Threading;
using Polly.Timeout;
using RestSharp;
using System.Threading.Tasks;

namespace PollyKoans
{
    public class Timeout_Cancel_Retry
    {
        [Test]
        public void Handle_and_Retry()
        {
            //Replace Fill.__in with value to make the test pass
            var count = Fill.__in - 2;
            Policy
                .Handle<DivideByZeroException>()
                .Retry(1)
                .Execute(() => 8 / count++);
            Assert.That(count, Is_.EqualTo_(2));
        }

        [Test]
        public void Handle_and_Retry_With_Count()
        {
            var retries = 0;
            var policy = Policy
                .Handle<DivideByZeroException>()
                .Retry(Fill.__in, (exception, retryCount, context) =>
                {
                    retries = retryCount;
                });
            try
            {
                policy.Execute(() => 8 / new int[] { 0 }.First());
            }
            catch
            {
                ++retries;
            }
            Assert.That(retries, Is_.EqualTo_(4));
        }

        [Test]
        public void Handle_Result_with_Fallback() {
            var policy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.NotFound)
            .Fallback(new HttpResponseMessage(Fill.status_code));
            var result = policy.Execute(() => {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            });
            Assert.That(result.StatusCode, Is_.EqualTo_(HttpStatusCode.OK));
        }

        [Test]
        public void Pessimistic_Timeout() {
            var result = "not timed out";
            var policy = Policy
            .Timeout(TimeSpan.FromMilliseconds(Fill.__in + 1000), TimeoutStrategy.Pessimistic);
            try
            {
                policy.Execute(() => Thread.Sleep(500));
            }
            catch {
                result = "timed out";
            }
            Assert.That(result, Is_.EqualTo_("timed out"));
        }

        [Test]
        public async Task Timeout_Asynch_Optimistic_With_Token() {
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource();
            var timeoutPolicy = Policy.TimeoutAsync(500, TimeoutStrategy.Optimistic);            
            var whatHappened = "";
            cancellationTokenSource.CancelAfter(Fill.__in + 1000);
            try
            {
                var httpResponse = await timeoutPolicy
                    .ExecuteAsync(async ct =>
                    {
                        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("http://tempuri.org", ct);
                        Thread.Sleep(100);
                        return httpResponseMessage;
                    }, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException) {
                whatHappened = "cancelled";
            }
            catch (TimeoutException)
            {
                whatHappened = "timeout";
            }
            catch(Exception e) {
                var y = e.InnerException;
                whatHappened = "unexpected";
            }
            Assert.That(whatHappened, Is_.EqualTo_("cancelled"));
        }

    }
}