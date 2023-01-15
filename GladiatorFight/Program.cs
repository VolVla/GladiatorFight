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
        private List<Fighter> _cloneFighters = new List<Fighter>();

        public Battle()
        {
            _fighters.Add(new Warrior("Jon", 200, 20, 40, 0, true));
            _fighters.Add(new Archer("Lyk", 160, 15, 30, 1, false));
            _fighters.Add(new Paladin("Boris", 300, 25, 50, 1, true));
            _fighters.Add(new PlagueDoctor("Sanek", 250, 30, 40, 0, true));
            _fighters.Add(new Barbarian("Dem", 160, 10, 60, 3, false));
            CreateShadowClones();
        }

        public void Fight()
        {
            ShowFighters(_fighters);
            Console.WriteLine("Выберите бойца:");
            int.TryParse(Console.ReadLine(), out int indexSecondFighter);
            Fighter firstFighter = _fighters[indexSecondFighter - 1];
            Console.WriteLine("Выберите второго бойца:");
            int.TryParse(Console.ReadLine(), out int indexFirstFighter);
            Fighter secondFighter = _fighters[indexFirstFighter - 1];

            if (secondFighter == firstFighter)
            {
                secondFighter = _cloneFighters[indexFirstFighter - 1];
            }

            while (secondFighter.Health >= 0 && firstFighter.Health >= 0)
            {
                secondFighter.TakeDamage(firstFighter);
                firstFighter.TakeDamage(secondFighter);
                firstFighter.ShowInfo();
                secondFighter.ShowInfo();

                if (secondFighter.Health <= 0 && firstFighter.Health <= 0)
                {
                    Console.WriteLine($"Поздравляю бойцы убили друг друга, никто не победил и все проиграли ");
                    break;
                }
                else if (secondFighter.Health <= 0)
                {
                    Console.WriteLine($"Победил {firstFighter.Name} ");
                    break;
                }
                else if (firstFighter.Health <= 0)
                {
                    Console.WriteLine($"Победил {secondFighter.Name} ");
                    break;
                }
            }
        }

        private void CreateShadowClones()
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                _cloneFighters.Add((Fighter)Clone(_fighters[i]));
            }
        }

        private object Clone(Fighter firstFighter)
        {
            return new Fighter(firstFighter.Name, firstFighter.Health, firstFighter.Armor, firstFighter.Damage, firstFighter.PointUseAbillity, firstFighter.UsePowerYourself);
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
        public Fighter(string name, int health, int armor, int damage, int pointUseAbillity, bool usePowerYourself)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 0;
            IsStanFighter = false;
            UsePowerYourself = usePowerYourself;
        }

        public bool IsStanFighter { get; private set; }
        public bool UsePowerYourself { get; protected set; }
        public int Armor { get; private set; }
        public int PointUseAbillity { get; protected set; }
        public int MinumumPointUseAbillity { get; protected set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }

        public void TakeDamage(Fighter opponent)
        {
            if (opponent.IsStanFighter == false)
            {
                Health -= opponent.Damage - Armor;
                UsePower(opponent);
                AddPointUseAbillity();
                Console.WriteLine($"Вражеский боец {opponent.Name} нанес  {Name} {opponent.Damage} урон");
            }
            else if (opponent.IsStanFighter == true)
            {
                opponent.IsStanFighter = false;
                Console.WriteLine($"{opponent.Name} оглушен, нанести удар сопернику не получилось");
            }
        }

        public void Stun(int damageKick)
        {
            DebaffArmor(damageKick);
            IsStanFighter = true;
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
            Console.WriteLine($"Имя {Name} HP - {Health}  Damage - {Damage} Armor - {Armor} PointUseAbillity - {PointUseAbillity}");
        }

        private void RemovePointUseAbillity()
        {
            PointUseAbillity -= MinumumPointUseAbillity;
        }

        private void AddPointUseAbillity()
        {
            PointUseAbillity++;
        }

        private void UsePower(Fighter opponent)
        {
            if (MinumumPointUseAbillity <= PointUseAbillity)
            {
                if (UsePowerYourself == true)
                {
                    UseSpecialSkill();
                }
                else if (UsePowerYourself == false)
                {
                    UseSpecialAttack(opponent);
                }

                RemovePointUseAbillity();
            }
        }

        protected virtual void UseSpecialSkill() { }

        protected virtual void UseSpecialAttack(Fighter opponent) { }
    }

    class Warrior : Fighter
    {
        private int _valueBuffDamage = 10;
        private int _valueDebaffArmor = 5;

        public Warrior(string name, int health, int armor, int damage, int pointUseAbillity, bool UsePowerYourself) : base(name, health, armor, damage, pointUseAbillity, UsePowerYourself)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 3;
        }

        protected override void UseSpecialSkill()
        {
            BaffDamage(_valueBuffDamage);
            DebaffArmor(_valueDebaffArmor);
            Console.WriteLine("Воин крикнул и повысил свой урон и снизив защиту");
        }
    }

    class Archer : Fighter
    {
        private int _damageShoot = 20;

        public Archer(string name, int health, int armor, int damage, int pointUseAbillity, bool UsePowerYourself) : base(name, health, armor, damage, pointUseAbillity, UsePowerYourself)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 1;
        }

        protected override void UseSpecialAttack(Fighter opponent)
        {
            opponent.DebaffHealth(_damageShoot);
            Console.WriteLine($"{Name} выстрел из пистолета по {opponent.Name} чистым уронам на {_damageShoot}");
        }
    }

    class Paladin : Fighter
    {
        private int _useSpell = 0;
        private int _maximumUseSpell = 3;
        private int _valueBuffArmor = 2;

        public Paladin(string name, int health, int armor, int damage, int pointUseAbillity, bool UsePowerYourself) : base(name, health, armor, damage, pointUseAbillity, UsePowerYourself)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 4;
        }

        protected override void UseSpecialSkill()
        {
            if (_maximumUseSpell > _useSpell)
            {
                BaffArmor(_valueBuffArmor);
                _useSpell++;
                Console.WriteLine("Паладин памолился и спомнил что его броня стала крепче");
            }
        }
    }

    class PlagueDoctor : Fighter
    {
        private int _magicPoint;
        private int _healUp = 10;

        public PlagueDoctor(string name, int health, int armor, int damage, int pointUseAbillity, bool UsePowerYourself) : base(name, health, armor, damage, pointUseAbillity, UsePowerYourself)
        {
            MinumumPointUseAbillity = 1;
            _magicPoint = 10;
        }

        protected override void UseSpecialSkill()
        {
            if (_magicPoint > MinumumPointUseAbillity)
            {
                BaffHealth(_healUp);
                _magicPoint -= MinumumPointUseAbillity; ;
                Console.WriteLine($"Чумной доктор воспользовался маной, осталось {_magicPoint} маны и отхилил на {_healUp}");
            }
            else
            {
                Console.WriteLine("Чумной доктор попытался использовать ману, но чувствует усталось и эффект  востановления сил не сработал ");
            }
        }
    }

    class Barbarian : Fighter
    {
        private int _damageKick = 5;

        public Barbarian(string name, int health, int armor, int damage, int pointUseAbillity, bool UsePowerYourself) : base(name, health, armor, damage, pointUseAbillity, UsePowerYourself)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 5;
        }

        protected override void UseSpecialAttack(Fighter opponent)
        {
            opponent.Stun(_damageKick);
            Console.WriteLine($"{Name} пнул {Name} в лицо уменьшил его броню на {_damageKick} и застанил его ");
        }
    }
}
