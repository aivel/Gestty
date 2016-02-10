using System.Threading.Tasks;
using Gestty.Renderers;
using Gestty.Utils;
using GoogleApi;
using Xamarin.Forms;

namespace Gestty
{
    public class App : Application
    {
        private const string SuccessCodePrefix = "Success code=";
        private const string FailCodePrefix = "Denied error=";
        private const string ClientId = "720428859311-vfc0maqhcrfq19htgi6sm9m5a79qch6q.apps.googleusercontent.com";
        private const string ClientSecret = "seElXR24k7laFWsPlWU6LqVD";
        private readonly string _pageTitlePropertyName;
        private readonly GoogleAuth _googleAuth = new GoogleAuth(ClientId, ClientSecret, scopes: Scopes.Make(new[] { Scopes.Calendar }));

        private readonly Label okLabel;

        public App() {
            var url = _googleAuth.BuildRequestUrl();
            
            var webView = new TitledWebView
            {
                Source = url,
                HeightRequest = 300,
                WidthRequest = 300
            };

            _pageTitlePropertyName = webView.GetPropertyName(wv => wv.PageTitle); // save property name

            okLabel = new Label();
            
            webView.PropertyChanged += (sender, args) =>
            {
                if (!webView.IsVisible) return; // properties might not be set yet if so
           
                if (args.PropertyName != _pageTitlePropertyName) return; // only when the exact property is changed

                var pageTitle = webView.PageTitle;

                if (okLabel != null && okLabel.IsVisible && pageTitle.StartsWith(SuccessCodePrefix))
                {
                    var successCode = pageTitle.Substring(SuccessCodePrefix.Length);

                    okLabel.Text = _googleAuth.RequestToken(successCode);
                    //OnSuccessAuth(successCode);
                }

                if (pageTitle.StartsWith(FailCodePrefix))
                {
                    var failCode = pageTitle.Substring(FailCodePrefix.Length);

                    OnFailAuth(failCode);
                }
            };

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        webView,
                        okLabel
                    }
                }
            };
        }

        public void OnSuccessAuth(string successCode)
        {
            okLabel.Text = _googleAuth.RequestToken(successCode);
        }

        protected void OnFailAuth(string failCode)
        {
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
