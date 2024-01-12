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
                case Define.MonsterType.TILFairy:
                    _name = $"Lv.{Lv}  [TIL 요정]";
                    break;
                case Define.MonsterType.HellofAlgorythm:
                    _name = $"Lv.{Lv}  [알고리즘 무사]";
                    break;
                case Define.MonsterType.VerifyGhost:
                    _name = $"Lv.{Lv}  [본인인증 유령]";
                    break;
                case Define.MonsterType.GitCrashCat:
                    _name = $"Lv.{Lv}  [깃허브 충돌 고양이]";
                    break;
                case Define.MonsterType.EnterMemoryEraser:
                    _name = $"Lv.{Lv}  [입실버튼 기억말소자]";
                    break;
                case Define.MonsterType.ExitMemoryEraser:
                    _name = $"Lv.{Lv}  [퇴실버튼 기억말소자]";
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
                Console.WriteLine($"{Name}  \tHP {Hp}  ATK {(int)Atk}");
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
            Console.WriteLine($"({index}) {Name}  \tHP {Hp}  ATK {(int)Atk}");
        }
    }
}
