using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Polly;
using PollyKoans.Framework;
using RestSharp;

namespace PollyKoans
{
    public class _2_Fallback_Koans
    {
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
