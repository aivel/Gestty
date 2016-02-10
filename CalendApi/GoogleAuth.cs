using System;
using System.Collections.Generic;
using System.Linq;
using ModernHttpClient;
using PortableRest;
using static System.String;

namespace GoogleApi
{
    public static class Scopes
    {
        private const string ReadOnlyModifier = ".readonly";
        private const string BaseUrl = "https://www.googleapis.com/auth/";

        public const string Calendar = "calendar";
        public static string CalendarReadOnly => Calendar + ReadOnlyModifier;

        public static string Make(IEnumerable<string> scopes)
        {
            return $"{BaseUrl}{Join(".", scopes)}";
        }
    }

    public class GoogleAuth
    {
        public class QueryStringBuilder
        {
            private readonly List<KeyValuePair<string, object>> _list;

            public QueryStringBuilder()
            {
                _list = new List<KeyValuePair<string, object>>();
            }

            public void Add(string name, object value)
            {
                _list.Add(new KeyValuePair<string, object>(name, value));
            }

            public override string ToString()
            {
                return Join("&", _list.Select(kvp => Concat(Uri.EscapeDataString(kvp.Key), "=", Uri.EscapeDataString(kvp.Value.ToString()))));
            }
        }

        private const string AuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private readonly string _clientId;
        private readonly string _scopes;
        private readonly string _loginHint;
        private readonly string _clientSecret;
        private readonly string _redirectUri = "urn:ietf:wg:oauth:2.0:oob:auto";
        private RestClient _client;
        private const string RequestTokenUrl = "https://www.googleapis.com/oauth2/v4/token";

        public GoogleAuth(string clientId, string clientSecret, string loginHint = "", string scopes = "")
        {
            _clientId = clientId;
            _scopes = scopes;
            _loginHint = loginHint;
            _clientSecret = clientSecret;
        }

        public string BuildRequestUrl(string state = "")
        {
            if (IsNullOrEmpty(_scopes))
            {
                throw new ArgumentNullException("You must specify Scopes");
            }

            var query = new QueryStringBuilder();

            query.Add("response_type", "code");
            query.Add("client_id", _clientId);
            query.Add("redirect_uri", _redirectUri);
            query.Add("scope", _scopes);
            query.Add("state", state);
            query.Add("login_hint", _loginHint);

            var result = $"{AuthUrl}?{query}";

            return result;
        }

        public string RequestToken(string code)
        {
            _client = new RestClient(new NativeMessageHandler());
            //var cl = new HttpClient(new NativeMessageHandler());

            //var s = cl.GetStringAsync("http://ya.ru").Result;

            //var res = cl.PostAsync(RequestTokenUrl, new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("code", code),
            //new KeyValuePair<string, string>("client_id", _clientId), 
            //new KeyValuePair<string, string>("client_secret", _clientSecret), 
            //new KeyValuePair<string, string>("redirect_uri", _redirectUri), 
            //new KeyValuePair<string, string>("grant_type", "authorization_code")
            //})).Result.Content.ReadAsStringAsync().Result;

            var request = new RestRequest();

            request.AddParameter("code", code);
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("redirect_uri", _redirectUri);
            request.AddParameter("grant_type", "authorization_code");

            var tsk = _client.ExecuteAsync<string>(request);
            var res = tsk.Result;

            return res;
        }
    }
}
