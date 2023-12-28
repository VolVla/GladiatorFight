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

                if (battleArena.TryCreateBattle())
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

        public BattleArena()
        {
            _fighters.Add(new Archer("Лучник", 100, 100, 20));
            _fighters.Add(new PlagueDoctor("Чумной Доктор", 100, 100, 15));
            _fighters.Add(new Paladin("Паладин", 100, 100, 50));
            _fighters.Add(new Barbarian("Варвар", 90, 100, 30));
            _fighters.Add(new Mystic("Мистик", 100, 100, 22));
        }

        public void AnnounceWinner()
        {
            if (_firstFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_secondFighter.Name} !");
            }
            else if (_secondFighter.Health <= 0)
            {
                Console.WriteLine($"Победил {_firstFighter.Name} !");
            }
            else if (_firstFighter.Health <= 0 && _secondFighter.Health <= 0)
            {
                Console.WriteLine("Поздравляю бойцы убили друг друга, никто не победил и все проиграли");
            }
        }

        public void Battle()
        {
            while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
            {
                _firstFighter.CauseDamage(_secondFighter);
                _secondFighter.CauseDamage(_firstFighter);
            }
        }

        public bool TryCreateBattle()
        {
            Console.WriteLine("Боец 1");
            _firstFighter = ChooseFighter();
            Console.WriteLine("Боец 2");
            _secondFighter = ChooseFighter();

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

        private Fighter ChooseFighter()
        {
            Fighter fighter = null;
            ShowFighters();
            Console.Write("Введите номер бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int inputID);

            if (isNumber == false)
            {
                Console.WriteLine("Вы ввели не коректные данные.");
            }
            else if (inputID > 0 && inputID - 1 < _fighters.Count)
            {
                Console.WriteLine("Боец успешно выбран.");
                fighter = _fighters[inputID - 1].Clone();
            }

            return fighter;
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

    abstract class Fighter
    {
        public Fighter(string name, float health, float damage, float armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }

        public void ShowStats()
        {
            Console.WriteLine($"{Name}. Здоровье: {Health}. Броня: {Armor} Урон {Damage}");
        }

        public void CauseDamage(Fighter fighter)
        {
            Console.WriteLine($"{Name} атаковал {fighter.Name}");
            fighter.TakeDamage(Damage);
            UsePower();
            ShowStats();
        }

        public abstract Fighter Clone();

        protected virtual void TakeDamage(float damageFighter)
        {
            float finalDamage = 0;

            if (Armor <= 0)
            {
                finalDamage = damageFighter;
                Health -= damageFighter;
            }
            else
            {
                finalDamage = damageFighter / Armor;
                Armor -= finalDamage;
                Health -= finalDamage;
            }

            Console.WriteLine($"{Name} получил {finalDamage} урона");
        }

        protected virtual void UsePower()
        {
            int rangeMaximalNumbers = 80;
            int chanceAbility = 50;
            Random random = new Random();
            int chanceUsingAbility = random.Next(rangeMaximalNumbers);

            if (chanceUsingAbility < chanceAbility)
            {
                UsePower();
            }
        }
    }

    class Mystic : Fighter
    {
        private int _healthBuff = 5;
        private int _coinsUsePower = 2;

        public Mystic(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        public override Fighter Clone()
        {
            return new Mystic(Name, Health, Armor, Damage);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            if (_coinsUsePower > 0)
            {
                Console.WriteLine($"{Name} использует мистическую силу. Здоровье увеличилось");
                Health += _healthBuff;
                _coinsUsePower--;
            }
        }
    }

    class Archer : Fighter
    {
        private int _damageBuff = 5;
        private int _armorBuff = 5;
        private int _coolDown = 3;
        private int _valueCoolDown = 1;

        public Archer(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        public override Fighter Clone()
        {
            return new Archer(Name, Health, Armor, Damage);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            if (_valueCoolDown == _coolDown)
            {
                Console.WriteLine($"{Name} использует Натянутый лук.Урон увеличился {_damageBuff} и  броня стала крепче на {_armorBuff} увелечены");
                Damage += _damageBuff;
                Armor += _armorBuff;
                _valueCoolDown = 0;
            }
            else
            {
                _valueCoolDown++;
            }
        }
    }

    class Paladin : Fighter
    {
        private int _healthBuff = 10;

        public Paladin(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        public override Fighter Clone()
        {
            return new Paladin(Name, Health, Armor, Damage);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал молитву. Здоровье увелечено");
            Health += _healthBuff;
        }
    }

    class Barbarian : Fighter
    {
        private int _damageBuff = 30;
        private int _armorBuff = 20;
        private int _coolDown = 3;
        private int _valueCoolDown = 3;
        private float _baseDamage;
        private float _baseArmor;

        public Barbarian(string name, float health, float armor, float damage) : base(name, health, damage, armor)
        {
            _baseDamage = damage;
            _baseArmor = armor;
        }

        public override Fighter Clone()
        {
            return new Barbarian(Name, Health, Armor, Damage);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            base.UsePower();
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} начал позировать своими мышцами, урон увеличился, но броня уменьшилась");

            if (_coolDown == _valueCoolDown)
            {
                Armor = _baseArmor - _armorBuff;
                Damage = _baseDamage + _damageBuff;
                _coolDown = 0;
            }
            else
            {
                Armor = _baseArmor;
                Damage = _baseDamage;
                _coolDown++;
            }
        }
    }

    class PlagueDoctor : Fighter
    {
        private int _mana = 2;
        private float _health = 20;
        private float _absorbedArmorFactor = 2;

        public PlagueDoctor(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        public override Fighter Clone()
        {
            return new PlagueDoctor(Name, Health, Armor, Damage);
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал случайное зелье");

            if (_mana > 0)
            {
                if (Armor != 0 && Health < _health)
                {
                    Console.WriteLine("Выпил зелье которая обменяла броню и здоровье");
                    float _temporarValue = Armor;
                    Health = Armor;
                    Armor = _temporarValue;
                }
                else if (Armor != 0)
                {
                    Console.WriteLine("Выпил зелье которая уничтожела броню но увеличила наносимый урон");
                    Damage += Armor / _absorbedArmorFactor;
                    Armor = 0;
                }
            }

            _mana--;
        }
    }
}
