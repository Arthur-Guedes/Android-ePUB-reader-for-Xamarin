using Android.Net;

namespace Com.Dteviot.EpubViewer
{
	public interface IResourceSource {
		// Fetch the requested resource
		ResourceResponse fetch(Uri resourceUri);
	}
}
