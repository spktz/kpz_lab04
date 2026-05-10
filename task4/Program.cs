using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task4
{
    public interface IImageLoadingStrategy
    {
        void Load(string href);
    }

    public class NetworkImageLoadingStrategy : IImageLoadingStrategy
    {
        public void Load(string href)
        {
            Console.WriteLine($"Downloading image from URL: {href}...");
        }
    }

    public class FileSystemImageLoadingStrategy : IImageLoadingStrategy
    {
        public void Load(string href)
        {
            Console.WriteLine($"Reading image from path: {href}...");
        }
    }

    public abstract class LightNode
    {
        public abstract string OuterHtml();
        public abstract string InnerHtml();
    }

    public class LightTextNode : LightNode
    {
        private readonly string _text;
        public LightTextNode(string text) => _text = text;
        public override string OuterHtml() => _text;
        public override string InnerHtml() => _text;
    }

    public class LightImageNode : LightNode
    {
        public string Href { get; }
        private IImageLoadingStrategy _loader;

        public LightImageNode(string href, IImageLoadingStrategy loader)
        {
            Href = href;
            _loader = loader;
            _loader.Load(Href);
        }

        public override string InnerHtml() => "";

        public override string OuterHtml()
        {
            return "<img src=\"" + Href + "\" />";
        }
    }

    public class LightElementNode : LightNode
    {
        public string TagName { get; }
        public List<string> CssClasses { get; } = new List<string>();
        private readonly List<LightNode> _children = new List<LightNode>();

        public LightElementNode(string tagName) => TagName = tagName;

        public void AddChild(LightNode node) => _children.Add(node);

        public override string InnerHtml() => string.Join("", _children.Select(c => c.OuterHtml()));

        public override string OuterHtml()
        {
            string classes = CssClasses.Any() ? " class=\"" + string.Join(" ", CssClasses) + "\"" : "";
            return "<" + TagName + classes + ">" + InnerHtml() + "</" + TagName + ">";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string webUrl = "https://i.natgeofe.com/n/548467d8-c5f1-4551-9f58-6817a8d2c45e/NationalGeographic_2572187_16x9.jpg?w=1200";
            var webImage = new LightImageNode(webUrl, new NetworkImageLoadingStrategy());
            Console.WriteLine("HTML: " + webImage.OuterHtml());
            Console.WriteLine();

            string localPath = "C:/Images/photo.jpg";
            var localImage = new LightImageNode(localPath, new FileSystemImageLoadingStrategy());
            Console.WriteLine("HTML: " + localImage.OuterHtml());

            var div = new LightElementNode("div");
            div.CssClasses.Add("photo");
            div.AddChild(webImage);
            div.AddChild(localImage);

            Console.WriteLine("\nПовний HTML ");
            Console.WriteLine(div.OuterHtml());
        }
    }
}
