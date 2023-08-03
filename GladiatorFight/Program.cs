using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

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
                _firstFighter.CauseDamage(_secondFighter);
                _secondFighter.CauseDamage(_firstFighter);
            }
        }

        public bool TryCreativeBattle()
        {
            int idFighter = 0;

            Console.WriteLine("Боец 1");
           _firstFighter = ChooseFighter( _firstFighter,ref idFighter);
            Console.WriteLine("Боец 2");
           _secondFighter = ChooseFighter( _secondFighter, ref idFighter);

            if(_firstFighter == _secondFighter)
            {
                _secondFighter = CreateFantomFighter(idFighter, _firstFighter );
            }

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

        private Fighter CreateFantomFighter(int inputID,Fighter fighter)
        {
            if (_firstFighter == fighter)
            {
                switch (inputID-1) 
                {
                    case 0:
                            fighter = new Archer(_fighters[inputID - 1].Name + " 2 такой же боец", _fighters[inputID - 1].Health, _fighters[inputID - 1].Armor, _fighters[inputID - 1].Damage);
                    break;
                    case 1:
                            fighter = new PlagueDoctor(_fighters[inputID-1].Name + " 2 такой же боец", _fighters[inputID-1].Health, _fighters[inputID-1].Armor, _fighters[inputID-1].Damage);
                    break;
                    case 2:
                            fighter = new Paladin(_fighters[inputID].Name + " 2 такой же боец", _fighters[inputID].Health, _fighters[inputID].Damage, _fighters[inputID].Armor);
                    break;
                    case 3:
                            fighter = new Barbarian(_fighters[inputID].Name + " 2 такой же боец", _fighters[inputID].Health, _fighters[inputID].Damage, _fighters[inputID].Armor);
                    break;
                    case 4:
                            fighter =new Mystic(_fighters[inputID].Name + " 2 такой же боец", _fighters[inputID].Health, _fighters[inputID].Damage, _fighters[inputID].Armor);
                    break;
                }
            }

            return fighter;
        }

        private Fighter ChooseFighter(Fighter fighter, ref int idFighter)
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
                Console.WriteLine("Боец успешно выбран.");
                fighter = _fighters[inputID - 1];
            }

            idFighter = inputID;
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

    class Fighter
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
            Console.WriteLine($"{fighter.Name} атаковал {Name}");
            TakeDamage(fighter.Damage);
            fighter.UsePower();
            ShowStats();
        }

        protected virtual void TakeDamage(float damageFighter)
        {
            float finalDamage = 0;
            float absorbedArmorFactor = 100;

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

            Console.WriteLine($"{Name} получил {finalDamage} урона");
        }

        protected virtual void UsePower() 
        {
            int rangeMaximalNumbers = 80;
            int chanceAbility = 20;
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

        public Archer(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} использует Натянутый лук.Урон увеличился {_damageBuff} и  броня стала крепче на {_armorBuff} увелечены");
            Damage += _damageBuff;
            Armor += _armorBuff;
        }
    }

    class Paladin : Fighter
    {
        private int _healthBuff = 10;

        public Paladin(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

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

        public Barbarian(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            base.UsePower();
        }

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

        public PlagueDoctor(string name, float health, float armor, float damage) : base(name, health, damage, armor) { }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал чумной зелья, увеличивая урон атаки");
            Damage += _damageBuff;
        }
    }
}
