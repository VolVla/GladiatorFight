using System;
using System.Collections.Generic;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main()
        {
            ConsoleKey exitButton = ConsoleKey.Enter;
            bool isWork = true;

            while (isWork)
            {
                BattleArena battleArena = new BattleArena();

                if (battleArena.TryCreativeBattle())
                {
                    Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                    Console.ReadKey();
                    Console.Clear();
                    battleArena.Battle();
                    battleArena.AnnounceWinner();

                }

                Console.WriteLine($"\nВы хотите выйти из программы?Нажмите {exitButton}.\nДля продолжение работы нажмите любую другую клавишу");

                if (Console.ReadKey().Key == exitButton)
                {
                    Console.WriteLine("Вы вышли из программы");
                    isWork = false;
                }

                Console.Clear();
            }
        }
    }

    class BattleArena
    {
        private Fighter _firstFighter;
        private Fighter _secondFighter;
        private List<Fighter> _fighters = new List<Fighter>();

        public bool TryCreativeBattle()
        {
            Console.WriteLine("Боец 1");
            ChooseFighter(out _firstFighter);
            Console.WriteLine("Боец 2");
            ChooseFighter(out _secondFighter);

            if (_firstFighter == null || _secondFighter == null)
            {
                Console.WriteLine("Ошибка выбора бойца");
                return false;
            }
            else
            {
                Console.WriteLine("Бойцы выбраны");
                return true;
            }
        }

        public BattleArena()
        {
            _fighters.Add(new Archer("Лучник", 100, 100, 40));
            _fighters.Add(new PlagueDoctor("Чумной Доктор", 100, 100, 15));
            _fighters.Add(new Paladin("Паладин", 100, 100, 50));
            _fighters.Add(new Barbarian("Варвар", 90, 100, 30));
            _fighters.Add(new Mystic("Мистик", 100, 100, 22));
        }

        public void AnnounceWinner()
        {
            if (_firstFighter.Health <= 0 && _secondFighter.Health <= 0)
            {
                Console.WriteLine("Поздравляю бойцы убили друг друга, никто не победил и все проиграли");
            }
            else if (_firstFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_secondFighter.Name} !");
            }
            else if (_secondFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_firstFighter.Name} !");
            }
        }

        public void Battle()
        {
            while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
            {
                _firstFighter.ShowStats();
                _secondFighter.ShowStats();
                _firstFighter.TakeDamege(_secondFighter.Damage);
                _secondFighter.TakeDamege(_firstFighter.Damage);
                _firstFighter.UseSpecialAttack();
                _secondFighter.UseSpecialAttack(); ;
            }
        }

        private void ChooseFighter(out Fighter fighter)
        {
            ShowFighters();
            Console.Write("Введите номер бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int inputID);

            if (isNumber == false)
            {
                Console.WriteLine("Вы ввели не коректные данные.");
                fighter = null;
            }
            else if (inputID > 0 && inputID - 1 < _fighters.Count)
            {
                fighter = _fighters[inputID - 1];
                _fighters.Remove(fighter);
                Console.WriteLine("Боец успешно выбран.");
            }
            else
            {
                Console.WriteLine("Бойца с таким номером отсутствует.");
                fighter = null;
            }
        }

        private void ShowFighters()
        {
            Console.WriteLine("Список доступный бойцов");

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.Write(i + 1);
                _fighters[i].ShowStats();
            }
        }
    }

    class Fighter
    {
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }

        public Fighter(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void TakeDamege(float damageFighter)
        {
            float finalDamage = 0;
            int absorbedArmorFactor = 100;

            if (Armor == 0)
            {
                Health -= damageFighter;
            }
            else
            {
                finalDamage = damageFighter / absorbedArmorFactor * Armor;
                Armor -= finalDamage;
                Health -= finalDamage;
            }

            Console.WriteLine($"{Name} нанес {finalDamage} урона");
        }

        public void ShowStats()
        {
            Console.WriteLine($"{Name}. Здоровье: {Health}. Броня: {Armor} Урон {Damage}");
        }

        public void UseSpecialAttack()
        {
            Random random = new Random();
            int rangeMaximalNumbers = 100;
            int chanceUsingAbility = random.Next(rangeMaximalNumbers);
            int chanceAbility = 20;

            if (chanceUsingAbility < chanceAbility)
            {
                UsePower();
            }
        }

        protected virtual void UsePower() { }
    }

    class Mystic : Fighter
    {
        private int _healthBuff = 5;

        public Mystic(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} использует мистическую силу. Здоровье увеличилось");
            Health += _healthBuff;
        }
    }

    class Archer : Fighter
    {
        private int _damageBuff = 55;
        private int _armorBuff = 45;

        public Archer(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} использует Натянутый лук.Урон увеличился {_damageBuff} и  броня стала крепче на {_armorBuff} увелечены");
            Damage += _damageBuff;
            Armor += _armorBuff;
        }
    }

    class Paladin : Fighter
    {
        private int _healthBuff = 60;

        public Paladin(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал молитву. Здоровье увелечено");
            Health += _healthBuff;
        }
    }

    class Barbarian : Fighter
    {
        private int _damageBuff = 60;
        private int _armorBuff = 50;

        public Barbarian(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} начал позировать своими мышцами, урон увеличился, но броня уменьшилась");
            Armor -= _armorBuff;
            Damage += _damageBuff;
        }
    }

    class PlagueDoctor : Fighter
    {
        private int _damageBuff = 20;

        public PlagueDoctor(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал чумной зелья, увеличивая урон атаки");
            Damage += _damageBuff;
        }
    }
}
