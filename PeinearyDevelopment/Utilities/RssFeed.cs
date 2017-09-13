using DataAccess.Contracts.Blog;
using Markdig;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PeinearyDevelopment.Utilities
{
    public class RssFeed : IRssFeed
    {
        private IPostsDal PostsDal { get; }
        private XmlDocument Document { get; }

        public RssFeed(IPostsDal postsDal)
        {
            PostsDal = postsDal;
            Document = new XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "UTF-8", null));
        }

        public async Task<string> Generate(CancellationToken cancellationToken = default(CancellationToken))
        {
            Document.AppendChild(await GenerateRssElement(cancellationToken).ConfigureAwait(false));
            var memoryStream = new MemoryStream();
            Document.Save(memoryStream);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        private async Task<XmlElement> GenerateRssElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            var rssElement = Document.CreateElement("rss");
            rssElement.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            rssElement.SetAttribute("xmlns:content", "http://purl.org/rss/1.0/modules/content/");
            rssElement.SetAttribute("xmlns:atom", "http://www.w3.org/2005/Atom");
            rssElement.SetAttribute("version", "2.0");
            rssElement.SetAttribute("xmlns:media", "http://search.yahoo.com/mrss/");
            rssElement.AppendChild(await GenerateChannelElement(cancellationToken).ConfigureAwait(false));
            return rssElement;
        }

        private async Task<XmlElement> GenerateChannelElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            var channelElement = Document.CreateElement("channel");
            channelElement.AppendChild(GenerateElementWithCData("title", Constants.BlogTitle));
            channelElement.AppendChild(GenerateElementWithCData("description", Constants.BlogTagLine));
            channelElement.AppendChild(GenerateElementWithInnerText("link", Constants.RootBlogUrl));
            channelElement.AppendChild(GenerateElementWithInnerText("lastBuildDate", DateTime.UtcNow.ToString("R")));
            channelElement.AppendChild(GenerateAtomLinkElement());
            channelElement.AppendChild(GenerateElementWithInnerText("ttl", "60"));

            var posts = await PostsDal.Search(0, 15, cancellationToken).ConfigureAwait(false);
            foreach (var post in posts.Results)
            {
                channelElement.AppendChild(GenerateItemElement(post));
            }

            return channelElement;
        }

        private XmlElement GenerateAtomLinkElement()
        {
            var atomLinkElement = Document.CreateElement("atom:link");
            atomLinkElement.SetAttribute("href", $"{Constants.RootBlogUrl}/rss/");
            atomLinkElement.SetAttribute("rel", "self");
            atomLinkElement.SetAttribute("type", "application/rss+xml");
            return atomLinkElement;
        }

        private XmlElement GenerateItemElement(PostDto post)
        {
            var itemElement = Document.CreateElement("item");
            itemElement.AppendChild(GenerateElementWithCData("title", post.Title));
            itemElement.AppendChild(GenerateElementWithCData("description", post.MarkdownContent.Substring(0, 255)));
            itemElement.AppendChild(GenerateElementWithInnerText("link", $"{Constants.RootBlogUrl}/{post.Slug}"));
            foreach (var tag in post.Tags)
            {
                itemElement.AppendChild(GenerateElementWithCData("category", tag.Tag.Name));
            }

            itemElement.AppendChild(GenerateElementWithInnerText("pubDate", post.PostedOn.Value.ToString("R")));
            itemElement.AppendChild(GenerateElementWithCData("content:encoded", Markdown.ToHtml(post.MarkdownContent)));
            return itemElement;
        }

        private XmlElement GenerateElementWithCData(string elementName, string cDataContent)
        {
            var element = Document.CreateElement(elementName);
            element.AppendChild(Document.CreateCDataSection(cDataContent));
            return element;
        }

        private XmlElement GenerateElementWithInnerText(string elementName, string innerText)
        {
            var lastBuildDateElement = Document.CreateElement(elementName);
            lastBuildDateElement.InnerText = innerText;
            return lastBuildDateElement;
        }
    }
}
