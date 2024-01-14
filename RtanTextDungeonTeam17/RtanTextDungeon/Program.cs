using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RtanTextDungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 144;
            Console.WindowHeight = 48;            
            
            // 타이틀 화면
            UI.AsciiArt(UI.AsciiPreset.TitleArt);
            Console.ReadKey();
            
            Console.SetCursorPosition(72, 30);            
            Console.Clear();
            
            // 인 게임 Dungeon클래스
            Dungeon dungeon = new Dungeon();
            // EnterGame 메서드로 게임에 입장한다. 들고갈 정보는 Player와 Shop
            dungeon.EnterGame();            
        }
    }
}
