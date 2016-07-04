using Org.Xml.Sax;

// A row in the epub Manifest
namespace Com.Dteviot.EpubViewer.Epub
{
	public class ManifestItem {
		private static readonly string XML_ATTRIBUTE_ID = "id";
		private static readonly string XML_ATTRIBUTE_HREF = "href";
		private static readonly string XML_ATTRIBUTE_MEDIA_TYPE = "media-type";

		private string mHref;
		private string mID;
		private string mMediaType;

		public string getHref() { return mHref; }
		public string getID() { return mID; }
		public string getMediaType() { return mMediaType; }

		// Construct from XML
		public ManifestItem(IAttributes attributes, HrefResolver resolver) {
			mHref = resolver.ToAbsolute(attributes.GetValue(XML_ATTRIBUTE_HREF));
			mID = attributes.GetValue(XML_ATTRIBUTE_ID);
			mMediaType = attributes.GetValue(XML_ATTRIBUTE_MEDIA_TYPE);
		}
	}
}
