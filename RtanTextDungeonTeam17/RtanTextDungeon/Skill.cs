using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
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

        public void Use(Player player, Monster monster)
        {
            //플레이어 MP 소모
            player.Mp -= Cost;
            //몬스터 피격
            monster.GetDamage(player.Atk * AtkMultiplier);
        }

        public void ShowText()
        {
            Console.WriteLine($"{OriginNumber}. {Name} - MP {Cost}");
            Console.WriteLine($"    {Description}\n");
        }
    }
}
