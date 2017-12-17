using System;
using System.Linq;
using NUnit.Framework;
using Polly;
using System.Net.Http;
using System.Net;
using System.Threading;
using Polly.Timeout;

namespace PollyKoans
{
    public class PollyKoan
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
                policy.Execute(() => 8 / new int[]{0}.First());
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
        public void Timeout() {
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
   
    }
}