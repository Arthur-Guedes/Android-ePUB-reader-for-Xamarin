using Android.Net;
using Android.Sax;
using Android.Util;
using Java.Util.Zip;
using Org.Xml.Sax;
using System.Collections.Generic;
using System.IO;

// Represents a book that's been packed into an epub file
namespace Com.Dteviot.EpubViewer.Epub
{
	public class Book : IResourceSource {
		private readonly static string HTTP_SCHEME = "http";

		// the container XML
		private static readonly string XML_NAMESPACE_CONTAINER = "urn:oasis:names:tc:opendocument:xmlns:container";
		private static readonly string XML_ELEMENT_CONTAINER = "container";
		private static readonly string XML_ELEMENT_ROOTFILES = "rootfiles";
		private static readonly string XML_ELEMENT_ROOTFILE = "rootfile";
		private static readonly string XML_ATTRIBUTE_FULLPATH = "full-path";
		private static readonly string XML_ATTRIBUTE_MEDIATYPE = "media-type";

		// the .opf XML
		private static readonly string XML_NAMESPACE_PACKAGE = "http://www.idpf.org/2007/opf";
		private static readonly string XML_ELEMENT_PACKAGE = "package";
		private static readonly string XML_ELEMENT_MANIFEST = "manifest";
		private static readonly string XML_ELEMENT_MANIFESTITEM = "item";
		private static readonly string XML_ELEMENT_SPINE = "spine";
		private static readonly string XML_ATTRIBUTE_TOC = "toc"; 
		private static readonly string XML_ELEMENT_ITEMREF = "itemref";
		private static readonly string XML_ATTRIBUTE_IDREF = "idref";

		// The zip archive
		private ZipFile mZip;

		// Name of the ".opf" file in the zip archive
		private string mOpfFileName;

		// Id of the "table of contents" entry in manifest
		private string mTocID;

		// Allow access to state for unit tests.
		public string getOpfFileName() { return mOpfFileName; }
		public string getTocID() { return mTocID; }
		public List<ManifestItem> getSpine() { return mSpine; }
		public Manifest getManifest() { return mManifest; }
		public TableOfContents getTableOfContents() { return mTableOfContents; }

		// The resources that are in the spine element of the metadata
		private List<ManifestItem> mSpine;

		// The manifest entry in the metadata
		private Manifest mManifest;

		// The Table of Contents in the metadata
		private TableOfContents mTableOfContents;

		// Intended for unit testing
		public Book() {
			mSpine = new List<ManifestItem>();
			mManifest = new Manifest();
			mTableOfContents = new TableOfContents();
		}

		/*
		* Constructor
		* @param fileName the filename of the Zip archive file
		*/
		public Book(string fileName) {
			mSpine = new List<ManifestItem>();
			mManifest = new Manifest();
			mTableOfContents = new TableOfContents();
			try {
				mZip = new ZipFile(fileName);
				parseEpub();
			} catch (IOException e) {
				Log.Error(Globals.TAG, "Error opening file", e);
			}
		}

		// Name of zip file
		public string getFileName() {
			return (mZip == null) ? null : mZip.Name;
		}

		// Fetch file from zip
		private Stream fetchFromZip(string fileName) {
			Stream stream = null;
			ZipEntry containerEntry = mZip.GetEntry(fileName);
			if (containerEntry != null) {
				try {
					stream = mZip.GetInputStream(containerEntry);
				} catch (IOException e) {
					Log.Error(Globals.TAG, "Error reading zip file " + fileName, e);
				}
			}

			if (stream == null) {
				Log.Error(Globals.TAG, "Unable to find file in zip: " + fileName);
			}

			return stream;
		}

		// Fetch resource from ebook
		public ResourceResponse fetch(Uri resourceUri) {
			string resourceName = url2ResourceName(resourceUri);
			ManifestItem item = mManifest.findByResourceName(resourceName);
			if (item != null) {
				ResourceResponse response = new ResourceResponse(item.getMediaType(), fetchFromZip(resourceName));
				response.setSize(mZip.GetEntry(resourceName).Size);
				return response;
			}

			// if get here, something went wrong
			Log.Error(Globals.TAG, "Unable to find resource in ebook " + resourceName);
			return null;
		}

		public Uri firstChapter() {
			return 0 < mSpine.Count ? resourceName2Url(mSpine[0].getHref()) : null;
		}

		/*
		* @return URI of next resource in sequence, or null if not one
		*/
		public Uri nextResource(Uri resourceUri) {
			string resourceName = url2ResourceName(resourceUri);
			for (int i = 0; i < mSpine.Count - 1; ++i) {
				if (mSpine[i].getHref().Equals(resourceName)) {
					return resourceName2Url(mSpine[i + 1].getHref());
				}
			}
			// if get here, not found
			return null;
		}

