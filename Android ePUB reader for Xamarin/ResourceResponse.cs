using System.IO;

// A POJO, basically a cut down WebResourceResponse for use on Android devices below 3.0
namespace Com.Dteviot.EpubViewer
{
	public class ResourceResponse {
		private string mMimeType;
		private Stream mData;
		private long mSize;

		public ResourceResponse(string mimeType, Stream data) {
			mMimeType = mimeType;
			mData = data;
		}

		public string getMimeType() { return mMimeType; }
		public Stream getData() { return mData; }
		public long getSize() { return mSize; }
		public void setMimeType(string mimeType) { mMimeType = mimeType; }
		public void setData(Stream data) { mData = data; }
		public void setSize(long size) { mSize = size; }
	}
}
