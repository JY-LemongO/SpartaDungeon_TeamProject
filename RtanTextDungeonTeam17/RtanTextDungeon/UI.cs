using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class UI
    {

        public static string UserInput(string alert="")
        {
            Console.WriteLine(".────────────────────────────────────────────────────── .");
            Console.WriteLine(" 다음 행동을 골라주세요");
            Console.WriteLine("");
            Console.WriteLine(""); Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {alert}"); Console.ResetColor();
            Console.WriteLine(" >>>"); 
            Console.WriteLine("'────────────────────────────────────────────────────── '");
            Console.SetCursorPosition(5, Console.CursorTop-2);
            return Console.ReadLine();
        }
        public enum AsciiPreset
        {
            MainMenu,
            Battle,
            CreateCharacter,
            PotionInventory,

        }

        public static string AsciiArt(AsciiPreset preset)
        {
            // 사용법
            // string art = UI.AsciiArt(UI.AsciiPreset.Battle);

            string content = "";

            switch (preset)
            {
                case AsciiPreset.MainMenu:
                    content =
                        "===============================================================================================\r\n" +
                        " _______                      __            _____                                              \r\n" +
                        "|     __|.-----..---.-..----.|  |_ .---.-. |     \\ .--.--..-----..-----..-----..-----..-----.  \r\n" +
                        "|__     ||  _  ||  _  ||   _||   _||  _  | |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |  \r\n" +
                        "|_______||   __||___._||__|  |____||___._| |_____/ |_____||__|__||___  ||_____||_____||__|__|  \r\n" +
                        "         |__|                                                    |_____|                       \r\n" +
                        "===============================================================================================\r\n" +
                        "                            └ 스파르타 던전에 온 것을 환영합니다 ┘                             \r\n";
                    break;

                case AsciiPreset.Battle:
                    content =
                        "                                                             \r\n" +
                        "        '||'''|,            ||      ||    '||`               \r\n" +
                        "         ||   ||            ||      ||     ||                \r\n" +
                        "         ||;;;;    '''|.  ''||''  ''||''   ||  .|''|,        \r\n" +
                        "         ||   ||  .|''||    ||      ||     ||  ||..||        \r\n" +
                        "        .||...|'  `|..||.   `|..'   `|..' .||. `|...         \n\n" +
                        "===========================[전 투]===========================\r\n";
                    break;

                case AsciiPreset.CreateCharacter:
                    content = 
                        "";
                    break;

                case AsciiPreset.PotionInventory:
                    content =
                        "                                                                   \r\n" +
                        "   .-.   p--- .'~`. .-=~=-.   :~:      |~|   .-~~8~~-.  |~|  .-.   \r\n" +
                        " .'__( .'~`.  `. .'  )___(  .'   `.    | |   |~~---~~|  | |  )__`. \r\n" +
                        " | l | | m |  .'n`. (  o  ) |  p  |] .' q `. |   r   | .'s`. | t | \r\n" +
                        " |___| |___|  `._.'  `._.'  |_____|  `.___.' `._____.' `._.' |___| \r\n" +
                        "                                                                   \r\n" +
                        "===========================[회복 아이템]===========================\r\n";
                    break;

                default:
                    content="아스키아트 프리셋이 없습니다.";
                    break;

            }

            return content;
        }
    }
}
