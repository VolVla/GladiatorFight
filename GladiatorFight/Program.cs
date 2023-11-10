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
        private int _firstFighter = 0;
        private int _secondFighter = 1;
        private List<Fighter> _fighters = new List<Fighter>();
        private List<Fighter> _fightFighters = new List<Fighter>();

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
            if (_fightFighters[_firstFighter].Health <= 0)
            {
                Console.WriteLine($"Победил {_fightFighters[_secondFighter].Name} !");
            }
            else if (_fightFighters[_secondFighter].Health <= 0)
            {
                Console.WriteLine($"Победил {_fightFighters[_firstFighter].Name} !");
            }
            else if (_fightFighters[_firstFighter].Health <= 0 && _fightFighters[_secondFighter].Health <= 0)
            {
                Console.WriteLine("Поздравляю бойцы убили друг друга, никто не победил и все проиграли");
            }
        }

        public void Battle()
        {
            while (_fightFighters[_firstFighter].Health > 0 && _fightFighters[_secondFighter].Health > 0)
            {
                _fightFighters[_firstFighter].CauseDamage(_fightFighters[_secondFighter]);
                _fightFighters[_secondFighter].CauseDamage(_fightFighters[_firstFighter]);
            }
        }

        public bool TryCreativeBattle()
        {
            Console.WriteLine("Боец 1");
            ChooseFighter();
            Console.WriteLine("Боец 2");
            ChooseFighter();

            if (_fightFighters[_firstFighter] == null || _fightFighters[_secondFighter] == null)
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

        private void ChooseFighter()
        {
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
                _fightFighters.Add(_fighters[inputID - 1].Clone());
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
            float absorbedArmorFactor = 100;

            if (Armor == 0)
            {
                finalDamage = damageFighter;
                Health -= damageFighter;
            }
            else
            {
                finalDamage = damageFighter / absorbedArmorFactor * Armor;
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
            Console.WriteLine($"{Name} использует мистическую силу. Здоровье увеличилось");
            Health += _healthBuff;
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
        private Random _random = new Random();
        private float absorbedArmorFactor = 2;
        public PlagueDoctor(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        public override Fighter Clone()
        {
            return new PlagueDoctor(Name, Health, Armor, Damage);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал случайное зелье");

            if (_mana > 0)
            {
                switch (_random.Next(0, 2))
                {
                    case 0:
                        if (Armor != 0 && Health != 0)
                        {
                            Console.WriteLine("Выпил зелье которая обменяла броню и здоровье");
                            float _temporarValue = Armor;
                            Health = Armor;
                            Armor = _temporarValue;
                        }
                        break;
                    case 1:
                        if (Armor != 0)
                        {
                            Console.WriteLine("Выпил зелье которая уничтожела броню но увеличила наносимый урон");
                            Damage += Armor / absorbedArmorFactor;
                            Armor = 0;
                        }
                        break;
                }

                _mana--;
            }
        }
    }
}
