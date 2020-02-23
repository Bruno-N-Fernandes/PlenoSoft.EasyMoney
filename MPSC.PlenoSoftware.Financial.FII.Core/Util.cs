using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace MPSC.PlenoSoftware.Financial.FII.Core
{
	public static class Util
	{
		private static readonly CultureInfo en_US = new CultureInfo("en-US");

		public static string ToPercentual(decimal? value)
		{
			return value.HasValue ? (value * 100M).ToString() + "%" : "";
		}

		public static decimal? GetPercentual(XmlNode[] xmlNodes, int index)
		{
			return GetDecimal(xmlNodes, index) / 100M;
		}

		public static decimal? GetDecimal(XmlNode[] xmlNodes, int index)
		{
			var value = GetString(xmlNodes, index, "data-order");
			if (decimal.TryParse(value, NumberStyles.Any, en_US, out var result))
				return (result <= -999999999M) ? default(decimal?) : result;

			return default;
		}

		public static int? GetInt32(XmlNode[] xmlNodes, int index)
		{
			var value = GetString(xmlNodes, index, "data-order");
			return int.TryParse(value, out var result) ? result : default(int?);
		}

		public static string GetString(XmlNode[] xmlNodes, int index, string attribute)
		{
			var xmlNode = xmlNodes[index];
			return xmlNode?.Attributes?[attribute]?.Value ?? xmlNode.InnerText;
		}

		public static string GetHtml(string urlAddress)
		{
			var result = "";
			var request = (HttpWebRequest)WebRequest.Create(urlAddress);
			using var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				using var receiveStream = response.GetResponseStream();
				using var readStream = string.IsNullOrWhiteSpace(response.CharacterSet)
					? new StreamReader(receiveStream)
					: new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

				result = readStream.ReadToEnd();

				response.Close();
				readStream.Close();
			}
			return result;
		}

		public static XmlNode[] GetPrimaryFields(string tableRow)
		{
			var xml = new XmlDocument();
			xml.LoadXml($"<tr>\n{tableRow}\n</tr>");
			return xml.DocumentElement.ChildNodes.OfType<XmlNode>().ToArray();
		}
	}
}