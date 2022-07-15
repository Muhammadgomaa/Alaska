using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace Alaska
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;


        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AY8q3CKiXM4eUarrW8o7BPfnEnuqvdQ7aONZGk1lC1Pay4JilJSDw9_Es61ob78ui4JE3KVGBdnBFqMF";
            clientSecret = "EC1LOjelzJeNoVEfNfuPaKFNTIeQQPrZdRCYsDRu3hNJI50JI6uxbI4EgoeshuLyewml4V6KQ7SjPVuH";
        }

        private static Dictionary<string, string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apicontext = new APIContext(GetAccessToken());
            apicontext.Config = getconfig();
            return apicontext;
        }
    }
}