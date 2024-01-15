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
            // 플레이어를 불러오기를 통해 생성한다.
            Console.SetCursorPosition(72, 30);
            Console.WriteLine("데이터를 불러오는중...");
            //Player player = new Player(1, "르탄이", Define.PlayerClass.Worrior, 10, 5, 100, 100, 1500);
            Thread.Sleep(1000);
            Console.Clear();

            // 인 게임 Dungeon클래스
            Dungeon dungeon = new Dungeon();
            dungeon.EnterGame();
        }
    }
}
