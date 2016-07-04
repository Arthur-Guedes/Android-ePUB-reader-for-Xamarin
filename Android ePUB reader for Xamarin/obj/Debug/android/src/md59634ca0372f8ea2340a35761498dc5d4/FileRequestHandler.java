package md59634ca0372f8ea2340a35761498dc5d4;


public class FileRequestHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.apache.http.protocol.HttpRequestHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_handle:(Lorg/apache/http/HttpRequest;Lorg/apache/http/HttpResponse;Lorg/apache/http/protocol/HttpContext;)V:GethandleRequest_Lorg_apache_http_HttpRequest_Lorg_apache_http_HttpResponse_Lorg_apache_http_protocol_HttpContext_Handler:Org.Apache.Http.Protocol.IHttpRequestHandlerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Dteviot.EpubViewer.WebServer.FileRequestHandler, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FileRequestHandler.class, __md_methods);
	}


	public FileRequestHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FileRequestHandler.class)
			mono.android.TypeManager.Activate ("Com.Dteviot.EpubViewer.WebServer.FileRequestHandler, Android ePUB reader for Xamarin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void handle (org.apache.http.HttpRequest p0, org.apache.http.HttpResponse p1, org.apache.http.protocol.HttpContext p2)
	{
		n_handle (p0, p1, p2);
	}

	private native void n_handle (org.apache.http.HttpRequest p0, org.apache.http.HttpResponse p1, org.apache.http.protocol.HttpContext p2);

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