		/*
		* @return URI of previous resource in sequence, or null if not one
		*/
		public Uri previousResource(Uri resourceUri) {
			string resourceName = url2ResourceName(resourceUri);
			for (int i = 1; i < mSpine.Count; ++i) {
				if (mSpine[i].getHref().Equals(resourceName)) {
					return resourceName2Url(mSpine[i - 1].getHref());
				}
			}
			// if get here not found
			return null;
		}

		// Build up structure of epub
		private void parseEpub() {
			// clear everything
			mOpfFileName = null;
			mTocID = null;
			mSpine.Clear();
			mManifest.clear();
			mTableOfContents.clear();

			// get the "container" file, this tells us where the ".opf" file is
			parseXmlResource("META-INF/container.xml", constructContainerFileParser());

			if (mOpfFileName != null) {
				parseXmlResource(mOpfFileName, constructOpfFileParser());
			}

			if (mTocID != null) {
				ManifestItem tocManifestItem = mManifest.findById(mTocID);
				if (tocManifestItem != null) {
					string tocFileName = tocManifestItem.getHref();
					HrefResolver resolver = new HrefResolver(tocFileName);
					parseXmlResource(tocFileName, mTableOfContents.constructTocFileParser(resolver));
				}
			}
		}

		private void parseXmlResource(string fileName, IContentHandler handler) {
			Stream stream = fetchFromZip(fileName);
			if (stream != null) {
				XmlUtil.parseXmlResource(stream, handler, null);
			}
		}

		/*
		* build parser to parse the container file,
		* i.e. get the name of the ".opf" file in the zip.
		* @return parser
		*/
		public IContentHandler constructContainerFileParser() {
			// describe the relationship of the elements
			RootElement root = new RootElement(XML_NAMESPACE_CONTAINER, XML_ELEMENT_CONTAINER);
			Element rootfiles = root.GetChild(XML_NAMESPACE_CONTAINER, XML_ELEMENT_ROOTFILES);
			Element rootfile = rootfiles.GetChild(XML_NAMESPACE_CONTAINER, XML_ELEMENT_ROOTFILE);

			rootfile.StartElement += (object sender, StartElementEventArgs e) => {
				string mediaType = e.Attributes.GetValue(XML_ATTRIBUTE_MEDIATYPE);
				if ((mediaType != null) && mediaType.Equals("application/oebps-package+xml")) {
					mOpfFileName = e.Attributes.GetValue(XML_ATTRIBUTE_FULLPATH);
				}
			};
			return root.ContentHandler;
		}

		/*
		* build parser to parse the ".opf" file,
		* @return parser
		*/
		public IContentHandler constructOpfFileParser() {
			// describe the relationship of the elements
			RootElement root = new RootElement(XML_NAMESPACE_PACKAGE, XML_ELEMENT_PACKAGE);
			Element manifest = root.GetChild(XML_NAMESPACE_PACKAGE, XML_ELEMENT_MANIFEST);
			Element manifestItem = manifest.GetChild(XML_NAMESPACE_PACKAGE, XML_ELEMENT_MANIFESTITEM);
			Element spine = root.GetChild(XML_NAMESPACE_PACKAGE, XML_ELEMENT_SPINE);
			Element itemref = spine.GetChild(XML_NAMESPACE_PACKAGE, XML_ELEMENT_ITEMREF);

			HrefResolver resolver = new HrefResolver(mOpfFileName);
			manifestItem.StartElement += (object sender, StartElementEventArgs e) => {
				mManifest.add(new ManifestItem(e.Attributes, resolver));
			};

			// get name of Table of Contents file from the Spine
			spine.StartElement += (object sender, StartElementEventArgs e) => {
				mTocID = e.Attributes.GetValue(XML_ATTRIBUTE_TOC);
			};

			itemref.StartElement += (object sender, StartElementEventArgs e) => {
				string temp = e.Attributes.GetValue(XML_ATTRIBUTE_IDREF);
				if (temp != null) {
					ManifestItem item = mManifest.findById(temp);
					if (item != null) {
						mSpine.Add(item);
					}
				}
			};
			return root.ContentHandler;	
		}

		/*
		* @param url used by WebView
		* @return resourceName used by zip file
		*/
		private static string url2ResourceName(Uri url) {
			// we only care about the path part of the URL
			string resourceName = url.Path;

			// if path has a '/' prepended, strip it
			if (resourceName[0] == '/') {
				resourceName = resourceName.Substring(1);
			}
			return resourceName;
		}

		/*
		* @param resourceName used by zip file
		* @return URL used by WebView 
		*/
		public static Uri resourceName2Url(string resourceName) {
			// build path assuming local file.
			// pack resourceName into path section of a file URI
			// need to leave '/' chars in path, so WebView is aware
			// of path to current resource, so it can can correctly resolve
			// path of any relative URLs in the current resource.
			return new Uri.Builder().Scheme(HTTP_SCHEME)
			.EncodedAuthority("localhost:" + Globals.WEB_SERVER_PORT)
			.AppendEncodedPath(Uri.Encode(resourceName, "/"))
			.Build();
		}
	}
}
