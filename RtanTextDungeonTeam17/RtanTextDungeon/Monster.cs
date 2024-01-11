using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Monster : IDamagable
    {
        // 생성 시 할당 이후 변화 없는 변수 name, lv, atk 는 readonly로 선언.
        private readonly string  _name;        
        private readonly int     _lv;
        private readonly float   _atk;        

        public string   Name    => _name;
        public int      Lv      => _lv;      
        public float    Atk     => _atk;

        private int     _hp;
        /// <summary>
        /// get 시, Hp 반환
        /// set 시, Hp = value 값 및 0 이하 시 Hp = 0, Dead 메서드 호출
        /// </summary>
        public int Hp 
        {
            get => _hp;
            private set
            {
                _hp = value;
                if(Hp <= 0)
                {
                    _hp = 0;
                    IsDead = true;
                }
            }
        }

        public bool     IsDead  { get; private set; } = false;

        private Define.MonsterType _type;
        // 타입에 따라 부여할 공격력/체력 (고정값)
        private readonly float[]    ATKs    = [5f, 10f, 15f, 7f, 12f, 18f];
        private readonly int[]      HPs     = [15, 10, 5, 20, 15, 10];

        public Monster(int lv, Define.MonsterType type)
        {
            _lv     = lv;
            _atk    = ATKs[(int)type] + ATKs[(int)type] * 0.2f * (_lv - 1);
            _hp     = HPs[(int)type] + (int)(HPs[(int)type] * 0.2f) * (_lv - 1);
            _type   = type;

            // 몬스터 타입에 따라 이름 할당.
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
                case Define.MonsterType.GoblinWorrior:
                    _name = $"Lv.{Lv}  고블린 전사";
                    break;
                case Define.MonsterType.GoblinArcher:
                    _name = $"Lv.{Lv}  고블린 궁수";
                    break;
                case Define.MonsterType.GoblinWizard:
                    _name = $"Lv.{Lv}  고블린 마법사";
                    break;
            }            
        }        

        public void GetDamage(float damage) => Hp -= (int)damage;

        // 인 게임 상에서 출력할 Text 함수, 오버로딩으로 전투씬 전,후 구별 사용
        public void ShowText()
        {
            if (IsDead)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{Name}  \tDead");
                Console.ResetColor();
            }                
            else
                Console.WriteLine($"{Name}  \tHP {Hp}");
        }
        public void ShowText(int index)
        {
            if (IsDead)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"({index}) {Name}  \tDead");
                Console.ResetColor();
                return;
            }
            Console.WriteLine($"({index}) {Name}  \tHP {Hp}");
        }
    }
}
