using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
            MainMenu2,
            Battle,
            Status,
            PotionInventory,
            Inventory,
            Shop,
            DungeonEntrance,
            DungeonEntrance2,
            DungeonEntrance3,
            Inn,
            DungeonVictory,
            DungeonFail,

        }

        // volatile: 멀티스레드 환경에서 변수의 값이 예기치 않게 변경될 수 있음을 컴파일러와 실행 환경에 알리는 데 사용
        //           여러 스레드에 의해 동시에 접근될 수 있고, 이러한 변수의 값을 읽고 쓸 때, 메모리에서 직접 읽거나 쓰도록 강제
        // 스레드를 사용하여 파티클 이펙트의 갱신을 하도록 하고, 메인 스레드에서는 키 입력을 받을 수 있도록 함.
        static volatile bool keepRunning = true; // 키 입력을 받으면 keepRunning을 false로 만들어, 스레드의 종료 요청을 보냄.
        static char[,] screenBuffer;

        /// <summary>
        /// 아스키아트를 콘솔에 표시합니다.
        /// </summary>
        /// <param name="preset">표시할 아스키아트의 프리셋을 고릅니다. </param>
        public static void AsciiArt(AsciiPreset preset)
        {
            // https://www.asciiart.eu/mythology/centaurs
            ConsoleColor statusColor = ConsoleColor.Gray;

            int x, y;

            switch (preset)
            {
                case AsciiPreset.TitleArt:
                    string titleArt =
                        "                                                                                               \n" +
                        "                                                                                               \n" +
                        "                                      -========-                                               \n" +
                        "                                      %%++++*#@%                                               \n" +
                        "                                      %#++++*#@%                                               \n" +
                        "                                      %%######@%                                               \n" +
                        "                                      %%#%%%%#@%                                               \n" +
                        "                                      %@@%%%@@@%                                               \n" +
                        "                                       .@##%@@                                                 \n" +
                        "                                   %@@@@@##%@@@@@@@                                            \n" +
                        "                                .%%########%%%%%%%%%*.                                         \n" +
                        "                              *#%####*###%%%%%%%####%%*+                                       \n" +
                        "                            +######+*###############%%%@#=                                     \n" +
                        "                            %@#####################%%%%%@+                                     \n" +
                        "                            %@###################%%%%%%%@+                                     \n" +
                        "                            %@###%%@@@@%########@@@@@%%%@+                                     \n" +
                        "                            %@###%#....-%#####%#....-%%%@+                                     \n" +
                        "                            %@###%#:+%%*:*####::%%%:=%%#@*                                     \n" +
                        "                         *##@@###%%#-..-#**%%%#*...*#%%%@@##*                                  \n" +
                        "                       +@:..%@%##%#+:....====+.....=+%%%@#..:@-                                \n" +
                        "                       +@:..%@%%%%%%#*...........-*%%%%%@#..:@-                                \n" +
                        "                         %*..-@@%%%%%#%+........#%%%%%@@.:.*%                                  \n" +
                        "                          :@:..*@%%%%#%+........#%%%@@:..-@.                                   \n" +
                        "                            %%...%@@@@@*.......:@@@@#...@@@.                                   \n" +
                        "                            %@@=..:###++++++++*###*...#@##@.                                   \n" +
                        "                            %@#%@:....**+++++####-..-@####@.                                   \n" +
                        "                            %@###@%.................-@%###@.                                   \n" +
                        "                            %@###@#.................-@%###@.             [아무 키나 눌러 ]            \n" +
                        "                            %@###@#.................-@%###@.             [게임을 시작하기]            \n" +
                        "                            %@###@#.................-@%###@.                                   \n" +
                        "                            %@###@%===========******#@%##%@.                                   \n" +
                        "                            #@###@%+++##############%@%##%@.                                   \n" +
                        "                                                                                               \n" +
                        "===============================================================================================\n" +
                        "          ____                   _          ____                                               \n" +
                        "         / ___| _ __   __ _ _ __| |_ __ _  |  _ \\ _   _ _ __   __ _  ___  ___  _ __            \n" +
                        "         \\___ \\| '_ \\ / _` | '__| __/ _` | | | | | | | | '_ \\ / _` |/ _ \\/ _ \\| '_ \\           \n" +
                        "          ___) | |_) | (_| | |  | || (_| | | |_| | |_| | | | | (_| |  __/ (_) | | | |          \n" +
                        "         |____/| .__/ \\__,_|_|   \\__\\__,_| |____/ \\__,_|_| |_|\\__, |\\___|\\___/|_| |_|          \n" +
                        "               |_|                                            |___/                            \n" +
                        "                                                                                               \n" +
                        "===============================================================================================";
                    InitializeScreenBuffer(titleArt); // 위 내용을 저장 해 둔다
                    Console.WriteLine(titleArt);

                    int xEnd = 95; int yEnd = 41; int particleQuantity = 15; int delay = 800;
                    Thread effectThread;
                    effectThread = new Thread(() => UpdateEffect((0,0), (xEnd, yEnd), '+', particleQuantity, delay, useSavedBuffer:true)); effectThread.Start();

                    Console.ReadKey();
                    keepRunning = false; // 스레드 내 종료 조건을 활성화
                    effectThread.Join(); // 스레드가 종료될 때까지 대기
                    break;

                case AsciiPreset.MainMenu:
                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("               _   ____  ____   _    ____ _____  _      _              ");
                    Console.WriteLine("              | | / ___||  _ \\ / \\  |  _ \\_   _|/ \\    | |             ");
                    Console.WriteLine("              | | \\___ \\| |_) / _ \\ | |_) || | / _ \\   | |             ");
                    Console.WriteLine("              |_|  ___) |  __/ ___ \\|  _ < | |/ ___ \\  |_|             ");
                    Console.WriteLine("              (_) |____/|_| /_/   \\_\\_| \\_\\|_/_/   \\_\\ (_)             ");
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("=======================================================================");
                    Console.Write("              └ [ ");
                    RandomColoredWrite("스파르타 던전에 오신것을 환영합니다");// 랜덤 색으로 문자열 표현
                    Console.WriteLine(" ] ┘              ");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    break;

                case AsciiPreset.MainMenu2:
                    x = 36; y = 13;
                    DrawOnSpecificPos("" +
                        "~         ~~          __\n" +
                        "       _T      .,,.    ~--~ ^^\n" +
                        " ^^   // \\                    ~\n" +
                        "      ][O]    ^^      ,-~ ~\n" +
                        "   /''-I_I         _II____\n" +
                        "__/_  /   \\ ______/ ''   /'\\_,__\n" +
                        "  | II--'''' \\,--:--..,_/,.-{ },\n" +
                        "; '/__\\,.--';|   |[] .-.| O{ _ }\n" +
                        ":' |  | []  -|   ''--:.;[,.'\\,/\n" +
                        "'  |[]|,.--'' '',   ''-,.    |\n" +
                        "  ..    ..-''    ;       ''. '",
                        (x, y));
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
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("             '||'''|,            ||      ||    '||`                    ");
                    Console.WriteLine("              ||   ||            ||      ||     ||                     ");
                    Console.WriteLine("              ||;;;;    '''|.  ''||''  ''||''   ||  .|''|,             ");
                    Console.WriteLine("              ||   ||  .|''||    ||      ||     ||  ||..||             ");
                    Console.WriteLine("             .||...|'  `|..||.   `|..'   `|..' .||. `|...              ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("--------------------------------[전 투]--------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Status:
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("               _______  __           __                                ");
                    Console.WriteLine("              |     __||  |_ .---.-.|  |_ .--.--..-----.               ");
                    Console.WriteLine("              |__     ||   _||  _  ||   _||  |  ||__ --|               ");
                    Console.WriteLine("              |_______||____||___._||____||_____||_____|               ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-------------------------------[상태 창]-------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Inventory:
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("     _______                              __                           ");
                    Console.WriteLine("    |_     _|.-----..--.--..-----..-----.|  |_ .-----..----..--.--.    ");
                    Console.WriteLine("     _|   |_ |     ||  |  ||  -__||     ||   _||  _  ||   _||  |  |    ");
                    Console.WriteLine("    |_______||__|__| \\___/ |_____||__|__||____||_____||__|  |___  |    ");
                    Console.WriteLine("                                                            |_____|    ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-------------------------------[인벤토리]------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;


                case AsciiPreset.PotionInventory:
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("     .-.   p--- .'~`. .-=~=-.   :~:      |~|   .-~~8~~-.  |~|  .-.     ");
                    Console.WriteLine("   .'__( .'~`.  `. .'  )___(  .'   `.    | |   |~~---~~|  | |  )__`.   ");
                    Console.WriteLine("   | l | | m |  .'n`. (  o  ) |  p  |] .' q `. |   r   | .'s`. | t |   ");
                    Console.WriteLine("   |___| |___|  `._.'  `._.'  |_____|  `.___.' `._____.' `._.' |___|   ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-----------------------------[회복 아이템]-----------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.Shop:
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("                      _______  __                                      ");
                    Console.WriteLine("                     |     __||  |--..-----..-----.                    ");
                    Console.WriteLine("                     |__     ||     ||  _  ||  _  |                    ");
                    Console.WriteLine("                     |_______||__|__||_____||   __|                    ");
                    Console.WriteLine("                                            |__|                       ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("-------------------------------[상   점]-------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.DungeonEntrance:

                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("            _____                                                      ");
                    Console.WriteLine("           |     \\ .--.--..-----..-----..-----..-----..-----.          ");
                    Console.WriteLine("           |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |          ");
                    Console.WriteLine("           |_____/ |_____||__|__||___  ||_____||_____||__|__|          ");
                    Console.WriteLine("                                 |_____|                               ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("------------------------------[던전 입구]------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("");
                    break;

                case AsciiPreset.DungeonEntrance2:
                    x = 40; y = 13;
                    DrawOnSpecificPos("" +
                        "        ______\n" +
                        "       /     /\\\n" +
                        "      /     /  \\\n" +
                        "     /_____/----\\_    (  \n" +
                        "    \"     \"          ).  \n" +
                        "   _ ___          o (:') o   \n" +
                        "  (@))_))        o ~/~~\\~ o   \n" +
                        "                  o  o  o",
                        (x, y));
                    break;

                case AsciiPreset.DungeonEntrance3:
                    x = 41; y = 25;
                    DrawOnSpecificPos("" +
                        "      |\\      _,,,---,,_\n" +
                        "ZZZzz /,`.-'`'    -.  ;-;;,_\n" +
                        "     |,4-  ) )-,_. ,\\ (  `'-'\n" +
                        "    '---''(_/--'  `-'\\_)\n",
                        (x, y));
                    break;

                case AsciiPreset.DungeonVictory:
                    x = 42; y = 13;
                    DrawOnSpecificPos("" +
                        "   |\\                     /)\n" +
                        " /\\_\\\\__               (_//\n" +
                        "|   `>\\-`     _._       //`)\n" +
                        " \\ /` \\\\  _.-`:::`-._  //\n" +
                        "  `    \\|`    :::    `|/\n" +
                        "        |     :::     |\n" +
                        "        |.....:::.....|\n" +
                        "        |:::::::::::::|\n" +
                        "        |     :::     |\n" +
                        "        \\     :::     /\n" +
                        "         \\    :::    /\n" +
                        "          `-. ::: .-'\n" +
                        "           //`:::`\\\\\n" +
                        "          //   '   \\\\\n" +
                        "         |/         \\\\",
                        (x, y));
                    break;

                case AsciiPreset.DungeonFail:
                    x = 45; y = 15;
                    DrawOnSpecificPos("" +
                        "  <=======]}======\n" +
                        "    --.   /|\n" +
                        "   _\\' / _.'/\n"+
                        " .'._._,.'\n" +
                        " :/ \\{}/\n" +
                        "(L  /--',----._\n" +
                        "    |          \\\\\n" +
                        "   : /-\\ .'-'\\ / |\n" +
                        "    \\\\, ||    \\|\n" +
                        "     \\/ ||    ||",
                        (x, y));
                    break;

                case AsciiPreset.Inn:
                    Console.WriteLine("                                                                       ");
                    Console.WriteLine("                            __                                         ");
                    Console.WriteLine("                           |__|.-----..-----.                          ");
                    Console.WriteLine("                           |  ||     ||     |                          ");
                    Console.WriteLine("                           |__||__|__||__|__|                          ");
                    Console.WriteLine("                                                                       ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine("--------------------------------[여 관]--------------------------------");
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

        public static void DrawOnSpecificPos(string s, (int x, int y) startPosition)
        {
            // 커서 위치를 저장해두어, Draw가 끝나면 커서 위치를 원상복귀
            int xSave = Console.CursorLeft;
            int ySave = Console.CursorTop;

            int x = startPosition.x;
            int y = startPosition.y;

            string[] lines = s.Split('\n');
            foreach (string line in lines)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(line);
                y++;
            }
            Console.SetCursorPosition(xSave, ySave);
        }

        // !!! 이 아래는 정말 마이너한 용도 !!!
        // 1. InitializeScreenBuffer() - 장면 내용 보관(콘솔 내용을 끌어오는 것은 불가능. 직접 문자열을 전달해야함)
        // 2. UpdateEffect() - 특정 직사각형 범위에 딜레이마다 파티클을 표시
        // 3. IsFullWidth() - 문자가 전각인지 확인
        static void InitializeScreenBuffer(string s, int top = 0, int left = 0) // 복원이 필요한 내용을 버퍼에 저장하기
        {
            // 콘솔 크기만큼 저장공간을 할당. 복원 목적이라면 콘솔크기 밖을 벗어나는 일은 없음.
            // 저장할 내용이 폭과 높이를 벗어나지 않도록 주의.
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            screenBuffer = new char[height, width]; // screenBuffer는 메서드 밖에 전역변수로 선언을 미리 해두었음

            // 내용 s를 screenBuffer에 저장
            using (var reader = new StringReader(s))
            {
                string line;
                int y = top;
                while ((line = reader.ReadLine()) != null)
                {
                    int bufferX = 0; // 버퍼 내의 X 좌표 추적
                    for (int x = left; x < line.Length; x++)
                    {
                        char currentChar = line[x];
                        screenBuffer[y, bufferX] = currentChar;

                        if (IsFullWidth(currentChar)) // 전각 문자인 경우 다음 칸을 비워둠
                        {
                            bufferX++;
                        }

                        bufferX++;
                    }
                    y++;
                }
            }
        }

        static void UpdateEffect( (int x, int y) startPosition, (int x, int y) endPosition, char c, int particleQuantity, int delayMs, bool useSavedBuffer=false)
        {   // 직사각형범위 시작(좌상)좌표, 직사각형범위 끝(우하)좌표, 파티클문자, 파티클갯수, 딜레이(밀리세컨드)
            Random random = new Random();
            List<Tuple<int, int>> positions = new List<Tuple<int, int>>();

            while (keepRunning)
            {
                // 파티클 위치 선정
                for (int i = 0; i < particleQuantity; i++)
                {
                    int x = random.Next(startPosition.x, endPosition.x);
                    int y = random.Next(startPosition.y, endPosition.y);
                    positions.Add(Tuple.Create(x, y));
                }

                // 파티클 문자 그리기
                foreach (var pos in positions)
                {
                    if (pos.Item1 > 0 && IsFullWidth(screenBuffer[pos.Item2, pos.Item1 - 1]))
                    {
                        // 현재 위치가 전각 문자의 두 번째 칸이면 x위치를 하나 줄여서 공백과 'c'를 함께 출력
                        Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
                        Console.Write($" {c}");
                    }
                    else
                    {
                        Console.SetCursorPosition(pos.Item1, pos.Item2);
                        if (IsFullWidth(screenBuffer[pos.Item2, pos.Item1]))
                        {
                            // 현재 위치가 전각 문자의 첫 번째 칸이면 'c'와 공백을 함께 출력
                            Console.Write($"{c} ");
                        }
                        else
                        {
                            // 그렇지 않으면 그냥 'c'만 출력
                            Console.Write(c);
                        }
                    }
                }
                Console.SetCursorPosition(0, 42);

                // {delayMs}ms 대기
                Thread.Sleep(delayMs);

                // 이전에 그려진 'c' 문자들을 지움
                foreach (var pos in positions)
                {
                    Console.SetCursorPosition(pos.Item1, pos.Item2);
                    if (useSavedBuffer)
                    {
                        try
                        {
                            char charToRestore = screenBuffer[pos.Item2, pos.Item1];
                            if (charToRestore == '\0' && pos.Item1 > 0) // 현재 위치가 비어있는 경우, 이전 위치 확인
                            {
                                char prevChar = screenBuffer[pos.Item2, pos.Item1 - 1];
                                if (IsFullWidth(prevChar)) // 이전 문자가 전각 문자인 경우
                                {
                                    Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
                                    Console.Write(prevChar); // 전각 문자 복원
                                    continue;
                                }
                            }
                            Console.Write(charToRestore);
                        }
                        catch
                        {
                            Console.Write(" ");
                        }
                    }
                    else Console.Write(" ");
                }
                // 리스트 비우기
                positions.Clear();

                Console.SetCursorPosition(0, 42);
            }
        }
        static bool IsFullWidth(char c) // 전각문자인지 판별
        {
            int[] ranges = { 
                0x1100, 0x115F, // 한글 자음과 모음
                0x2E80, 0x2EFF, // CJK(중국, 일본, 한국) 급진 부호
                0x3000, 0x303F, // CJK 심볼 및 구두점
                0x3200, 0x32FF, // CJK 호환성 음절
                0x3400, 0x4DBF, // CJK 통합 한자 확장
                0x4E00, 0x9FFF, // CJK 통합 한자
                0xAC00, 0xD7AF, // 한글 음절
                0xF900, 0xFAFF, // CJK 호환성 한자
                0xFE30, 0xFE4F, // CJK 호환성 형태
                0xFF01, 0xFFEF, // 하프와이드 및 풀와이드 형태
            };
            int code = (int)c;

            for (int i = 0; i < ranges.Length; i += 2)
            {
                if (code >= ranges[i] && code <= ranges[i + 1])
                {
                    return true; // 전각
                }
            }
            return false; // 반각
        }
    }
}
