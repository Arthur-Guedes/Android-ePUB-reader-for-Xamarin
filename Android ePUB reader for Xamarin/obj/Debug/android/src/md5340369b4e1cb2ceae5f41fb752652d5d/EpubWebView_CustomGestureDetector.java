package md5340369b4e1cb2ceae5f41fb752652d5d;


public class EpubWebView_CustomGestureDetector
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFling:(Landroid/view/MotionEvent;Landroid/view/MotionEvent;FF)Z:GetOnFling_Landroid_view_MotionEvent_Landroid_view_MotionEvent_FFHandler\n" +
			"n_onDoubleTap:(Landroid/view/MotionEvent;)Z:GetOnDoubleTap_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.EpubWebView+CustomGestureDetector, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EpubWebView_CustomGestureDetector.class, __md_methods);
	}


	public EpubWebView_CustomGestureDetector () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomGestureDetector.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomGestureDetector, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public EpubWebView_CustomGestureDetector (md5340369b4e1cb2ceae5f41fb752652d5d.EpubWebView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomGestureDetector.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomGestureDetector, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Com.Dteviot.EpubViewer.EpubWebView, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public boolean onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3)
	{
		return n_onFling (p0, p1, p2, p3);
	}

	private native boolean n_onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3);


	public boolean onDoubleTap (android.view.MotionEvent p0)
	{
		return n_onDoubleTap (p0);
	}

	private native boolean n_onDoubleTap (android.view.MotionEvent p0);

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
