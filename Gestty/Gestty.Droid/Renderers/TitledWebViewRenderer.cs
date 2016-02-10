using Android.Webkit;
using Gestty.Droid.Renderers;
using Gestty.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(TitledWebView), typeof(TitledWebViewRenderer))]
namespace Gestty.Droid.Renderers
{
    public class TitledWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.SetWebViewClient(new TitledWebViewClient(this));
            }
        }

        internal class TitledWebViewClient : WebViewClient
        {
            readonly TitledWebViewRenderer titleWebViewRenderer;

            internal TitledWebViewClient(TitledWebViewRenderer titleWebViewRenderer)
            {
                this.titleWebViewRenderer = titleWebViewRenderer;
            }

            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
                ((IElementController)titleWebViewRenderer.Element).SetValueFromRenderer(TitledWebView.PageTitleProperty, view.Title);
            }
        }
    }
}