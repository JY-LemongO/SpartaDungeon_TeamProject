using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class UI
    {
        #region UserInput
        /// <summary>
        /// 경고문과 함께 사용자에게 입력을 받는 UI를 표시하고, 사용자가 입력한 값을 반환합니다.
        /// </summary>
        /// <param name="alert">표시할 경고문입니다. 기본값은 빈 문자열입니다.</param>
        /// <param name="positive">경고문의 성격이 긍정적인지 나타냅니다. 기본값은 false입니다.</param>
        /// <returns>사용자로부터 입력받은 문자열을 반환합니다.</returns>
        public static string UserInput(string alert="", bool positive=false, string reqMsg= "다음 행동을 입력하세요.")
        {
            // 입력 : (string)경고문, (bool)긍정적인 경고문인지
            // 출력 : (string)유저입력
            Console.WriteLine(".──────────────────────────────────────────────────────────────────── .");
            Console.WriteLine($" {reqMsg}");
            Console.WriteLine("");
            Console.WriteLine(""); Console.ForegroundColor = (positive)?ConsoleColor.Green:ConsoleColor.Red;
            Console.WriteLine($" {alert}"); Console.ResetColor();
            Console.WriteLine(" >>>"); 
            Console.WriteLine("'──────────────────────────────────────────────────────────────────── '");
            Console.SetCursorPosition(5, Console.CursorTop-2);
            return Console.ReadLine();
        }
        #endregion

        #region AsciiArt
        public enum AsciiPreset
        {
            MainMenu,
            Battle,
            Status,
            CreateCharacter,
            PotionInventory,
            Inventory,
            Shop,
            DungeonEntrance,
            Inn,

        }
        /// <summary>
        /// 아스키아트를 콘솔에 표시합니다.
        /// </summary>
        /// <param name="preset">표시할 아스키아트의 프리셋을 고릅니다. </param>
        public static void AsciiArt(AsciiPreset preset)
        {
            switch (preset)
            {
                case AsciiPreset.MainMenu:
                    Console.WriteLine(
                        "===============================================================================================\r\n" +
                        " _______                      __            _____                                              \r\n" +
                        "|     __|.-----..---.-..----.|  |_ .---.-. |     \\ .--.--..-----..-----..-----..-----..-----.  \r\n" +
                        "|__     ||  _  ||  _  ||   _||   _||  _  | |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |  \r\n" +
                        "|_______||   __||___._||__|  |____||___._| |_____/ |_____||__|__||___  ||_____||_____||__|__|  \r\n" +
                        "         |__|                                                    |_____|                       \r\n" +
                        "===============================================================================================\r\n" +
                        "                            └ 스파르타 던전에 온 것을 환영합니다 ┘                             \r\n"
                        );
                    //int rtanX = 42; int rtanY = 11;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("            -*++%-            "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("            -%%%%-            "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("             +%%+             "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("          %####%%%%#          "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        *%#*##%%%###%*        "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        %#########%%%@        "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        %##:-+###=-:%@        "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("     :+*%#%=.-#%#:.+%@*=:     "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("     %:-@%%#=.....*%%@-:%     "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("       *.*%%%:...-%%*.#       "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        @+:+++++*#+:+%@       "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        %#%........:@#@       "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        %#%........:@#@       "); rtanY++;
                    //Console.SetCursorPosition(rtanX, rtanY); Console.Write("        %#%+++++*###@#@       ");
                    //Console.SetCursorPosition(0, 9);

                    break;

                case AsciiPreset.Battle:
                    Console.Clear();
                    Console.WriteLine(
                        "                                                             \r\n" +
                        "        '||'''|,            ||      ||    '||`               \r\n" +
                        "         ||   ||            ||      ||     ||                \r\n" +
                        "         ||;;;;    '''|.  ''||''  ''||''   ||  .|''|,        \r\n" +
                        "         ||   ||  .|''||    ||      ||     ||  ||..||        \r\n" +
                        "        .||...|'  `|..||.   `|..'   `|..' .||. `|...         \n\n" +
                        "===========================[전 투]===========================\r\n"
                        );
                    break;

                case AsciiPreset.Status:
                    Console.WriteLine(" _______  __           __                 ");
                    Console.WriteLine("|     __||  |_ .---.-.|  |_ .--.--..-----.");
                    Console.WriteLine("|__     ||   _||  _  ||   _||  |  ||__ --|");
                    Console.WriteLine("|_______||____||___._||____||_____||_____|");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("=================[상태 창]=================");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Inventory:
                    Console.WriteLine(" _______                              __                       ");
                    Console.WriteLine("|_     _|.-----..--.--..-----..-----.|  |_ .-----..----..--.--.");
                    Console.WriteLine(" _|   |_ |     ||  |  ||  -__||     ||   _||  _  ||   _||  |  |");
                    Console.WriteLine("|_______||__|__| \\___/ |_____||__|__||____||_____||__|  |___  |");
                    Console.WriteLine("                                                        |_____|");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("============================[인벤토리]============================");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.CreateCharacter:
                    Console.WriteLine(
                        ""
                        );
                    break;

                case AsciiPreset.PotionInventory:
                    Console.WriteLine("                                                                   ");
                    Console.WriteLine("   .-.   p--- .'~`. .-=~=-.   :~:      |~|   .-~~8~~-.  |~|  .-.   ");
                    Console.WriteLine(" .'__( .'~`.  `. .'  )___(  .'   `.    | |   |~~---~~|  | |  )__`. ");
                    Console.WriteLine(" | l | | m |  .'n`. (  o  ) |  p  |] .' q `. |   r   | .'s`. | t | ");
                    Console.WriteLine(" |___| |___|  `._.'  `._.'  |_____|  `.___.' `._____.' `._.' |___| ");
                    Console.WriteLine("                                                                   ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("===========================[회복 아이템]===========================");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Shop:
                    Console.WriteLine(" _______  __                  ");
                    Console.WriteLine("|     __||  |--..-----..-----.");
                    Console.WriteLine("|__     ||     ||  _  ||  _  |");
                    Console.WriteLine("|_______||__|__||_____||   __|");
                    Console.WriteLine("                       |__|   ");
                    Console.WriteLine("                              ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("============[상   점]============");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.DungeonEntrance:

                    Console.WriteLine(" _____                                            ");
                    Console.WriteLine("|     \\ .--.--..-----..-----..-----..-----..-----.");
                    Console.WriteLine("|  --  ||  |  ||     ||  _  ||  -__||  _  ||     |");
                    Console.WriteLine("|_____/ |_____||__|__||___  ||_____||_____||__|__|");
                    Console.WriteLine("                      |_____|                     ");
                    Console.WriteLine("                                                  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("====================[던전 입구]====================");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Inn:

                    Console.WriteLine("");
                    Console.WriteLine(" __               ");
                    Console.WriteLine("|__|.-----..-----.");
                    Console.WriteLine("|  ||     ||     |");
                    Console.WriteLine("|__||__|__||__|__|");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==================[여 관]==================");
                    Console.ResetColor();
                    Console.WriteLine("");

                    break;

                default:
                    Console.WriteLine( "아스키아트 프리셋이 없습니다.");
                    break;

            }
        }
        #endregion
    }
}
