using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Polly;
using PollyKoans.Framework;
using RestSharp;

namespace PollyKoans
{
    public class _1_Retry_Koans
    {
        [Test]
        public void Handle_Exception_and_Retry()
        {
            //Replace FILL.__IN with value to make the test pass
            var count = FILL.__IN - 2;
            Policy
                .Handle<DivideByZeroException>()
                .Retry(1)
                .Execute(() => 8 / count++);
            Assert.That(count, Is_.Equal_To(2));
        }

        [Test]
        public void Handle_Exception_and_Retry_with_count()
        {
            var retries = 0;
            var policy = Policy
                .Handle<DivideByZeroException>()
                .Retry(FILL.__IN, (exception, retryCount, context) => { retries = retryCount; });
            try
            {
                policy.Execute(() => 8 / new[] {0}.First());
            }
            catch
            {
                ++retries;
            }

            Assert.That(retries, Is_.Equal_To(4));
        }

        [Test]
        public void Handle_Result_with_Fallback()
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.NotFound)
                .Fallback(new HttpResponseMessage(FILL.IN_STATUS_CODE));
            var result = policy.Execute(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            Assert.That(result.StatusCode, Is_.Equal_To(HttpStatusCode.OK));
        }

        [Test]
        public void Handle_multipe_exceptions_with_Fallback()
        {
            var whatHappened = "nothing yet";
            var policy = Policy
                .Handle<DivideByZeroException>()
                .Or<FILL_IN_EXEPTION>()
                .Fallback(() => whatHappened = "fallback");
            try
            {
                policy.Execute(() =>
                    {
                        var ignoreMe = new RestClient("should Url have protocol?")
                            .Execute(new RestRequest("foo"));
                    }
                );
            }
            catch (Exception)
            {
                whatHappened = "unexpected";
            }

            Assert.That(whatHappened, Is_.Equal_To("fallback"));
        }
    }
}