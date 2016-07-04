package md506ac3999666171c60ccea312e0fad152;


public class NavPoint
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.os.Parcelable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_InititalizeCreator:()Lmd506ac3999666171c60ccea312e0fad152/NavPoint_MyParcelableCreator;:__export__\n" +
			"n_describeContents:()I:GetDescribeContentsHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.Epub.NavPoint, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NavPoint.class, __md_methods);
	}


	public NavPoint () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NavPoint.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.Epub.NavPoint, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public NavPoint (java.lang.String p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == NavPoint.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.Epub.NavPoint, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}

	public NavPoint (android.os.Parcel p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == NavPoint.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.Epub.NavPoint, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Parcel, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	private static md506ac3999666171c60ccea312e0fad152.NavPoint_MyParcelableCreator CREATOR = InititalizeCreator ();

	private static md506ac3999666171c60ccea312e0fad152.NavPoint_MyParcelableCreator InititalizeCreator ()
	{
		return n_InititalizeCreator ();
	}

	private static native md506ac3999666171c60ccea312e0fad152.NavPoint_MyParcelableCreator n_InititalizeCreator ();


	public int describeContents ()
	{
		return n_describeContents ();
	}

	private native int n_describeContents ();


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

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
