package md5340369b4e1cb2ceae5f41fb752652d5d;


public class EpubWebView_CustomWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldInterceptRequest:(Landroid/webkit/WebView;Ljava/lang/String;)Landroid/webkit/WebResourceResponse;:GetShouldInterceptRequest_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.EpubWebView+CustomWebViewClient, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EpubWebView_CustomWebViewClient.class, __md_methods);
	}


	public EpubWebView_CustomWebViewClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomWebViewClient.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomWebViewClient, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public EpubWebView_CustomWebViewClient (md5340369b4e1cb2ceae5f41fb752652d5d.EpubWebView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomWebViewClient.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomWebViewClient, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Com.Dteviot.EpubViewer.EpubWebView, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public android.webkit.WebResourceResponse shouldInterceptRequest (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldInterceptRequest (p0, p1);
	}

	private native android.webkit.WebResourceResponse n_shouldInterceptRequest (android.webkit.WebView p0, java.lang.String p1);


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
