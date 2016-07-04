package md5340369b4e1cb2ceae5f41fb752652d5d;


public class TextToSpeechWrapper_CustomTextToSpeechListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.speech.tts.TextToSpeech.OnInitListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onInit:(I)V:GetOnInit_IHandler:Android.Speech.Tts.TextToSpeech/IOnInitListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.TextToSpeechWrapper+CustomTextToSpeechListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TextToSpeechWrapper_CustomTextToSpeechListener.class, __md_methods);
	}


	public TextToSpeechWrapper_CustomTextToSpeechListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TextToSpeechWrapper_CustomTextToSpeechListener.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.TextToSpeechWrapper+CustomTextToSpeechListener, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onInit (int p0)
	{
		n_onInit (p0);
	}

	private native void n_onInit (int p0);

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
