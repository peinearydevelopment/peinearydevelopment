using DataAccess.Contracts.Blog;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PeinearyDevelopment.Utilities
{
    public class Sitemap : ISitemap
    {
        private IPostsDal PostsDal { get; }
        private XmlDocument Document { get; }

        public Sitemap(IPostsDal postsDal)
        {
            PostsDal = postsDal;
            Document = new XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "UTF-8", null));
        }

        public async Task<string> Generate(CancellationToken cancellationToken = default(CancellationToken))
        {
            Document.AppendChild(await GenerateUrlsetElement(cancellationToken).ConfigureAwait(false));
            var memoryStream = new MemoryStream();
            Document.Save(memoryStream);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        private async Task<XmlElement> GenerateUrlsetElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            var urlsetElement = Document.CreateElement("urlset");
            urlsetElement.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var post in await PostsDal.ReadMany(p => true, cancellationToken).ConfigureAwait(false))
            {
                var urlElement = Document.CreateElement("url");
                urlElement.AppendChild(GenerateElementWithInnerText("loc", $"{Constants.RootBlogUrl}/{post.Slug}"));
                if (post.LastUpdatedOn.HasValue)
                {
                    urlElement.AppendChild(GenerateElementWithInnerText("lastmod", post.LastUpdatedOn.Value.ToString("yyyy-MM-dd")));
                }

                urlElement.AppendChild(GenerateElementWithInnerText("changefreq", "yearly"));
                urlsetElement.AppendChild(urlElement);
            }

            return urlsetElement;
        }

        private XmlElement GenerateElementWithInnerText(string elementName, string innerText)
        {
            var lastBuildDateElement = Document.CreateElement(elementName);
            lastBuildDateElement.InnerText = innerText;
            return lastBuildDateElement;
        }
    }
}
