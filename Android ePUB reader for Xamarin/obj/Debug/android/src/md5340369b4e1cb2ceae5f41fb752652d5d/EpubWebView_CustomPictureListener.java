package md5340369b4e1cb2ceae5f41fb752652d5d;


public class EpubWebView_CustomPictureListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.webkit.WebView.PictureListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onNewPicture:(Landroid/webkit/WebView;Landroid/graphics/Picture;)V:GetOnNewPicture_Landroid_webkit_WebView_Landroid_graphics_Picture_Handler:Android.Webkit.WebView/IPictureListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.EpubWebView+CustomPictureListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EpubWebView_CustomPictureListener.class, __md_methods);
	}


	public EpubWebView_CustomPictureListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomPictureListener.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomPictureListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public EpubWebView_CustomPictureListener (md5340369b4e1cb2ceae5f41fb752652d5d.EpubWebView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomPictureListener.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomPictureListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Com.Dteviot.EpubViewer.EpubWebView, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onNewPicture (android.webkit.WebView p0, android.graphics.Picture p1)
	{
		n_onNewPicture (p0, p1);
	}

	private native void n_onNewPicture (android.webkit.WebView p0, android.graphics.Picture p1);

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
