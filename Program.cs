using System;
using System.Collections.Generic;

namespace Aquarium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Work();
        }
    }

    class Aquarium
    {
        private List<Fish> _fish = new List<Fish>();

        private int _maxFish = 15;

        public void Work()
        {
            const string CommandAddFish = "1";
            const string CommandDeleteFish = "2";
            const string CommandWait = "3";
            const string CommandExit = "exit";

            bool isRunning = true;

            while (isRunning)
            {
                ShowFish();
                Console.WriteLine();
                ShowNumberFreePlace();

                Console.WriteLine($"\nВведите {CommandAddFish}, чтобы добавить рыб.\n" +
                    $"Введите {CommandDeleteFish}, чтобы достать рыбу из аквариума.\n" +
                    $"Введите {CommandWait}, чтобы подождать.\n" +
                    $"Введите {CommandExit}, чтобы выйти.");
                string userInput = Console.ReadLine();

                Console.Clear();

                switch (userInput)
                {
                    case CommandAddFish:
                        TryAddFish();
                        break;

                    case CommandDeleteFish:
                        TryDeleteFish();
                        break;

                    case CommandWait:
                        Console.WriteLine("\nАквариум живёт своей жизнью.\n");
                        break;

                    case CommandExit:
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет.");
                        break;
                }

                FishGetOld();
            }
        }

        private void FishGetOld()
        {
            Console.WriteLine("Прошёл год.");

            foreach (Fish fish in _fish)
                fish.GetOld();
        }

        private int GetNumberFreePlace()
        {
            return _maxFish - _fish.Count;
        }

        private void TryAddFish()
        {
            if (_fish.Count < _maxFish)
                AddFish();
            else
                Console.WriteLine("Аквариум полон.");
        }

        private void AddFish()
        {
            int numberFish = ReadNumberFish();

            for (int i = 0; i < numberFish; i++)
                _fish.Add(new Fish());
        }

        private void TryDeleteFish()
        {
            if (_fish.Count != 0)
                DeleteFish();
            else
                Console.WriteLine("Аквариум пуст.");
        }

        private void DeleteFish()
        {
            bool isRunning;

            do
            {
                ShowFish();

                Console.WriteLine("\nВведите номер рыбы, которую хотите удалить или что-нибудь, чтобы выйти.");
                isRunning = int.TryParse(Console.ReadLine(), out int number);

                if (number >= 0 && number < _fish.Count)
                    _fish.RemoveAt(number);
                else
                    isRunning = false;

                Console.Clear();
            }
            while (isRunning);
        }

        private void ShowFish()
        {
            if (_fish.Count != 0)
            {
                for (int i = 0; i < _fish.Count; i++)
                {
                    Console.Write($"{i} - ");
                    _fish[i].ShowStats();
                }
            }
            else
            {
                Console.WriteLine("В аквариуме рыб нет.");
            }
        }

        private int ReadNumberFish()
        {
            int number;

            do
            {
                ShowNumberFreePlace();

                Console.WriteLine("Введите количество рыб, которое хотите добавить.");
                number = ReadInt();
            }
            while (number <= 0 || number > GetNumberFreePlace());

            return number;
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }

        private void ShowNumberFreePlace()
        {
            Console.WriteLine($"В аквариуме {GetNumberFreePlace()} свободного места.");
        }
    }

    class CreatorFish
    {
        protected string Name;
        protected int Age;
        protected int MaxAge;

        public CreatorFish()
        {
            int minLimitAge = 1;
            int maxLimitAge = 5;

            int minLimitMaxAge = 10;
            int maxLimitMaxAge = 20;

            Name = GetName();
            Age = UserUtils.GenerateRandomNumber(minLimitAge, maxLimitAge);
            MaxAge = UserUtils.GenerateRandomNumber(minLimitMaxAge, maxLimitMaxAge);
            IsAlive = true;
        }

        public bool IsAlive { get; protected set; }

        private string GetName()
        {
            List<string> names = new List<string>() { "Немо", "Жабр", "Персик", "Жак", "Пузырь", "Грот", "Бульк", "Бриз", "Штиль", "Марлин", "Дори", "Якорь" };

            return names[UserUtils.GenerateRandomNumber(0, names.Count - 1)];
        }
    }

    class Fish : CreatorFish
    {
        public void GetOld()
        {
            if (Age >= MaxAge)
                Die();
            else
                Age++;
        }

        private void Die()
        {
            IsAlive = false;
        }

        public void ShowStats()
        {
            if (IsAlive)
                Console.WriteLine($"Рыба по имени {Name} возрастом {Age} плавает в аквариуме.");
            else
                Console.WriteLine($"Рыба по имени {Name} умерла в возрасте {Age} и плавает кверху брюхом.");
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max);
        }
    }
}
