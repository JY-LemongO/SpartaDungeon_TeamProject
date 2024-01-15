using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Skill
    {
        public int OriginNumber         { get; set; }
        public string Name              { get; set; }
        public int Cost                 { get; set; }
        public int AtkMultiplier        { get; set; }
        public string Description       { get; set; }
        public int NumberTargets        { get; set; }   //스킬로 공격할 대상의 수 : 1~5만 존재

        static private int tempNumber = 1; //정적 변수 증가시켜 넘버링

        public Skill(string name, int cost, int atkMultiplier, string description, int numberTargets)
        {
            Name = name;
            Cost = cost;
            AtkMultiplier = atkMultiplier;
            Description = description;
            OriginNumber = tempNumber++;
            NumberTargets = numberTargets;
            if (NumberTargets < 1) NumberTargets = 1;
            if (NumberTargets > 5) NumberTargets = 5;
        }

        

        public float UseSkill(Player player, float originDamage)
        {
            player.Mp -= Cost;                       // 마나 소모
            return originDamage * AtkMultiplier;    // 스킬 배수 계산 반영 데미지 반환
        }

        public bool IsAvailable(Player player)                   // 마나 잔여량으로 사용 가능 체크
        {
            return player.Mp >= Cost;
        }

        public void ShowText() // 스킬 정보 출력
        {
            Console.WriteLine($"  {OriginNumber}. {Name} - MP {Cost}");
            Console.WriteLine($"     {Description}\n");
        }
    }
}
