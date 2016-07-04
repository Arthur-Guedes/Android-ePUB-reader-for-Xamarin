using Java.IO;
using Java.Net;
using Org.Apache.Http;
using Org.Apache.Http.Impl;
using Org.Apache.Http.Params;
using Org.Apache.Http.Protocol;

// A minimal HTTP request processor
namespace Com.Dteviot.EpubViewer.WebServer
{
	public class WebServer {
		private static readonly string MATCH_EVERYTING_PATTERN = "*";

		private BasicHttpContext mHttpContext = null;
		private HttpService mHttpService = null;

		/*
		* @handler that processes get requests
		*/
		public WebServer(IHttpRequestHandler handler){
			mHttpContext = new BasicHttpContext();

			// set up Interceptors.
			//... ResponseContent is required, or it doesn't work.
			//... Others are recommended (in Apache docs) but not
			//... strictly needed in this case.
			BasicHttpProcessor httpproc = new BasicHttpProcessor();
			httpproc.AddInterceptor(new ResponseContent());
			httpproc.AddInterceptor(new ResponseConnControl());
			httpproc.AddInterceptor(new ResponseDate());
			httpproc.AddInterceptor(new ResponseServer());

			mHttpService = new HttpService(httpproc, new DefaultConnectionReuseStrategy(), new DefaultHttpResponseFactory());

			HttpRequestHandlerRegistry registry = new HttpRequestHandlerRegistry();
			registry.Register(MATCH_EVERYTING_PATTERN, handler);
			mHttpService.SetHandlerResolver(registry);
		}

		/*
		* Called when a client connects to server
		* @socket the client is using
		*/
		public void processClientRequest(Socket socket) {
			try {
				DefaultHttpServerConnection serverConnection = new DefaultHttpServerConnection();
				serverConnection.Bind(socket, new BasicHttpParams());
				mHttpService.HandleRequest(serverConnection, mHttpContext);
				serverConnection.Shutdown();
			} catch (IOException e) {
				e.PrintStackTrace();
			} catch (HttpException e) {
				e.PrintStackTrace();
			}
		}
	}
}
