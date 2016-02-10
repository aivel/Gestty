using Gestty.iOS.Renderers;
using Gestty.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TitledWebView), typeof(TitledWebViewRenderer))]
namespace Gestty.iOS.Renderers
{
    public class TitledWebViewRenderer : WebViewRenderer
    {
        public TitledWebViewRenderer()
        {
            LoadFinished += (sender, e) => {
                ((IElementController)Element).SetValueFromRenderer(TitledWebView.PageTitleProperty,
                    EvaluateJavascript("document.title"));
            };
        }
    }
}
