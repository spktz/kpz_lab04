using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public interface IEventListener
    {
        void Update(string eventType, LightElementNode target);
    }

    public class LogEventListener : IEventListener
    {
        private string _name;
        public LogEventListener(string name) => _name = name;

        public void Update(string eventType, LightElementNode target)
        {
            Console.WriteLine($"[Слухач {_name}]: Подія '{eventType}' відбулася на тегу <{target.TagName}>");
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

    public class LightElementNode : LightNode
    {
        public string TagName { get; }
        public string DisplayType { get; }
        public bool IsSelfClosing { get; }
        public List<string> CssClasses { get; } = new List<string>();

        private readonly List<LightNode> _children = new List<LightNode>();

        private readonly Dictionary<string, List<IEventListener>> _eventListeners = new Dictionary<string, List<IEventListener>>();

        public LightElementNode(string tagName, string displayType, bool isSelfClosing)
        {
            TagName = tagName;
            DisplayType = displayType;
            IsSelfClosing = isSelfClosing;
        }

        public void AddEventListener(string eventType, IEventListener listener)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<IEventListener>();
            }
            _eventListeners[eventType].Add(listener);
        }

        public void TriggerEvent(string eventType)
        {
            if (_eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in _eventListeners[eventType])
                {
                    listener.Update(eventType, this);
                }
            }
        }

        public void AddChild(LightNode node) => _children.Add(node);
        public int ChildrenCount => _children.Count;

        public override string InnerHtml()
        {
            return string.Join("", _children.Select(child => child.OuterHtml()));
        }

        public override string OuterHtml()
        {
            string classes = CssClasses.Any() ? " class=\"" + string.Join(" ", CssClasses) + "\"" : "";

            if (IsSelfClosing)
            {
                return "<" + TagName + classes + " />";
            }

            return "<" + TagName + classes + ">" + InnerHtml() + "</" + TagName + ">";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var button = new LightElementNode("button", "inline", false);
            button.AddChild(new LightTextNode("click me"));
            button.CssClasses.Add("btn-submit");

            var div = new LightElementNode("div", "block", false);
            div.AddChild(new LightTextNode("Блок з контентом"));

            var logger = new LogEventListener("CLogger");
            var analytics = new LogEventListener("Analytics");

            button.AddEventListener("click", logger);
            button.AddEventListener("click", analytics);

            div.AddEventListener("mouseover", logger);

            Console.WriteLine("Взаємодія");
            Console.WriteLine("Клікаємо на кнопку:");
            button.TriggerEvent("click");

            Console.WriteLine("\nНаводимо мишу на блок:");
            div.TriggerEvent("mouseover");

            Console.WriteLine("\nСпроба викликати подію, на яку ніхто не підписаний:");
            button.TriggerEvent("focus");

            Console.WriteLine("\nГотовий HTML:");
            Console.WriteLine(button.OuterHtml());

            Console.ReadKey();
        }
    }
}
