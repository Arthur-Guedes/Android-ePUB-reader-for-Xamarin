package md5340369b4e1cb2ceae5f41fb752652d5d;


public class EpubWebView_CustomUtteranceProgressListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.speech.tts.TextToSpeech.OnUtteranceCompletedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onUtteranceCompleted:(Ljava/lang/String;)V:GetOnUtteranceCompleted_Ljava_lang_String_Handler:Android.Speech.Tts.TextToSpeech/IOnUtteranceCompletedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.EpubWebView+CustomUtteranceProgressListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EpubWebView_CustomUtteranceProgressListener.class, __md_methods);
	}


	public EpubWebView_CustomUtteranceProgressListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomUtteranceProgressListener.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomUtteranceProgressListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public EpubWebView_CustomUtteranceProgressListener (md5340369b4e1cb2ceae5f41fb752652d5d.EpubWebView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == EpubWebView_CustomUtteranceProgressListener.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.EpubWebView+CustomUtteranceProgressListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Com.Dteviot.EpubViewer.EpubWebView, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onUtteranceCompleted (java.lang.String p0)
	{
		n_onUtteranceCompleted (p0);
	}

	private native void n_onUtteranceCompleted (java.lang.String p0);

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
