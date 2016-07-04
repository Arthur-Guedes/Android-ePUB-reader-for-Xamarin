package md5340369b4e1cb2ceae5f41fb752652d5d;


public class XhtmlToText
	extends org.xml.sax.helpers.DefaultHandler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_characters:([CII)V:GetCharacters_arrayCIIHandler\n" +
			"n_endElement:(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V:GetEndElement_Ljava_lang_String_Ljava_lang_String_Ljava_lang_String_Handler\n" +
			"n_endDocument:()V:GetEndDocumentHandler\n" +
			"n_startElement:(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Lorg/xml/sax/Attributes;)V:GetStartElement_Ljava_lang_String_Ljava_lang_String_Ljava_lang_String_Lorg_xml_sax_Attributes_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.XhtmlToText, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", XhtmlToText.class, __md_methods);
	}


	public XhtmlToText () throws java.lang.Throwable
	{
		super ();
		if (getClass () == XhtmlToText.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.XhtmlToText, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void characters (char[] p0, int p1, int p2)
	{
		n_characters (p0, p1, p2);
	}

	private native void n_characters (char[] p0, int p1, int p2);


	public void endElement (java.lang.String p0, java.lang.String p1, java.lang.String p2)
	{
		n_endElement (p0, p1, p2);
	}

	private native void n_endElement (java.lang.String p0, java.lang.String p1, java.lang.String p2);


	public void endDocument ()
	{
		n_endDocument ();
	}

	private native void n_endDocument ();


	public void startElement (java.lang.String p0, java.lang.String p1, java.lang.String p2, org.xml.sax.Attributes p3)
	{
		n_startElement (p0, p1, p2, p3);
	}

	private native void n_startElement (java.lang.String p0, java.lang.String p1, java.lang.String p2, org.xml.sax.Attributes p3);

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
