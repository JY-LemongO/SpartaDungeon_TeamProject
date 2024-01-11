using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RtanTextDungeon.Define;

namespace RtanTextDungeon
{    
    internal class Dungeon
    {
        // 인게임에서 사용될 Player
        private Player player = null;
        // 인게임에서 상점 이용에 쓰일 아이템 목록을 가지고 있는 Shop 필드
        private Shop shop = null;


        #region 게임시작
        public void EnterGame(Shop shop)
        {
            if (this.player == null)
                CharacterCreation();
            if (this.shop == null)
                this.shop = shop;

            
            
            while (true)
            {
                Console.WriteLine("===============================================================================================");
                Console.WriteLine(" _______                      __            _____                                              ");
                Console.WriteLine("|     __|.-----..---.-..----.|  |_ .---.-. |     \\ .--.--..-----..-----..-----..-----..-----. ");
                Console.WriteLine("|__     ||  _  ||  _  ||   _||   _||  _  | |  --  ||  |  ||     ||  _  ||  -__||  _  ||     |  ");
                Console.WriteLine("|_______||   __||___._||__|  |____||___._| |_____/ |_____||__|__||___  ||_____||_____||__|__|  ");
                Console.WriteLine("         |__|                                                    |_____|                       ");
                Console.WriteLine("===============================================================================================\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("※※※스파르타 던전에 온 것을 환영합니다.※※※\n\n");
                Console.ResetColor();

                Console.WriteLine("-------------------------------------------\n");
                Console.WriteLine("(E) : [상태]\n\n(I) : [인벤토리]\n\n(S) : [상점]\n\n(D) : [던전입장]\n\n(P) : [회복아이템]\n\n(R) : [휴식]\n\n(X) : [게임종료]\n");
                Console.WriteLine("-------------------------------------------\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "E":
                    case "e":                        
                        Status();
                        break;
                    case "I":
                    case "i":
                        Inventory();
                        break;
                    case "S":
                    case "s":
                        Shop();
                        break;
                    case "D":
                    case "d":
                        DungeonEntrance();
                        break;
                    case "R":
                    case "r":
                        Rest();
                        break;
                    case "P":
                    case "p":
                        PotionInventory();
                        break;
                    case "X":
                    case "x":
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("※※※게임을 종료합니다※※※");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!!잘못된 입력입니다!!!");
                        Console.ResetColor();
                        break;
                }
            }
        }
        #endregion

        #region 캐릭터 생성
        public void CharacterCreation()
        {
            string name = string.Empty;
            string input = string.Empty;

            while(true)
            {
                Console.Clear();
                Console.WriteLine("===== 캐릭터 생성 =====");
                Console.WriteLine("이름을 입력하십시오.");

                name = Console.ReadLine();

                if (name == "")
                {
                    Console.WriteLine("입력 값이 없습니다.");
                    Console.ReadLine();
                    continue;
                }

                Console.Clear();
                Console.WriteLine("===== 캐릭터 생성 =====");
                Console.WriteLine($"입력하신 이름은 {name} 입니다.");

                Console.WriteLine("1. 저장");
                Console.WriteLine("2. 취소");

                Console.Write("");
                input = Console.ReadLine();

                switch(input)
                {
                    case "1":
                        break;
                    case "2":
                        continue;
                    default:
                        continue;
                }

                Console.Clear();
                Console.WriteLine("===== 캐릭터 생성 =====");
                Console.WriteLine($"이름 : {name}");
                Console.ReadLine();
                Console.Clear();

                // 직업 선택 영역 구현
                Console.Clear();
                Console.WriteLine("===== 캐릭터 직업 선택 =====");
                Console.WriteLine("1. 전사 - 공격력: 10, 방어력: 5, 체력: 100, Max체력: 100, GOLD: 1500");
                Console.WriteLine("2. 궁수 - 공격력: 13, 방어력: 3, 체력: 70, Max체력: 70, GOLD: 2500");
                Console.WriteLine("3. 마법사 - 공격력: 3, 방어력: 13, 체력: 80, Max체력: 80, GOLD: 3500");
                Console.WriteLine("4. 도둑 - 공격력: 2, 방어력: 3, 체력: 120, Max체력: 120, GOLD: 5500");

                Console.Write("");
                input = Console.ReadLine();

                switch(input)
                {
                    case "1":
                        player = new Player(1, name, PlayerClass.Worrior, 10, 5, 100, 100, 1500);
                        break;
                    case "2":
                        player = new Player(1, name, PlayerClass.Archer, 13, 3, 70, 70, 2500);
                        break;
                    case "3":
                        player = new Player(1, name, PlayerClass.Magic, 3, 13, 80, 80, 3500);
                        break;
                    case "4":
                        player = new Player(1, name, PlayerClass.Thief, 2, 3, 120, 120, 5500);
                        break;
                    default:
                        continue;
                }

                Console.Clear(); // 콘솔 화면 한 번 지우기

                break;
            }
        }
        #endregion


        #region 상태창
        private void Status()
        {
            string weaponStatus = player.equippedItems.ContainsKey(typeof(Weapon)) ? $"{player.equippedItems[typeof(Weapon)].AdditionalATK}" : "";
            string armorStatus  = player.equippedItems.ContainsKey(typeof(Armor)) ? player.equippedItems[typeof(Armor)].AdditionalDEF : "";
            string amuletATK    = player.equippedItems.ContainsKey(typeof(Amulet)) ? player.equippedItems[typeof(Amulet)].AdditionalATK : "";
            string amuletDEF    = player.equippedItems.ContainsKey(typeof(Amulet)) ? player.equippedItems[typeof(Amulet)].AdditionalDEF : "";

            while (true)
            {
                Console.WriteLine(" _______  __           __                 ");
                Console.WriteLine("|     __||  |_ .---.-.|  |_ .--.--..-----.");
                Console.WriteLine("|__     ||   _||  _  ||   _||  |  ||__ --|");
                Console.WriteLine("|_______||____||___._||____||_____||_____|");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=================[상태 창]=================");
                Console.ResetColor();

                Console.WriteLine("-------------------------------------------\n");
                Console.WriteLine($"Lv. {player.Lv.ToString("00")}\n" +
                                $"이름\t:  {player.Name}({player.m_Class})\n\n" +
                                $"공격력\t:  {player.Atk} {weaponStatus + amuletATK}\n" +
                                $"방어력\t:  {player.Def} {armorStatus + amuletDEF}\n" +
                                $"체 력\t:  {player.Hp}\n" +
                                $"Gold\t:  {player.Gold:N0} G\n");
                Console.WriteLine("-------------------------------------------\n");

                Console.WriteLine("(I) : [인벤토리]\n\n(S) : [상점]\n\n(B) : [마을로 돌아가기]\n\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "I":
                    case "i":                        
                        Inventory();
                        return;
                    case "S":
                    case "s":                        
                        Shop();
                        return;
                    case "B":
                    case "b":                        
                        return;
                    default:                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!!잘못된 입력입니다!!!");
                        Console.ResetColor();
                        break;
                }
            }
        }
        #endregion

        #region 인벤토리
        private void Inventory()
        {
            while (true)
            {
                Console.WriteLine(" _______                              __                       ");
                Console.WriteLine("|_     _|.-----..--.--..-----..-----.|  |_ .-----..----..--.--.");
                Console.WriteLine(" _|   |_ |     ||  |  ||  -__||     ||   _||  _  ||   _||  |  |");
                Console.WriteLine("|_______||__|__| \\___/ |_____||__|__||____||_____||__|  |___  |");
                Console.WriteLine("                                                        |_____|");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("============================[인벤토리]============================");
                Console.ResetColor();

                Console.WriteLine("------------------------------------------------------------------\n");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("[아이템 목록]\n");
                Console.ResetColor();
                // 아이템 목록은 아이템 리스트에 있는 아이템들을 전부 불러와야겠지?

                int index = 1;
                if (player.items.Count == 0)
                    Console.WriteLine($"[비어있음]");
                foreach (Item item in player.items)
                {
                    if (item.IsEquip)
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    if (item is Weapon weapon)
                        Console.WriteLine($"- ({index})  {weapon.AbilityName} : +{weapon.damage}\t| {weapon.Name}\t| {weapon.Desc}");
                    else if (item is Armor armor)
                        Console.WriteLine($"- ({index})  {armor.AbilityName} : +{armor.defense}\t| {armor.Name}\t| {armor.Desc}");
                    else if (item is Amulet amulet)
                        Console.WriteLine($"- ({index})  {amulet.AbilityName} : +{amulet.damage} / +{amulet.defense}\t| {amulet.Name}\t| {amulet.Desc}");
                    Console.ResetColor();
                    index++;
                }
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------------\n");

                Console.WriteLine("(E) : [상태]\n\n(S) : [상점]\n\n(B) : [마을로 돌아가기]\n\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                int itemIndex;
                switch (input)
                {
                    case "E":
                    case "e":                        
                        Status();
                        return;
                    case "S":
                    case "s":
                        Shop();
                        return;
                    case "B":
                    case "b":
                        return;
                    default:
                        if (int.TryParse(input, out itemIndex) && itemIndex <= player.items.Count && itemIndex > 0)
                        {
                            itemIndex--;
                            if (player.items[itemIndex] is Weapon weapon)
                                player.EquipOrUnequipItem(weapon);
                            else if (player.items[itemIndex] is Armor armor)
                                player.EquipOrUnequipItem(armor);
                            else if (player.items[itemIndex] is Amulet amulet)
                                player.EquipOrUnequipItem(amulet);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("!!!잘못된 입력입니다!!!");
                            Console.ResetColor();
                        }
                        break;
                }
            }
        }
        #endregion

        #region 상점
        private void Shop()
        {
            while (true)
            {
                Console.WriteLine(" _______  __                  ");
                Console.WriteLine("|     __||  |--..-----..-----.");
                Console.WriteLine("|__     ||     ||  _  ||  _  |");
                Console.WriteLine("|_______||__|__||_____||   __|");
                Console.WriteLine("                       |__|   ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("============[상   점]============");
                Console.ResetColor();
                Console.WriteLine("---------------------------------\n");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("[보유 골드]");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{player.Gold:N0} G\n");
                Console.ResetColor();

                Console.WriteLine("[아이템 목록]\t[능력]\t\t\t\t[가격]\t\t[이름]\t\t[설명]\n");
                int index = 1;
                foreach (Item item in shop.items)
                {
                    if (item.IsBuy)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"- ({index})\t\t{item.Name}\t| 구매완료 - 판매가격 : {(int)(item.Price * 0.85f)} G\n");
                        Console.ResetColor();
                    }
                    else if (item is Weapon weapon)
                        Console.WriteLine($"- ({index})\t\t{weapon.AbilityName} : +{weapon.damage}\t\t\t| {weapon.Price:N0} G\t| {weapon.Name}\t| {weapon.Desc}\n");
                    else if (item is Armor armor)
                        Console.WriteLine($"- ({index})\t\t{armor.AbilityName} : +{armor.defense}\t\t\t| {armor.Price:N0} G\t| {armor.Name}\t| {armor.Desc}\n");
                    else if (item is Amulet amulet)
                        Console.WriteLine($"- ({index})\t\t{amulet.AbilityName} : +{amulet.damage} / +{amulet.defense}\t| {amulet.Price:N0} G\t| {amulet.Name}\t| {amulet.Desc}\n");
                    index++;
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("아이템 번호를 입력하시면 구매/판매가 가능합니다.\n");
                Console.ResetColor();

                Console.WriteLine("---------------------------------\n");

                Console.WriteLine("(E) : [상태]\n\n(I) : [인벤토리]\n\n(B) : [마을로 돌아가기]\n\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                int itemIndex;
                switch (input)
                {
                    case "E":
                    case "e":
                        Status();
                        return;
                    case "I":
                    case "i":
                        Inventory();
                        return;
                    case "B":
                    case "b":
                        return;
                    default:
                        if (int.TryParse(input, out itemIndex) && itemIndex <= shop.items.Length && itemIndex > 0)
                        {
                            itemIndex--;
                            if (!shop.items[itemIndex].IsBuy)
                                shop.Buy(player, shop.items[itemIndex]);
                            else
                                shop.Sell(player, shop.items[itemIndex]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("!!!잘못된 입력입니다!!!");
                            Console.ResetColor();
                        }
                        break;
                }
            }
        }
        #endregion

        #region 던전입구
        private void DungeonEntrance()
        {            
            bool status = false;
            bool hpZero = false;
            int startHp = player.Hp;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(" _____                                            ");
                Console.WriteLine("|     \\ .--.--..-----..-----..-----..-----..-----.");
                Console.WriteLine("|  --  ||  |  ||     ||  _  ||  -__||  _  ||     |");
                Console.WriteLine("|_____/ |_____||__|__||___  ||_____||_____||__|__|");
                Console.WriteLine("                      |_____|                     ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("====================[던전 입구]====================");
                Console.ResetColor();
                Console.WriteLine("---------------------------------\n");

                Console.WriteLine("=================================\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (!status)
                    Console.WriteLine("(E) : ▶ 내 정보\n");
                else
                {
                    Console.WriteLine("(E) : ▼ 내 정보");
                    Console.WriteLine($"{player.Name} Lv. {player.Lv.ToString("00")}\n\n" +
                                $"공격력\t:  {player.Atk}\n" +
                                $"방어력\t:  {player.Def}\n" +
                                $"체 력\t:  {player.Hp}\n" +
                                $"Gold\t:  {player.Gold:N0} G\n");
                }
                Console.ResetColor();
                Console.WriteLine("=================================\n");

                Console.WriteLine("" +
                    "(1) 전투 시작\n" +
                    "(B) 마을로 돌아가기\n");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                if (hpZero)
                Console.WriteLine("체력이 없습니다. 여관에서 휴식을 취하세요.\n");
                Console.ResetColor();

                Console.WriteLine("---------------------------------");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "1":                    
                        if (player.Hp > 0)
                            EnterDungeon(startHp);
                        else
                            hpZero = true;
                        break;                        
                    case "E":
                    case "e":
                        status = !status;
                        break;
                    case "B":
                    case "b":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!!잘못된 입력입니다!!!");
                        Console.ResetColor();
                        break;
                }
            }
        }
        #endregion

        #region 배틀
        private void EnterDungeon(int startHp)
        {            
            #region 몬스터 스폰
            int spawnCount = new Random().Next(1, 5);
            Monster[] monsters = new Monster[spawnCount];
            for (int i = 0; i < monsters.Length; i++)
            {
                int randLv = new Random().Next(1, 6);
                int randomType = new Random().Next(0, Enum.GetValues(typeof(MonsterType)).Length);

                monsters[i] = new Monster(randLv, (MonsterType)randomType);                
            }
            #endregion
            bool invalid = false;
            bool isSkillShow = false;

            while (true)
            {
                BattlePrint();

                for (int i = 0; i < monsters.Length; i++)
                    monsters[i].ShowText();

                Console.WriteLine($"\n" +
                    $"[내정보]\n" +
                    $"Lv.{player.Lv}\t{player.Name}\n" +
                    $"HP {player.Hp}/{player.MaxHp}\n\n");
                
                if(!isSkillShow)
                {
                    Console.WriteLine("1. 공격\n");
                    Console.WriteLine("2. 스킬\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n");

                    if (invalid)
                        Console.WriteLine("잘못된 입력입니다.");

                    string input = Console.ReadLine();
                    Console.Clear();
                    switch (input)
                    {
                        case "1":
                            Fight(monsters, startHp);
                            invalid = false;
                            break;
                        case "2":
                            isSkillShow = true; //스킬 선택 화면으로
                            break;
                        default:
                            invalid = true;
                            continue;
                    }
                }
                else // 스킬 선택 화면
                {

                    for(int i = 0; i < player.Skills.Count; ++i)
                    {
                        player.Skills[i].ShowText();
                    }
                    Console.WriteLine("0. 취소\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n");
                    if (invalid)
                        Console.WriteLine("잘못된 입력입니다.");

                    string input = Console.ReadLine();
                    Console.Clear();
                    switch (input)
                    {
                        case "0":
                            isSkillShow = false; //공격, 스킬 선택 화면으로
                            continue;
                        default:
                            int skillNum;
                            //입력 값이 (숫자 and 1 이상 and 스킬 개수 이하) 인 경우
                            if (int.TryParse(input,out skillNum) && skillNum >= 1 && (skillNum - 1) < player.Skills.Count)
                            {
                                // 여기부터 개발해야함.
                            }
                            else
                                invalid = true;
                            continue;
                    }
                }

                if (monsters.All(x => x.IsDead) || player.Hp <= 0)
                    return;
            }                   
        }

        private void Fight(Monster[] monsters, int startHp)
        {            
            bool invalid = false;                      

            while (true)
            {
                BattlePrint();

                for (int i = 0; i < monsters.Length; i++)
                    monsters[i].ShowText(i + 1);


                // 전투 종료
                if (monsters.All(x => x.IsDead))
                {
                    Victory(monsters.Length, startHp, monsters);   
                    return;
                }                        
                else if (player.Hp <= 0)
                {
                    Lose(startHp);
                    return;
                }

                Console.WriteLine($"\n" +
                    $"[내정보]\n" +
                    $"Lv.{player.Lv}\t{player.Name}\n" +
                    $"HP {player.Hp}/{player.MaxHp}\n\n");

                Console.WriteLine("0. 취소\n");
                Console.WriteLine("대상을 선택해주세요.\n");

                if (invalid)
                    Console.WriteLine("잘못된 입력입니다.");

                string input = Console.ReadLine();
                int inputNum;
                bool isNum = int.TryParse(input, out inputNum);
                if (!isNum)
                {
                    invalid = true;
                    continue;
                }

                if (inputNum > 0 && inputNum <= monsters.Length)
                {
                    if (monsters[inputNum - 1].IsDead)
                    {
                        invalid = true;
                        continue;
                    }
                    else
                    {
                        invalid = false;
                        PlayerPhase(monsters[inputNum - 1]);
                    }
                }
                else if (inputNum == 0)
                    return;
                else
                {
                    invalid = true;
                    continue;
                }                    

                MonsterPhase(monsters);
                
                if (monsters.All(x => x.IsDead))
                    Victory(monsters.Length, startHp, monsters);
                else if (player.Hp <= 0)
                    Lose(startHp);

                return;
            }            
        }
        #endregion

        #region 공격
        private void PlayerPhase(Monster monster)
        {
            int error = player.Atk * 0.1f % 1 != 0 ? (int)(player.Atk * 0.1f) + 1 : (int)(player.Atk * 0.1f);
            int damage = player.Atk + new Random().Next(-error, error + 1);

            string prevHp = monster.Hp.ToString();
            monster.GetDamage(damage);
            string currentHp = monster.IsDead ? "Dead" : monster.Hp.ToString();

            BattlePrint();

            Console.WriteLine($"{player.Name} 의 공격!\n" +
            $"{monster.Name} 을(를) 맞췄습니다. [데미지 : {damage}]\n" +
            $"\n" +
            $"{monster.Name}\n" +
            $"HP {prevHp} -> {currentHp}\n" +
            $"\n" +
            $"계속\n" +
            $"\n");

            Console.ReadLine();
        }

        private void MonsterPhase(Monster[] monsters)
        {
            BattlePrint();

            foreach (Monster monster in monsters)
            {
                if (!monster.IsDead && player.Hp > 0)
                {
                    int prevHp = player.Hp;
                    player.GetDamage(monster.Atk);

                    Console.WriteLine($"{monster.Name} 의 공격!\n" +
                    $"{player.Name} 을(를) 맞췄습니다. [데미지 : {monster.Atk}]\n" +
                    $"\n" +
                    $"{player.Name}\n" +
                    $"HP {prevHp} -> {player.Hp}");
                    
                    Console.ReadLine();
                }                    
            }
        }
        #endregion

        #region 결과
        
        // 레벨업 기능 구현
        public void LevelCal(Monster[] monsters, Player player)
        {
            int point = 0;
        
            foreach(Monster monster in monsters)
            {
                point += monster.Lv;
            }

            if (point >= 10 && point < 35)
            {
                player.Lv = 2;
            }
            else if (point >= 35 && point < 65)
            {
                player.Lv = 3;
            } 
            else if (point >= 65 && point < 100)
            {
                player.Lv = 4;
            }
            else if (point >= 100)
            {
                player.Lv = 5;
            }
            
        }

        private void Victory(int monsterCount, int startHp, Monster[] monsters)
        {
            BattlePrint();
            Console.WriteLine("Result\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Victory\n");
            Console.ResetColor();
            int preLv = player.Lv;
            LevelCal(monsters, player); 
            Console.WriteLine(
                $"던전에 몬스터 {monsterCount}마리를 잡았습니다.\n" +
                $"\n" +
                $"Lv.{preLv} {player.Name} -> Lv.{player.Lv} {player.Name}\n" +
                $"HP {startHp} -> {player.Hp}\n" +
                $"\n" +
                $"계속");
             
            Console.ReadLine();
        }

        private void Lose(int startHp)
        {
            BattlePrint();
            Console.WriteLine("Result\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You Lose\n");
            Console.ResetColor();

            Console.WriteLine(
                $"Lv.{player.Lv} {player.Name}\n" +
                $"HP {startHp} -> {player.Hp}\n" +
                $"\n" +
                $"계속");

            Console.ReadLine();
        }
        #endregion

        #region 회복 아이템
        private void PotionInventory()
        {
            // 회복 아이템 사용이 가능한 장면

            /* 사양) ```
            회복
            포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션: 3 )

            1.사용하기
            0.나가기

            원하시는 행동을 입력해주세요.
            >>
            ```*/

            while (true)
            {
                Console.WriteLine(" __               "); // 임시로 여관 아트 가져옴
                Console.WriteLine("|__|.-----..-----.");
                Console.WriteLine("|  ||     ||     |");
                Console.WriteLine("|__||__|__||__|__|");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==================[회 복]==================");
                Console.ResetColor();
                Console.WriteLine("-------------------------------------------\n");

                // 정말 만약에 Potion 객체가 shop.items에 없을 경우를 위해 대비한 로직
                // 참조 타입 변수에 null이 할당될 가능성이 있을 때 나오는 경고문떄문에 아래와 같이 라인 수 많아짐.
                Potion? potion = shop.items.OfType<Potion>().FirstOrDefault();
                if (potion != null)
                {
                    Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {potion.count}개)");
                }
                else
                {
                    Console.WriteLine("게임에 포션이 구현되지 않아 갯수를 표시 할 수 없습니다.");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[현재 체력 : {player.Hp}]\n");
                Console.ResetColor();
                Console.WriteLine("");
                Console.WriteLine("(1) : 사용하기");
                Console.WriteLine("(0) : 나가기");
                Console.WriteLine("");
                Console.WriteLine("---------------------------------");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.ResetColor();
                Console.WriteLine("");

                string input = Console.ReadLine();

                Console.Clear();

                switch (input)
                {
                    case "1":

                        if(potion == null) // shop.items 배열에 Potion 객체가 존재하지 않을 경우
                        {
                            Console.WriteLine("게임에 포션이 구현되지 않아 사용 할 수 없습니다.");
                            break;
                        }
                        if (player.Hp == 100) // 체력이 100일 경우
                        {
                            Console.WriteLine("체력이 이미 모두 회복되어 포션을 사용 할 수 없습니다.");
                        }
                        else if (potion.count <= 0) // 포션이 0개 남아있을 경우
                        {
                            Console.WriteLine("현재 소지한 포션이 없습니다.");
                        }
                        else // 사용 요건을 충족하였을 경우
                        {
                            potion.UsePotionOnInventory(); // 포션 갯수를 한 개 줄이기
                            player.SetHp(Math.Min(player.Hp + 30, 100)); // 체력 +30, 100 초과 시 100으로 설정
                            Console.WriteLine($"포션을 사용하여 체력이 {player.Hp} 이 되었습니다.");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!!잘못된 입력입니다!!!");
                        Console.ResetColor();
                        break;
                }

            }

        }
        #endregion

        #region 휴식
        private void Rest()
        {
            bool canRest = player.Gold >= 500;
            bool rest = false;
            bool fullCondition = player.Hp == player.MaxHp;
            while (true)
            {
                Console.WriteLine(" __               ");
                Console.WriteLine("|__|.-----..-----.");
                Console.WriteLine("|  ||     ||     |");
                Console.WriteLine("|__||__|__||__|__|");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==================[여 관]==================");
                Console.ResetColor();
                Console.WriteLine("-------------------------------------------\n");

                Console.WriteLine($"500 G 를 지불하시면 체력을 회복할 수 있습니다. (보유골드 : {player.Gold} G)");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[현재 체력 : {player.Hp}]\n");
                Console.ResetColor();
                if (rest)
                {
                    if (fullCondition)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"이미 컨디션이 최상입니다!\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (canRest)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"회복되었습니다!\n");
                            Console.ResetColor();
                            fullCondition = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"숙박 비용이 모자랍니다!\n");
                            Console.ResetColor();
                        }
                    }
                }

                Console.WriteLine("(R) : [휴식]\n\n(B) : [마을로 돌아가기]\n");

                Console.WriteLine("-------------------------------------------");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("※※※원하시는 행동을 선택하세요.※※※");
                Console.WriteLine("※※※입력값은 대소문자를 구분하지 않습니다.※※※\n");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "R":
                    case "r":
                        rest = true;
                        canRest = player.Gold >= 500;
                        if (!fullCondition && canRest)
                        {
                            player.Rest();
                            player.GetGold(-500);                            
                        }
                        break;
                    case "B":
                    case "b":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!!잘못된 입력입니다!!!");
                        Console.ResetColor();
                        break;
                }
            }
        }
        #endregion

        #region Battle 아스키 아트
        private void BattlePrint()
        {
            Console.Clear();
            Console.WriteLine("" +
                "'||'''|,            ||      ||    '||`        \r\n" +
                " ||   ||            ||      ||     ||         \r\n" +
                " ||;;;;    '''|.  ''||''  ''||''   ||  .|''|, \r\n" +
                " ||   ||  .|''||    ||      ||     ||  ||..|| \r\n" +
                ".||...|'  `|..||.   `|..'   `|..' .||. `|...  \n\n");
        }
        #endregion
    }
}
