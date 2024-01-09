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
        }

        public enum ItemType
        {
            Weapon,
            Armor,
            Amulet,
        }

        public enum DungeonDiff
        {
            Easy,
            Normal,
            Hard,
            Test,
        }
    }
}
