using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task5
{
    public class TextVersionCard
    {
        private readonly string _savedText;
        public DateTime CreatedAt { get; }

        public TextVersionCard(string text)
        {
            _savedText = text;
            CreatedAt = DateTime.Now;
        }

        internal string GetSavedContent() => _savedText;
    }

    public class TextWorkBuffer
    {
        private string _currentText = "";

        public void Write(string newText)
        {
            Console.WriteLine($"Додано текст: {newText}");
            _currentText = newText;
        }

        public void PrintStatus()
        {
            Console.WriteLine($"Поточний стан: \"{_currentText}\"");
        }

        public TextVersionCard CreateVersion()
        {
            return new TextVersionCard(_currentText);
        }

        public void LoadVersion(TextVersionCard card)
        {
            if (card != null)
            {
                _currentText = card.GetSavedContent();
                Console.WriteLine($"Відновлено до версії від {card.CreatedAt}");
            }
        }
    }

    public class RecoveryManager
    {
        private readonly Stack<TextVersionCard> _backups = new Stack<TextVersionCard>();
        private readonly TextWorkBuffer _buffer;

        public RecoveryManager(TextWorkBuffer buffer)
        {
            _buffer = buffer;
        }

        public void MakeBackup()
        {
            Console.WriteLine("Створення бекапу...");
            _backups.Push(_buffer.CreateVersion());
        }

        public void UndoAction()
        {
            if (_backups.Count == 0)
            {
                Console.WriteLine("Немає збережених копій для відкату.");
                return;
            }

            var lastCard = _backups.Pop();
            _buffer.LoadVersion(lastCard);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var myText = new TextWorkBuffer();
            var manager = new RecoveryManager(myText);

            myText.Write("Hello world!");
            manager.MakeBackup();

            myText.Write("Hello world 123456");
            manager.MakeBackup();

            myText.Write("Видалення...");
            myText.PrintStatus();

            Console.WriteLine("\nСкасування останньої дії");
            manager.UndoAction();
            myText.PrintStatus();

            Console.WriteLine("\nСкасування ще однієї дії");
            manager.UndoAction();
            myText.PrintStatus();

        }
    }
}
