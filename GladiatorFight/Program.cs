using System;
using System.Collections.Generic;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main()
        {
            bool isWork = true;
            ConsoleKey exitButton = ConsoleKey.Enter;

            while (isWork)
            {
                Battle battle = new Battle();
                battle.Fight();
                Console.WriteLine($"Вы хотите выйти из программы?Нажмите Enter.\nДля продолжение работы нажмите любую другую клавишу");

                if (Console.ReadKey().Key == exitButton)
                {
                    isWork = false;
                    Console.WriteLine("Вы вышли из программы");
                }

                Console.Clear();
            }
        }
    }

    class Battle
    {
        private const string Warrior = "Warrior";
        private const string Archer = "Archer";
        private const string Paladin = "Paladin";
        private const string PlagueDoctor = "PlagueDoctor";
        private const string Barbarian = "Barbarian";
        private List<Fighter> _fighters = new List<Fighter>();
        private List<Warrior> _warriors = new List<Warrior>();
        private List<Archer> _archers = new List<Archer>();
        private List<Paladin> _paladins = new List<Paladin>();
        private List<PlagueDoctor> _plagueDoctors = new List<PlagueDoctor>();
        private List<Barbarian> _barbarians = new List<Barbarian>();
        private bool _isBattle;

        public Battle()
        {
            _isBattle = true;
            _warriors.Add(new Warrior("Jon", 200, 20, 40, 0, "Warrior", true));
            _archers.Add(new Archer("Lyk", 160, 15, 30, 1, "Archer", true));
            _paladins.Add(new Paladin("Boris", 300, 25, 50, 1, "Paladin", false));
            _plagueDoctors.Add(new PlagueDoctor("Sanek", 250, 30, 40, 0, "PlagueDoctor", true));
            _barbarians.Add(new Barbarian("Dem", 160, 10, 60, 3, "Barbarian", false));
            _fighters.AddRange(_warriors);
            _fighters.AddRange(_archers);
            _fighters.AddRange(_paladins);
            _fighters.AddRange(_plagueDoctors);
            _fighters.AddRange(_barbarians);
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
                switch (firstFighter.TypeFighter)
                {
                    case Warrior:
                        Warrior warriorShadow = (Warrior)_warriors[0].Clone();
                        _warriors.Add(warriorShadow);
                        secondFighter = warriorShadow;
                        break;
                    case Archer:
                        Archer archerShadow = (Archer)_archers[0].Clone();
                        _archers.Add(archerShadow);
                        secondFighter = archerShadow;
                        break;
                    case Paladin:
                        Paladin paladinShadow = (Paladin)_paladins[0].Clone();
                        _paladins.Add(paladinShadow);
                        secondFighter = paladinShadow;
                        break;
                    case PlagueDoctor:
                        PlagueDoctor plagueDoctorShadow = (PlagueDoctor)_plagueDoctors[0].Clone();
                        _plagueDoctors.Add(plagueDoctorShadow);
                        secondFighter = plagueDoctorShadow;
                        break;
                    case Barbarian:
                        Barbarian barbarianShadow = (Barbarian)_barbarians[0].Clone();
                        _barbarians.Add(barbarianShadow);
                        secondFighter = barbarianShadow;
                        break;
                }

                secondFighter.ChangeShadowFighter();
            }

            while (_isBattle == true)
            {
                SelectSkill(firstFighter, secondFighter);
                secondFighter.TakeDamage(firstFighter);
                SelectSkill(secondFighter, firstFighter);
                firstFighter.TakeDamage(secondFighter);
                ShowInfoRound(secondFighter, firstFighter, ref _isBattle);
            }
        }

        private void SelectSkill(Fighter fighter, Fighter opponent)
        {
            int idFighter = 0;

            if (fighter.IsStanFighter != true)
            {
                switch (fighter.TypeFighter)
                {
                    case Warrior:
                        if (fighter.IsBattleReplica)
                        {
                            idFighter = _warriors.Count - 1;
                        }
                        AttackWarrior(idFighter, fighter);
                        break;
                    case Archer:
                        if (fighter.IsBattleReplica)
                        {
                            idFighter = _archers.Count - 1;
                        }
                        AttackArcher(idFighter, fighter, opponent);
                        break;
                    case Paladin:
                        if (fighter.IsBattleReplica)
                        {
                            idFighter = _paladins.Count - 1;
                        }
                        AttackPaladin(idFighter, fighter);
                        break;
                    case PlagueDoctor:
                        if (fighter.IsBattleReplica)
                        {
                            idFighter = _plagueDoctors.Count - 1;
                        }
                        AttackPlagueDoctor(idFighter, fighter);
                        break;
                    case Barbarian:
                        if (fighter.IsBattleReplica)
                        {
                            idFighter = _barbarians.Count - 1;
                        }
                        AttackBarbarian(idFighter, fighter, opponent);
                        break;
                }
            }
        }

        private void AttackPaladin(int idFighter, Fighter fighter)
        {
            if (_paladins[idFighter].MinumumPointUseAbillity <= _paladins[idFighter].PointUseAbillity)
            {
                _paladins[idFighter].Feath();
                fighter.RemovePointUseAbillity(fighter.PointUseAbillity);
            }
        }

        private void AttackArcher(int idFighter, Fighter fighter, Fighter opponent)
        {
            if (_archers[idFighter].MinumumPointUseAbillity <= _archers[idFighter].PointUseAbillity)
            {
                _archers[idFighter].Shoot(opponent);
                fighter.RemovePointUseAbillity(fighter.MinumumPointUseAbillity);
            }
        }

        private void AttackWarrior(int idFighter, Fighter fighter)
        {
            if (_warriors[idFighter].MinumumPointUseAbillity <= _warriors[idFighter].PointUseAbillity)
            {
                _warriors[idFighter].BuffAttack();
                fighter.RemovePointUseAbillity(fighter.MinumumPointUseAbillity);
            }
        }

        private void AttackPlagueDoctor(int idFighter, Fighter fighter)
        {
            if (_plagueDoctors[idFighter].MinumumPointUseAbillity <= _plagueDoctors[idFighter].PointUseAbillity)
            {
                _plagueDoctors[idFighter].Healted();
                fighter.RemovePointUseAbillity(fighter.MinumumPointUseAbillity);
            }
        }

        private void AttackBarbarian(int idFighter, Fighter fighter, Fighter opponent)
        {
            if (_barbarians[idFighter].MinumumPointUseAbillity <= _barbarians[idFighter].PointUseAbillity)
            {
                _barbarians[idFighter].Kick(opponent);
                fighter.RemovePointUseAbillity(fighter.MinumumPointUseAbillity);
            }
        }

        private void ShowInfoRound(Fighter secondFighter, Fighter firstFighter, ref bool battle)
        {
            firstFighter.ShowInfo();
            secondFighter.ShowInfo();

            if (secondFighter.Health <= 0 && firstFighter.Health <= 0)
            {
                battle = false;
                Console.WriteLine($"Поздравляю бойцы убили друг друга, никто не победил и все проиграли ");
            }
            else if (secondFighter.Health <= 0)
            {
                battle = false;
                Console.WriteLine($"Победил {firstFighter.Name} ");
            }
            else if (firstFighter.Health <= 0)
            {
                battle = false;
                Console.WriteLine($"Победил {secondFighter.Name} ");
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
        public bool IsStanFighter { get; private set; }
        public bool IsSpellPozitiv { get; protected set; }
        public bool IsBattleReplica { get; private set; }
        public int Armor { get; private set; }
        public int PointUseAbillity { get; protected set; }
        public int MinumumPointUseAbillity { get; protected set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }
        public string TypeFighter { get; private set; }

        public Fighter(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 0;
            TypeFighter = typeFighter;
            IsStanFighter = false;
            IsBattleReplica = false;
        }

        public void TakeDamage(Fighter opponent)
        {
            if (opponent.IsStanFighter == false)
            {
                Health -= opponent.Damage - Armor;
                AddPointUseAbillity();
                Console.WriteLine($"Вражеский боец {opponent.Name} нанес  {Name} {opponent.Damage} урон");
            }
            else if (opponent.IsStanFighter == true)
            {
                opponent.IsStanFighter = false;
                Console.WriteLine($"{opponent.Name} оглушен, нанести удар сопернику не получилось");
            }
        }

        public void Stun(int damageKick, bool isSpellPozitiv)
        {
            ChangeArmor(damageKick, isSpellPozitiv);
            IsStanFighter = true;
        }

        public void ChangeArmor(int value, bool isSpellPozitiv)
        {
            if (isSpellPozitiv)
            {
                Armor += value;
            }
            else
            {
                Armor -= value;
            }
        }

        public void ChangeHealth(int value, bool isSpellPozitiv)
        {
            if (isSpellPozitiv)
            {
                Health += value;
            }
            else
            {
                Health -= value;
            }
        }

        public void ChangeDamage(int value, bool isSpellPozitiv)
        {
            if (isSpellPozitiv)
            {
                Damage += value;
            }
            else
            {
                Damage -= value;
            }
        }

        public void RemovePointUseAbillity(int MinumumPointUseAbillity)
        {
            PointUseAbillity -= MinumumPointUseAbillity;
        }

        public void AddPointUseAbillity()
        {
            PointUseAbillity++;
        }

        public void ChangeShadowFighter()
        {
            Name += "Replica ";
            IsBattleReplica = true;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя {Name} HP - {Health}  Damage - {Damage} Armor - {Armor} PointUseAbillity - {PointUseAbillity}");
        }
    }

    class Warrior : Fighter
    {
        private int _valueBuffDamage = 10;
        private int _valueDebaffArmor = 5;

        public Warrior(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter, bool isSpellPozitiv) : base(name, health, armor, damage, pointUseAbillity, typeFighter)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 3;
        }

        public object Clone()
        {
            return new Warrior(Name, Health, Armor, Damage, PointUseAbillity, TypeFighter, IsSpellPozitiv);
        }

        public void BuffAttack()
        {
            ChangeDamage(_valueBuffDamage, IsSpellPozitiv);
            IsSpellPozitiv = false;
            ChangeArmor(_valueDebaffArmor, IsSpellPozitiv);
            Console.WriteLine("Воин крикнул и повысил свой урон и снизив защиту");
        }
    }

    class Archer : Fighter
    {
        private int _damageShoot = 20;

        public Archer(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter, bool IsSpellPozitiv) : base(name, health, armor, damage, pointUseAbillity, typeFighter)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 1;
        }

        public object Clone()
        {
            return new Archer(Name, Health, Armor, Damage, PointUseAbillity, TypeFighter, IsSpellPozitiv);
        }

        public void Shoot(Fighter opponent)
        {
            opponent.ChangeHealth(_damageShoot, IsSpellPozitiv);
            Console.WriteLine($"{Name} выстрел из пистолета по {opponent.Name} чистым уронам на {_damageShoot}");
        }
    }

    class Paladin : Fighter
    {
        private int _useSpell = 0;
        private int _maximumUseSpell = 3;
        private int _valueBuffArmor = 2;

        public Paladin(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter, bool IsSpellPozitiv) : base(name, health, armor, damage, pointUseAbillity, typeFighter)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 4;
        }

        public object Clone()
        {
            return new Paladin(Name, Health, Armor, Damage, PointUseAbillity, TypeFighter, IsSpellPozitiv);
        }

        public void Feath()
        {
            if (_maximumUseSpell > _useSpell)
            {
                IsSpellPozitiv = true;
                ChangeArmor(_valueBuffArmor, IsSpellPozitiv);
                _useSpell++;
                Console.WriteLine("Паладин памолился и спомнил что его броня стала крепче");
            }
        }
    }

    class PlagueDoctor : Fighter
    {
        private int _magicPoint;
        private int _healUp = 10;

        public PlagueDoctor(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter, bool IsSpellPozitiv) : base(name, health, armor, damage, pointUseAbillity, typeFighter)
        {
            MinumumPointUseAbillity = 1;
            _magicPoint = 10;
        }

        public object Clone()
        {
            return new PlagueDoctor(Name, Health, Armor, Damage, PointUseAbillity, TypeFighter, IsSpellPozitiv);
        }

        public void Healted()
        {
            if (_magicPoint > MinumumPointUseAbillity)
            {
                ChangeHealth(_healUp, IsSpellPozitiv);
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

        public Barbarian(string name, int health, int armor, int damage, int pointUseAbillity, string typeFighter, bool IsSpellPozitiv) : base(name, health, armor, damage, pointUseAbillity, typeFighter)
        {
            PointUseAbillity = pointUseAbillity;
            MinumumPointUseAbillity = 5;
        }

        public object Clone()
        {
            return new Barbarian(Name, Health, Armor, Damage, PointUseAbillity, TypeFighter, IsSpellPozitiv);
        }

        public void Kick(Fighter opponent)
        {
            opponent.Stun(_damageKick, IsSpellPozitiv);
            Console.WriteLine($"{Name} пнул {opponent.Name} в лицо уменьшил его броню на {_damageKick} и застанил его ");
        }
    }
}