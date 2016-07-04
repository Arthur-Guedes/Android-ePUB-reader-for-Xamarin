using Android.Net;
using Org.Apache.Http;
using Org.Apache.Http.Entity;
using Org.Apache.Http.Protocol;

namespace Com.Dteviot.EpubViewer.WebServer
{
	public class FileRequestHandler : Java.Lang.Object , IHttpRequestHandler {

		private IResourceSource mResourceSource = null;

		public FileRequestHandler(IResourceSource resourceSource){
			mResourceSource = resourceSource;
		}

		public void handleRequest(IHttpRequest request, IHttpResponse response, IHttpContext context) {
			string uriString = request.RequestLine.Uri;
			ResourceResponse resource = mResourceSource.fetch(Uri.Parse(uriString));
			if ((resource != null) && (resource.getData() != null)) {
				InputStreamEntity entity = new InputStreamEntity(resource.getData(), resource.getSize());
				entity.SetContentType(resource.getMimeType());
				response.Entity = entity;
			} else {
				response.SetStatusLine(request.ProtocolVersion, HttpStatus.ScNotFound, "File Not Found");
			}
		}
	}
}
