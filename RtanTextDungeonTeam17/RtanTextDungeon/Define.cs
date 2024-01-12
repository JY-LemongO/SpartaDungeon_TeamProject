using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Define
    {
        public enum PlayerClass
        {
            Worrior,
            Archer,
            Magic,
            Thief,
            Deadbeat,
        }

        public enum ItemType
        {
            Weapon,
            Armor,
            Amulet,
            Potion,
        }

        public enum DungeonDiff
        {
            Easy,
            Normal,
            Hard,            
        }

        public enum MonsterType
        {
            SkeletonWorrior,
            SkeletonArcher,
            SkeletonWizard,
            GoblinWorrior,
            GoblinArcher,
            GoblinWizard,
        }
    }
}
