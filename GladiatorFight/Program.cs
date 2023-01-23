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

            while (isWork == true)
            {
                Battle battle = new Battle();
                battle.Fight();
                Console.WriteLine($"Вы хотите выйти из программы?Нажмите {exitButton}.\nДля продолжение работы нажмите любую другую клавишу");

                if (Console.ReadKey().Key == exitButton)
                {
                    Console.WriteLine("Вы вышли из программы");
                    isWork = false;
                }

                Console.Clear();
            }
        }
    }

    class Battle
    {
        private List<Fighter> _fighters = new List<Fighter>();
        private bool _isBattle;
        private Fighter _firstFighter;
        private Fighter _secondFighter;

        public Battle()
        {
            CreateFighters();
        }

        public void Fight()
        {
            ChooseFighters();

            while (_isBattle == true)
            {
                if (_firstFighter.Health > 0 & _secondFighter.Health > 0)
                {
                    _firstFighter.ShowInfo();
                    _secondFighter.ShowInfo();
                    _firstFighter.Attack(_secondFighter);
                    _secondFighter.Attack(_firstFighter);
                    Console.WriteLine("");
                }

                AnnounceWinner();
            }
        }

        private void CreateFighters()
        {
            _fighters.Add(new Warrior());
            _fighters.Add(new Archer());
            _fighters.Add(new Paladin());
            _fighters.Add(new PlagueDoctor());
            _fighters.Add(new Barbarian());
        }

        private void ChooseFighters()
        {
            Console.WriteLine("Выберите бойца:");
            AssignetFighter(ref _firstFighter);
            Console.WriteLine("Выберите второго бойца:");
            AssignetFighter(ref _secondFighter);
            _isBattle = true;
        }

        private void AssignetFighter(ref Fighter fighter)
        {
            bool isWork = true;

            while (isWork == true)
            {
                ShowFighters(_fighters);
                bool isNumber = int.TryParse(Console.ReadLine(), out int numberFighter);

                if ((isNumber == true && _fighters[numberFighter - 1] == _firstFighter))
                {
                    fighter = _fighters[numberFighter - 1].CreateShadow();
                    isWork = false;
                }
                else if (isNumber == true && numberFighter <= _fighters.Count && numberFighter > 0)
                {
                    fighter = _fighters[numberFighter - 1];
                    isWork = false;
                }
                else
                {
                    Console.WriteLine("\nВы ввели не число или данного бойца нет в списке\n");
                }
            }
        }

        private void AnnounceWinner()
        {
            if (_secondFighter.Health <= 0 && _firstFighter.Health <= 0)
            {
                Console.WriteLine($"Поздравляю бойцы убили друг друга, никто не победил и все проиграли ");
                _isBattle = false;
            }
            else if (_secondFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_firstFighter.Name} ");
                _isBattle = false;
            }
            else if (_firstFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_firstFighter.Name} ");
                _isBattle = false;
            }
        }

        private void ShowFighters(List<Fighter> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(i + 1 + " ");
                list[i].ShowInfo();
            }
        }
    }

    class Fighter
    {
        public int Armor { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }
        public int AttackSpeed { get; protected set; }

        public Fighter(string name, int health, int armor, int damage, int attackSpeed)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
            AttackSpeed = attackSpeed;
        }

        public virtual void Attack(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int Damage)
        {
            if (Armor >= Damage)
            {
                DebaffArmor(Damage);
            }
            else
            {
                Health -= Damage - Armor;
            }
        }

        public virtual Fighter CreateShadow()
        {
            return new Fighter(Name, Health, Armor, Damage, AttackSpeed);
        }

        public void BaffArmor(int value)
        {
            Armor += value;
        }

        public void DebaffArmor(int value)
        {
            Armor -= value;
        }

        public void BaffHealth(int value)
        {
            Health += value;
        }

        public void DebaffHealth(int value)
        {
            Health -= value;
        }

        public void BaffDamage(int value)
        {
            Damage += value;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя {Name} HP - {Health}  Damage - {Damage} Armor - {Armor}");
        }
    }

    class Warrior : Fighter
    {
        private int _valueBuffDamage = 10;
        private int _valueDebaffArmor = 5;

        public Warrior() : base("Jon", 200, 20, 40, 1)
        {
        }

        public override Fighter CreateShadow()
        {
            return base.CreateShadow();
        }

        public override void TakeDamage(int damage)
        {
            BaffDamage(_valueBuffDamage);
            DebaffArmor(_valueDebaffArmor);
            Console.WriteLine($"Воин крикнул и повысил урон на {_valueBuffDamage} и уменьшил броню {_valueDebaffArmor} ");
            base.TakeDamage(damage);
        }
    }

    class Archer : Fighter
    {
        private int _damageBaff = 20;
        private int _debaffHealth = 2;

        public Archer() : base("Lyk", 160, 15, 30, 2) { }

        public override Fighter CreateShadow()
        {
            return base.CreateShadow();
        }

        public override void TakeDamage(int Damage)
        {
            Console.WriteLine($"{Name} использует Натянутый лук Урон увеличился на {_damageBaff} и уменьшил здоровья на {_debaffHealth}");
            base.TakeDamage(_damageBaff);
            DebaffHealth(_debaffHealth);
        }
    }

    class Paladin : Fighter
    {
        private int _valueBuffArmor = 2;

        public Paladin() : base("Boris", 300, 25, 50, 3) { }

        public override Fighter CreateShadow()
        {
            return base.CreateShadow();
        }

        public override void TakeDamage(int Damage)
        {
            BaffArmor((Damage - _valueBuffArmor) / 2);
            base.TakeDamage(Damage);
            Console.WriteLine("Паладин памолился и спомнил что его броня стала крепче");
        }
    }

    class PlagueDoctor : Fighter
    {
        private int _magicPoint;
        private int _healUp;
        private int _health = 5;

        public PlagueDoctor() : base("Sanek", 250, 30, 40, 2)
        {
            _magicPoint = 10;
        }

        public override Fighter CreateShadow()
        {
            return base.CreateShadow();
        }

        public override void TakeDamage(int Damage)
        {
            RegenerateHealth();
            base.TakeDamage(Damage);
            Console.WriteLine($"Чумной доктор воспользовался маной, осталось {_magicPoint} маны и отхилил на {_healUp}");
        }

        private void RegenerateHealth()
        {
            if (_magicPoint > 0)
            {
                _healUp = Damage / _health;
                BaffHealth(_healUp);
                _magicPoint--;
            }
        }
    }

    class Barbarian : Fighter
    {
        private int _damageKick = 5;
        private int _doubleDamage;

        public Barbarian() : base("Dem", 160, 10, 60, 4)
        {
            _doubleDamage = _damageKick * AttackSpeed;
        }

        public override Fighter CreateShadow()
        {
            return base.CreateShadow();
        }

        public override void TakeDamage(int Damage)
        {
            base.TakeDamage(_doubleDamage);
            Console.WriteLine($"{Name} пнул  в лицо {_doubleDamage} перехватил инициативу и ударил ещё раз обычным ударом {Damage}");
            base.TakeDamage(Damage);
        }
    }
}
