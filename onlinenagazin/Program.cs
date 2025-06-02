using System;
using System.Collections.Generic;

namespace onlinemagazine
{
    // Интерфейс «Товар»
    public interface IProduct
    {
        string GetName(); // Получить название товара
        decimal GetPrice(); // Получить цену товара
        string GetDescription(); // Получить описание товара
    }

    // Интерфейс «Корзина»
    public interface ICart
    {
        void AddProduct(IProduct product); // Добавить товар в корзину
        void RemoveProduct(IProduct product); // Удалить товар из корзины
        void Checkout(); // Оформить заказ
        decimal GetTotalPrice(); // Получить общую сумму
    }

    // Интерфейс «Оплата»
    public interface IPayment
    {
        bool ProcessPayment(decimal amount); // Провести платеж
        void HandleTransaction(string transactionId, bool success); // Обработать транзакцию
    }

    // Реализация товара
    public class Product : IProduct
    {
        private string name;
        private decimal price;
        private string description;

        public Product(string name, decimal price, string description)
        {
            this.name = name;
            this.price = price;
            this.description = description;
        }

        public string GetName() => name;

        public decimal GetPrice() => price;

        public string GetDescription() => description;
    }

    // Реализация корзины
    public class ShoppingCart : ICart
    {
        private List<IProduct> products = new List<IProduct>();

        public void AddProduct(IProduct product)
        {
            products.Add(product);
            Console.WriteLine($"{product.GetName()} добавлен в корзину.");
        }

        public void RemoveProduct(IProduct product)
        {
            if (products.Remove(product))
            {
                Console.WriteLine($"{product.GetName()} удален из корзины.");
            }
            else
            {
                Console.WriteLine($"{product.GetName()} не найден в корзине.");
            }
        }

        public void Checkout()
        {
            Console.WriteLine("\nОформление заказа:");
            foreach (var product in products)
            {
                Console.WriteLine($"- {product.GetName()} (Цена: {product.GetPrice()} грн, Описание: {product.GetDescription()})");
            }
            Console.WriteLine($"Общая сумма: {GetTotalPrice()} грн");
            products.Clear();
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;
            foreach (var product in products)
            {
                total += product.GetPrice();
            }
            return total;
        }

        // Добавляем метод для получения списка товаров
        public List<IProduct> GetProducts()
        {
            return products;
        }
    }

    // Реализация оплаты
    public class PaymentSystem : IPayment
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Оплата на сумму {amount} грн...");
            // Представим, что платеж всегда успешен
            return true;
        }

        public void HandleTransaction(string transactionId, bool success)
        {
            if (success)
            {
                Console.WriteLine($"Транзакция {transactionId} успешно завершена.");
            }
            else
            {
                Console.WriteLine($"Транзакция {transactionId} не удалась.");
            }
        }
    }

    // Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            ICart cart = new ShoppingCart();
            IPayment paymentSystem = new PaymentSystem();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Оформить заказ");
                Console.WriteLine("4. Выйти");
                Console.Write("Выберите опцию: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название товара: ");
                        string name = Console.ReadLine();
                        Console.Write("Введите цену товара: ");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                        {
                            Console.WriteLine("Пожалуйста, введите корректную цену.");
                        }
                        Console.Write("Введите описание товара: ");
                        string description = Console.ReadLine();

                        IProduct product = new Product(name, price, description);
                        cart.AddProduct(product);
                        break;

                    case "2":
                        Console.Write("Введите название товара, который нужно удалить: ");
                        string removeName = Console.ReadLine();
                        IProduct productToRemove = null;

                        foreach (var p in (cart as ShoppingCart).GetProducts())
                        {
                            if (p.GetName().Equals(removeName, StringComparison.OrdinalIgnoreCase))
                            {
                                productToRemove = p;
                                break;
                            }
                        }

                        if (productToRemove != null)
                        {
                            cart.RemoveProduct(productToRemove);
                        }
                        else
                        {
                            Console.WriteLine($"Товар '{removeName}' не найден в корзине.");
                        }
                        break;

                    case "3":
                        decimal totalAmount = cart.GetTotalPrice();
                        cart.Checkout();

                        Console.WriteLine("Хотите оплатить заказ? (да/нет)");
                        string payChoice = Console.ReadLine();
                        if (payChoice.Equals("да", StringComparison.OrdinalIgnoreCase))
                        {
                            bool paymentSuccess = paymentSystem.ProcessPayment(totalAmount);
                            paymentSystem.HandleTransaction(Guid.NewGuid().ToString(), paymentSuccess);
                        }
                        break;

                    case "4":
                        Console.WriteLine("Спасибо за использование программы!");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }
    }
}