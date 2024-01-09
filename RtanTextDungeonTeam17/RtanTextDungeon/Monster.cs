using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Monster : IDamagable
    {
        private readonly string  _name;        
        private readonly int     _lv;
        private readonly int     _atk;        

        public string   Name    => _name;
        public int      Lv      => _lv;      
        public int      Atk     => _atk;

        private float   _hp;
        /// <summary>
        /// get 시 Hp 반환
        /// set 시 Hp = value 값 및 0 이하 시 Hp = 0, Dead 메서드 호출
        /// </summary>
        public float    Hp 
        {
            get => _hp;
            private set
            {
                _hp = value;
                if(Hp <= 0)
                {
                    _hp = 0;
                    Dead();
                }
            }
        }

        public bool     IsDead  { get; private set; } = false;

        private Define.MonsterType _type;
        private readonly int[]      ATKs    = [10, 15, 25];
        private readonly float[]    HPs     = [30f, 20f, 10f];

        public Monster(int lv, Define.MonsterType type)
        {
            _lv     = lv;
            _atk    = ATKs[(int)type];
            _hp     = HPs[(int)type];
            _type   = type;

            switch (_type)
            {
                case Define.MonsterType.SkeletonWorrior:
                    _name = $"Lv.{Lv}  스켈레톤 전사";
                    break;
                case Define.MonsterType.SkeletonArcher:
                    _name = $"Lv.{Lv}  스켈레톤 궁수";
                    break;
                case Define.MonsterType.SkeletonWizard:
                    _name = $"Lv.{Lv}  스켈레톤 마법사";
                    break;
            }            
        }
        
        public void Dead() => IsDead = true;

        public void GetDamage(float damage) => Hp -= damage;

        public void ShowText()
        {
            Console.WriteLine($"{Name}\t\tHP {Hp}");
        }
        public void ShowText(int index)
        {
            if (IsDead)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"({index}) {Name}\t\tDead");
                Console.ResetColor();
                return;
            }
            Console.WriteLine($"({index}) {Name}\t\tHP {Hp}");
        }
    }
}
