using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    public abstract class SupportHandler
    {
        protected SupportHandler _nextHandler;

        public void SetNext(SupportHandler next)
        {
            _nextHandler = next;
        }
        public abstract bool HandleRequest(int choice);
    }

    public class GeneralSupport : SupportHandler
    {
        public override bool HandleRequest(int choice)
        {
            if (choice == 1)
            {
                Console.WriteLine("\n[1]: Перенаправляю на сторінку з тарифними планами...");
                return true;
            }
            return _nextHandler?.HandleRequest(choice) ?? false;
        }
    }
    public class TechnicalSupport : SupportHandler
    {
        public override bool HandleRequest(int choice)
        {
            if (choice == 2)
            {
                Console.WriteLine("\n[2]: Спробуйте перезавантажити пристрій.");
                return true;
            }
            return _nextHandler?.HandleRequest(choice) ?? false;
        }
    }

    public class BillingSupport : SupportHandler
    {
        public override bool HandleRequest(int choice)
        {
            if (choice == 3)
            {
                Console.WriteLine("\n[3]: На вашому рахунку 83.59 грн.");
                return true;
            }
            return _nextHandler?.HandleRequest(choice) ?? false;
        }
    }
    public class LiveSupport : SupportHandler
    {
        public override bool HandleRequest(int choice)
        {
            if (choice == 4)
            {
                Console.WriteLine("\n[4]: З'єдную з персональним менеджером. Будь ласка, зачекайте...");
                return true;
            }
            return _nextHandler?.HandleRequest(choice) ?? false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var l1 = new GeneralSupport();
            var l2 = new TechnicalSupport();
            var l3 = new BillingSupport();
            var l4 = new LiveSupport();

            l1.SetNext(l2);
            l2.SetNext(l3);
            l3.SetNext(l4);

            bool isHandled = false;

            while (!isHandled)
            {
                Console.WriteLine("\nВітаємо у службі підтримки");
                Console.WriteLine("1. Тарифи та послуги");
                Console.WriteLine("2. Технічні несправності");
                Console.WriteLine("3. Стан рахунку");
                Console.WriteLine("4. Зв'язок з оператором");
                Console.Write("\nОберіть пункт меню: ");

                if (int.TryParse(Console.ReadLine(), out int userChoice))
                {
                    isHandled = l1.HandleRequest(userChoice);

                    if (!isHandled)
                    {
                        Console.WriteLine("\nТакого пункту не існує. Спробуйте ще раз.");
                    }
                }
                else
                {
                    Console.WriteLine("\nБудь ласка, введіть число.");
                }
            }

            Console.WriteLine("\nДякуємо, що скористалися нашою системою підтримки!");
        }
    }
}
