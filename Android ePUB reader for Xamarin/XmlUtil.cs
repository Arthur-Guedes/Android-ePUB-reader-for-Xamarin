using Android.Util;
using Java.Net;
using Javax.Xml.Parsers;
using Org.Xml.Sax;
using Org.Xml.Sax.Helpers;
using System;
using System.IO;
using System.Text;

// Functions for processing XML
namespace Com.Dteviot.EpubViewer
{
	public class XmlUtil {
		private static readonly Base64Flags BASE64_DATA_URI = Base64Flags.NoWrap;

		/*
		* @param uri of the XML document being processed (used to resolve links)
		* @param source to fetch data from
		* @param attrs to update
		* @param attributeName name of attribute to replace value for
		*/
		public static AttributesImpl replaceAttributeValueWithDataUri(Android.Net.Uri uri, IResourceSource source, IAttributes attrs, string attributeName) {

			AttributesImpl newAttrs = new AttributesImpl(attrs);

			// find wanted attribute and update
			for(int i = 0; i < newAttrs.Length; ++i) {
				if (newAttrs.GetLocalName(i).Equals(attributeName)) {
					// if it's already a data URI, nothing to do
					string value = newAttrs.GetValue(i);
					if ((value.Length < 5) || !value.Substring(0, 5).Equals("data:")) {
						Android.Net.Uri content = resolveRelativeUri(uri, value); 
						ResourceResponse response = source.fetch(content);
						if (response != null) {
							newAttrs.SetValue(i, buildDataUri(response));
						}
					}
					break;
				}
			}
			return newAttrs;
		}

		/*
		* Convert a relative URI into an absolute one
		* @param sourceUri of XML document holding the relative URI
		* @param relativeUri to resolve
		* @return absolute URI
		*/
		public static Android.Net.Uri resolveRelativeUri(Android.Net.Uri sourceUri, string relativeUri) {
			URL source = new URL(sourceUri.ToString());
			URL absolute = new URL(source, relativeUri);
			return Android.Net.Uri.Parse(absolute.ToString());
		}

		public static string buildDataUri(ResourceResponse response) {
			StringBuilder sb = new StringBuilder("data:");
			sb.Append(response.getMimeType());
			sb.Append(";base64,");
			streamToBase64(response.getData(), sb);
			return sb.ToString();
		}

		public static void streamToBase64(Stream stream, StringBuilder sb) {
			int buflen = 4096;
			byte[] buffer = new byte[buflen];
			int offset = 0;
			int len = 0;
			while (len != -1) {
				len = stream.Read(buffer, offset, buffer.Length - offset);
				if (len != -1) {
					// must process a multiple of 3 bytes, so that no padding chars are placed
					int total = offset + len;
					offset = total % 3; 
					int bytesToProcess = total - offset;
					if (0 < bytesToProcess) {
						sb.Append(Base64.EncodeToString(buffer, 0, bytesToProcess, BASE64_DATA_URI));
					}
					// shuffle unused bytes to start of array
					Array.Copy(buffer, bytesToProcess, buffer, 0, offset);
				} else if (0 < offset) {
					// flush
					sb.Append(Base64.EncodeToString(buffer, 0, offset, BASE64_DATA_URI));
				}
			}
			stream.Close();
		}

		/*
		* Parse an XML file in the zip.
		*  @fileName name of XML file in the zip
		*  @root parser to read the XML file
		*/
		public static void parseXmlResource(Stream stream, IContentHandler handler, XMLFilterImpl lastFilter) {
			if (stream != null) {
				try {
					SAXParserFactory parseFactory = SAXParserFactory.NewInstance();
					IXMLReader reader = parseFactory.NewSAXParser().XMLReader;
					reader.ContentHandler = handler;

					try {
						InputSource source = new InputSource(stream);
						source.Encoding = "UTF-8";

						if (lastFilter != null) {
							// this is a chain of filters, setup the pipeline
							((XMLFilterImpl)handler).Parent = reader;
							lastFilter.Parse(source);
						} else {
							// simple content handler
							reader.Parse(source);
						}
					} finally {
						stream.Close();
					}
				} catch (ParserConfigurationException e) {
					// TODO Auto-generated catch block
					e.PrintStackTrace();
				} catch (IOException e) {
					Log.Error(Globals.TAG, "Error reading XML file ", e);
				} catch (SAXException e) {
					Log.Error(Globals.TAG, "Error parsing XML file ", e);
				}
			}
		}

		/*
		* @param attrs attributes to look through
		* @param name of attribute to get value of
		* @return value of requested attribute, or null if not found
		*/
		public static string getAttributesValue(IAttributes attrs, string name) {
			for(int i = 0; i < attrs.Length; ++i) {
				if (attrs.GetLocalName(i).Equals(name)) {
					return attrs.GetValue(i);
				}
			}
			return null;
		}
	}
}
