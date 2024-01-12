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
            Console.WriteLine("");
            Console.WriteLine(".──────────────────────────────────────────────────────────────────── .");
            Console.WriteLine($"  {reqMsg}");
            Console.WriteLine("");
            Console.WriteLine(""); Console.ForegroundColor = (positive)?ConsoleColor.Green:ConsoleColor.Red;
            Console.WriteLine($"  {alert}"); Console.ResetColor();
            Console.WriteLine("  >>>"); 
            Console.WriteLine("'──────────────────────────────────────────────────────────────────── '");
            Console.SetCursorPosition(6, Console.CursorTop-2);
            return Console.ReadLine();
        }
        #endregion

        #region AsciiArt
        public enum AsciiPreset
        {
            TitleArt,
            CreateCharacter,
            SelectClass,
            MainMenu,
            Battle,
            Status,
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
            ConsoleColor statusColor = ConsoleColor.Gray;

            switch (preset)
            {
                case AsciiPreset.TitleArt:
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("          +                           -========-                                      +        ");
                    Console.WriteLine("                                      %%++++*#@%                  +                            ");
                    Console.WriteLine("                         +            %#++++*#@%                                               ");
                    Console.WriteLine("                                      %%######@%                                               ");
                    Console.WriteLine("                  *                   %%#%%%%#@%                                               ");
                    Console.WriteLine("                                      %@@%%%@@@%                                   +           ");
                    Console.WriteLine("                                       .@##%@@                                                 ");
                    Console.WriteLine("                                   %@@@@@##%@@@@@@@                                            ");
                    Console.WriteLine("      +                         .%%########%%%%%%%%%*.                       *                 ");
                    Console.WriteLine("                              *#%####*###%%%%%%%####%%*+                                       ");
                    Console.WriteLine("              +             +######+*###############%%%@#=       +                          +  ");
                    Console.WriteLine("                            %@#####################%%%%%@+                                     ");
                    Console.WriteLine("                            %@###################%%%%%%%@+                                     ");
                    Console.WriteLine("                            %@###%%@@@@%########@@@@@%%%@+                                     ");
                    Console.WriteLine("                            %@###%#....-%#####%#....-%%%@+                                     ");
                    Console.WriteLine("                            %@###%#:+%%*:*####::%%%:=%%#@*                                     ");
                    Console.WriteLine("                         *##@@###%%#-..-#**%%%#*...*#%%%@@##*                +                 ");
                    Console.WriteLine("                       +@:..%@%##%#+:....====+.....=+%%%@#..:@-                                ");
                    Console.WriteLine("                       +@:..%@%%%%%%#*...........-*%%%%%@#..:@-                                ");
                    Console.WriteLine("                         %*..-@@%%%%%#%+........#%%%%%@@.:.*%                                  ");
                    Console.WriteLine("            *             :@:..*@%%%%#%+........#%%%@@:..-@.                                   ");
                    Console.WriteLine("                            %%...%@@@@@*.......:@@@@#...@@@.                                   ");
                    Console.WriteLine("     +                      %@@=..:###++++++++*###*...#@##@.                                   ");
                    Console.WriteLine("                            %@#%@:....**+++++####-..-@####@.         +                         ");
                    Console.WriteLine("                            %@###@%.................-@%###@.                                   ");
                    Console.WriteLine("              +             %@###@#.................-@%###@.             [아무 키나 눌러 ]     ");
                    Console.WriteLine("                            %@###@#.................-@%###@.             [게임을 시작하기]     ");
                    Console.WriteLine("                            %@###@#.................-@%###@.                                   ");
                    Console.WriteLine("                            %@###@%===========******#@%##%@.                                   ");
                    Console.WriteLine("                            #@###@%+++##############%@%##%@.       +                           ");
                    Console.WriteLine("");
                    Console.WriteLine("===============================================================================================");
                    Console.WriteLine("          ____       *           _          ____                                          +    ");
                    Console.WriteLine("         / ___| _ __   __ _ _ __| |_ __ _  |  _ \\ _   _ _ __   __ _  ___  ___  _ __  ");
                    Console.WriteLine("    *    \\___ \\| '_ \\ / _` | '__| __/ _` | | | | | | | | '_ \\ / _` |/ _ \\/ _ \\| '_ \\ ");
                    Console.WriteLine("          ___) | |_) | (_| | |  | || (_| | | |_| | |_| | | | | (_| |  __/ (_) | | | |");
                    Console.WriteLine("         |____/| .__/ \\__,_|_|   \\__\\__,_| |____/ \\__,_|_| |_|\\__, |\\___|\\___/|_| |_|");
                    Console.WriteLine("               |_|   +                                        |___/           +                ");
                    Console.WriteLine("                                     +                                                         ");
                    Console.WriteLine("===============================================================================================");

                    Random random = new Random();
                    for(int i= 0; i < 1; i++)
                    {
                        Console.SetCursorPosition(random.Next(0, 85), random.Next(0, 41));
                        RandomColoredWrite("*");// 랜덤 색으로 문자열 표현
                    }
                    Console.SetCursorPosition(0, 42);

                    break;

                case AsciiPreset.MainMenu:

                    Console.WriteLine("===============================================================================================");
                    Console.WriteLine("");
                    Console.WriteLine(" _______                      __            _____                                              ");
                    Console.WriteLine("|     __|.-----..---.-..----.|  |_ .---.-. |     \\ .--.--..-----..-----..-----..-----..-----. ");
                    Console.WriteLine("|__     ||  _  ||  _  ||   _||   _||  _  | |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |  ");
                    Console.WriteLine("|_______||   __||___._||__|  |____||___._| |_____/ |_____||__|__||___  ||_____||_____||__|__|  ");
                    Console.WriteLine("         |__|                                                    |_____|                       ");
                    Console.WriteLine("");
                    Console.WriteLine("===============================================================================================");
                    Console.Write("                         └ [ ");
                    RandomColoredWrite("스파르타 던전에 오신것을 환영합니다");// 랜덤 색으로 문자열 표현
                    Console.WriteLine(" ] ┘                           ");
                    Console.WriteLine("");
                    Console.WriteLine("");

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

                case AsciiPreset.CreateCharacter:
                    Console.WriteLine("                                                                         ");
                    Console.WriteLine("         _   _ _______        __   ____    _    __  __ _____         ");
                    Console.WriteLine("        | \\ | | ____\\ \\      / /  / ___|  / \\  |  \\/  | ____|        ");
                    Console.WriteLine("        |  \\| |  _|  \\ \\ /\\ / /  | |  _  / _ \\ | |\\/| |  _|          ");
                    Console.WriteLine("        | |\\  | |___  \\ V  V /   | |_| |/ ___ \\| |  | | |___         ");
                    Console.WriteLine("        |_| \\_|_____|  \\_/\\_/     \\____/_/   \\_\\_|  |_|_____|        ");
                    Console.WriteLine("");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("----------------------------[캐릭터 생성]----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;


                case AsciiPreset.SelectClass:
                    Console.WriteLine("                                                                         ");
                    Console.WriteLine("         _   _ _______        __   ____    _    __  __ _____         ");
                    Console.WriteLine("        | \\ | | ____\\ \\      / /  / ___|  / \\  |  \\/  | ____|        ");
                    Console.WriteLine("        |  \\| |  _|  \\ \\ /\\ / /  | |  _  / _ \\ | |\\/| |  _|          ");
                    Console.WriteLine("        | |\\  | |___  \\ V  V /   | |_| |/ ___ \\| |  | | |___         ");
                    Console.WriteLine("        |_| \\_|_____|  \\_/\\_/     \\____/_/   \\_\\_|  |_|_____|        ");
                    Console.WriteLine("");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("----------------------------[클래스 선택]----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Battle:
                    Console.Clear();
                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("            '||'''|,            ||      ||    '||`                   ");
                    Console.WriteLine("             ||   ||            ||      ||     ||                    ");
                    Console.WriteLine("             ||;;;;    '''|.  ''||''  ''||''   ||  .|''|,            ");
                    Console.WriteLine("             ||   ||  .|''||    ||      ||     ||  ||..||            ");
                    Console.WriteLine("            .||...|'  `|..||.   `|..'   `|..' .||. `|...             ");
                    Console.WriteLine("                                                                     ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-------------------------------[전 투]-------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");

                    break;

                case AsciiPreset.Status:
                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("              _______  __           __                               ");
                    Console.WriteLine("             |     __||  |_ .---.-.|  |_ .--.--..-----.              ");
                    Console.WriteLine("             |__     ||   _||  _  ||   _||  |  ||__ --|              ");
                    Console.WriteLine("             |_______||____||___._||____||_____||_____|              ");
                    Console.WriteLine("                                                                     ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("------------------------------[상태 창]------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Inventory:
                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("    _______                              __                          ");
                    Console.WriteLine("   |_     _|.-----..--.--..-----..-----.|  |_ .-----..----..--.--.   ");
                    Console.WriteLine("    _|   |_ |     ||  |  ||  -__||     ||   _||  _  ||   _||  |  |   ");
                    Console.WriteLine("   |_______||__|__| \\___/ |_____||__|__||____||_____||__|  |___  |   ");
                    Console.WriteLine("                                                           |_____|   ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("------------------------------[인벤토리]-----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;


                case AsciiPreset.PotionInventory:
                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("    .-.   p--- .'~`. .-=~=-.   :~:      |~|   .-~~8~~-.  |~|  .-.    ");
                    Console.WriteLine("  .'__( .'~`.  `. .'  )___(  .'   `.    | |   |~~---~~|  | |  )__`.  ");
                    Console.WriteLine("  | l | | m |  .'n`. (  o  ) |  p  |] .' q `. |   r   | .'s`. | t |  ");
                    Console.WriteLine("  |___| |___|  `._.'  `._.'  |_____|  `.___.' `._____.' `._.' |___|  ");
                    Console.WriteLine("                                                                     ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("----------------------------[회복 아이템]----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Shop:
                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("                     _______  __                                     ");
                    Console.WriteLine("                    |     __||  |--..-----..-----.                   ");
                    Console.WriteLine("                    |__     ||     ||  _  ||  _  |                   ");
                    Console.WriteLine("                    |_______||__|__||_____||   __|                   ");
                    Console.WriteLine("                                           |__|                      ");
                    Console.WriteLine("                                                                     ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("------------------------------[상   점]------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.DungeonEntrance:

                    Console.WriteLine("                                                                     ");
                    Console.WriteLine("           _____                                                     ");
                    Console.WriteLine("          |     \\ .--.--..-----..-----..-----..-----..-----.         ");
                    Console.WriteLine("          |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |         ");
                    Console.WriteLine("          |_____/ |_____||__|__||___  ||_____||_____||__|__|         ");
                    Console.WriteLine("                                |_____|                              ");
                    Console.WriteLine("                                                                     ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-----------------------------[던전 입구]-----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Inn:

                    Console.WriteLine("");
                    Console.WriteLine("                           __                                        ");
                    Console.WriteLine("                          |__|.-----..-----.                         ");
                    Console.WriteLine("                          |  ||     ||     |                         ");
                    Console.WriteLine("                          |__||__|__||__|__|                         ");
                    Console.WriteLine("");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-------------------------------[여 관]-------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");

                    break;

                default:
                    Console.WriteLine( "아스키아트 프리셋이 없습니다.");
                    break;

            }
        }
        #endregion

        public static void ColoredWriteLine(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static void ColoredWrite(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ResetColor();
        }
        public static void RandomColoredWrite(string s)
        {
            Random random = new Random();
            foreach (char c in s)
            {
                Console.ForegroundColor = (ConsoleColor)random.Next(9, 16);
                Console.Write(c);
            }
            Console.ResetColor();
        }
    }
}
