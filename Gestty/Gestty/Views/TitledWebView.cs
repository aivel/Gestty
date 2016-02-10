using System;
using Xamarin.Forms;

namespace Gestty.Renderers
{
    public class TitledWebView : WebView
    {
        public static readonly BindableProperty PageTitleProperty = BindableProperty.Create<TitledWebView, string>(v => v.PageTitle, null, BindingMode.OneWayToSource);

        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set {SetValue(PageTitleProperty, value);}
        }
    }
}
