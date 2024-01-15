﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RtanTextDungeon.Define;
using static RtanTextDungeon.DataManager;

namespace RtanTextDungeon
{    
    internal class Dungeon
    {
        // 인게임에서 사용될 Player
        private Player player = null;        
        // 인게임에서 상점 이용에 쓰일 아이템 목록을 가지고 있는 Shop 필드
        private Shop shop = new Shop();

        private int chooseFloor = 0;

        int startHp;
        int startMp;

        #region 게임시작
        public void EnterGame()
        {
            player = LoadGame<Player>("PlayerData.json");

            if (player == null)
                CharacterCreation();
            else
                shop.Restore(player);

            string alertMsg = "";

            while (true)
            {
                UI.AsciiArt(UI.AsciiPreset.MainMenu);

                Console.WriteLine("  (E) : [상태]\n\n  (I) : [인벤토리]\n\n  (S) : [상점]\n\n  (D) : [던전입장]\n\n  (P) : [회복아이템]\n\n  (R) : [휴식]\n\n  (X) : [게임종료]\n");
                
                string input = UI.UserInput(alertMsg);
                alertMsg = "";

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
                        UI.ColoredWriteLine("※※※게임을 종료합니다※※※", ConsoleColor.Yellow);                        
                        SaveGame(player, "PlayerData.json");                        
                        return;
                    default:
                        alertMsg="잘못된 입력입니다!";
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
            string className = string.Empty;

            while(true)
            {   
                if(name == "")
                {
                    UI.AsciiArt(UI.AsciiPreset.CreateCharacter);

                    Console.WriteLine("  이름을 입력하십시오.");
                    Console.WriteLine("");
                    Console.Write("  >>> ");

                    name = Console.ReadLine();
                    Console.Clear();
                    if (name == "")
                    {
                        Console.WriteLine("  입력 값이 없습니다.");                        
                        continue;
                    }
                }

                UI.AsciiArt(UI.AsciiPreset.CreateCharacter);

                Console.WriteLine($"  입력하신 이름은 {name} 입니다.");
                Console.WriteLine("");

                Console.WriteLine("  1. 저장");
                Console.WriteLine("  2. 취소");

                Console.WriteLine("");
                Console.Write("  >>> ");

                input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "1":
                        break;
                    case "2":
                        name = "";
                        continue;
                    default:                        
                        Console.WriteLine("  올바르지 않은 입력입니다.");
                        continue;
                }

                break;
            }

            while (true)
            {
                // 직업 선택 영역 구현          
                UI.AsciiArt(UI.AsciiPreset.SelectClass);

                Console.WriteLine("  전직할 클래스를 선택 해 주세요.");
                Console.WriteLine("");
                Console.WriteLine("  1. 전사 - 공격력: 10, 방어력: 5, 최대 체력: 100, 최대 마나: 100, GOLD: 1500");
                Console.WriteLine("  2. 궁수 - 공격력: 12, 방어력: 3, 최대 체력: 90, 최대 마나: 100, GOLD: 1500");
                Console.WriteLine("  3. 마법사 - 공격력: 10, 방어력: 4, 최대 체력: 80, 최대 마나: 120, GOLD: 1500");
                Console.WriteLine("  4. 도둑 - 공격력: 11, 방어력: 4, 최대 체력: 80, 최대 마나: 100, GOLD: 1500");
                Console.WriteLine("  5. 무직백수 - 공격력: 6, 방어력: 5, 최대 체력: 100, 최대 마나: 100, GOLD: 500");
                Console.WriteLine("  ");
                Console.Write("  >>> ");

                input = Console.ReadLine();

                Console.Clear();// 콘솔 화면 한 번 지우기
                switch (input)
                {
                    case "1":
                        player = new Warrior(name);
                        break;
                    case "2":
                        player = new Archer(name);
                        break;
                    case "3":
                        player = new Magic(name);
                        break;
                    case "4":
                        player = new Thief(name);
                        break;
                    case "5":
                        player = new Deadbeat(name);
                        break;
                    default:
                        Console.WriteLine("  올바르지 않은 입력입니다.");
                        continue;
                }

                UI.AsciiArt(UI.AsciiPreset.SelectClass);
                Console.WriteLine($"  선택하신 직업은 [{player.GetClassName()}] 입니다.");
                Console.WriteLine("");

                Console.WriteLine("  1. 저장");
                Console.WriteLine("  2. 취소");
                Console.WriteLine("");

                Console.Write("  >>> ");

                input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "1":
                        break;
                    case "2":
                        player = null;
                        continue;
                    default:
                        Console.WriteLine("  올바르지 않은 입력입니다.");
                        continue;
                }

                break;

            }


        }
        #endregion

        #region 상태창
        private void Status()
        {
            string weaponStatus = player.equippedItems.ContainsKey("Weapon") ? $"{player.equippedItems["Weapon"].AdditionalATK}" : "";
            string armorStatus  = player.equippedItems.ContainsKey("Armor") ? player.equippedItems["Armor"].AdditionalDEF : "";
            string amuletATK    = player.equippedItems.ContainsKey("Amulet") ? player.equippedItems["Amulet"].AdditionalATK : "";
            string amuletDEF    = player.equippedItems.ContainsKey("Amulet") ? player.equippedItems["Amulet"].AdditionalDEF : "";

            string alertMsg = "";

            while (true)
            {

                UI.AsciiArt(UI.AsciiPreset.Status);

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("  .=================================.\n");
                Console.WriteLine("");
                Console.WriteLine($"    Lv. {player.Lv.ToString("00")}\n\n" +
                                $"    이름\t:  {player.Name}({player.GetClassName()})\n\n" +
                                //$"    레벨\t:  Lv. {player.Lv.ToString("00")}\n" +
                                $"    공격력\t:  {player.Atk} {weaponStatus + amuletATK}\n" +
                                $"    방어력\t:  {player.Def} {armorStatus + amuletDEF}\n" +
                                $"    체  력\t:  {player.Hp} / {player.MaxHp}\n" +
                                $"    마  나\t:  {player.Mp} / {player.MaxMp}\n\n" +
                                $"    Gold\t:  {player.Gold:N0} G\n");

                Console.WriteLine("");
                Console.WriteLine("  '================================='\n");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("  (I) : [인벤토리]\n\n  (S) : [상점]\n\n  (B) : [마을로 돌아가기]\n");

                string input = UI.UserInput(alertMsg);
                alertMsg = "";

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
                        alertMsg = "잘못된 입력입니다!";
                        break;
                }
            }
        }
        #endregion

        #region 인벤토리
        private void Inventory()
        {
            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                UI.AsciiArt(UI.AsciiPreset.Inventory);
                UI.ColoredWriteLine(" [아이템 목록]\n", ConsoleColor.DarkGreen);
                // 아이템 목록은 아이템 리스트에 있는 아이템들을 전부 불러와야겠지?

                int index = 1;
                if (player.Items.Count == 0)
                    Console.WriteLine($" ㄴ 비어있음");
                foreach (int Itemindex in player.Items)
                {
                    Item item = shop.items[Itemindex];

                    if (item.IsEquip)
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    if (item is Weapon weapon)
                        Console.WriteLine($"  - ({index})  {weapon.AbilityName} : +{weapon.damage}\t| {weapon.Name}\t| {weapon.Desc}");
                    else if (item is Armor armor)
                        Console.WriteLine($"  - ({index})  {armor.AbilityName} : +{armor.defense}\t| {armor.Name}\t| {armor.Desc}");
                    else if (item is Amulet amulet)
                        Console.WriteLine($"  - ({index})  {amulet.AbilityName} : +{amulet.damage} / +{amulet.defense}\t| {amulet.Name}\t| {amulet.Desc}");
                    Console.ResetColor();
                    index++;
                }
                Console.WriteLine();
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("  (E) : [상태]\n\n  (S) : [상점]\n\n  (B) : [마을로 돌아가기]\n");

                string input = UI.UserInput(alertMsg);
                alertMsg = "";

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
                        if (int.TryParse(input, out itemIndex) && itemIndex <= player.Items.Count && itemIndex > 0)
                        {
                            itemIndex--;
                            player.EquipOrUnequipItem(shop.items[player.Items[itemIndex]]);
                        }
                        else
                        {
                            alertMsg = "잘못된 입력입니다!";
                        }
                        break;
                }
            }
        }
        #endregion

        #region 상점
        private void Shop()
        {
            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                UI.AsciiArt(UI.AsciiPreset.Shop);
                UI.ColoredWriteLine($"[보유 골드 : {player.Gold:N0} G]\n", ConsoleColor.Yellow);                

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
                        Console.WriteLine($"- ({index})\t\t{weapon.AbilityName} : +{weapon.damage}\t\t\t| {weapon.Price:N0} G    \t| {weapon.Name}\t| {weapon.Desc}\n");
                    else if (item is Armor armor)
                        Console.WriteLine($"- ({index})\t\t{armor.AbilityName} : +{armor.defense}\t\t\t| {armor.Price:N0} G    \t| {armor.Name}\t| {armor.Desc}\n");
                    else if (item is Amulet amulet)
                        Console.WriteLine($"- ({index})\t\t{amulet.AbilityName} : +{amulet.damage} / +{amulet.defense}\t| {amulet.Price:N0} G    \t| {amulet.Name}\t| {amulet.Desc}\n");
                    index++;
                }

                Console.WriteLine();
                UI.ColoredWriteLine("아이템 번호를 입력하시면 구매/판매가 가능합니다.\n", ConsoleColor.Yellow);
                Console.WriteLine();

                Console.WriteLine("  (E) : [상태]\n\n  (I) : [인벤토리]\n\n  (B) : [마을로 돌아가기]\n");

                string input = UI.UserInput(alertMsg);
                alertMsg = "";

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
                            alertMsg = "잘못된 입력입니다!";
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
            bool choiceFloorPanel = false;

            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                Console.Clear();

                UI.AsciiArt(UI.AsciiPreset.DungeonEntrance2);
                if (status) UI.AsciiArt(UI.AsciiPreset.DungeonEntrance3);
                UI.AsciiArt(UI.AsciiPreset.DungeonEntrance);

                Console.WriteLine("  .=============================.\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (!status)
                    Console.WriteLine("  (E) : ▶ 내 정보\n");
                else
                {
                    Console.WriteLine("  (E) : ▼ 내 정보");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n    Lv. {player.Lv.ToString("00")}\t{player.Name}\n\n" +
                                $"    공격력\t:  {player.Atk}\n" +
                                $"    방어력\t:  {player.Def}\n" +
                                $"    체  력\t:  {player.Hp}\n" +
                                $"    마  나\t:  {player.Mp}\n" +
                                $"    Gold\t:  {player.Gold:N0} G\n");
                }
                Console.ResetColor();
                Console.WriteLine("  '============================='\n");

                if (!choiceFloorPanel)
                {
                    Console.WriteLine("\n" +
                    $"  (1) 전투 시작 (현재 진행 : {DungeonInfo.HighestFloor}층)\n\n" +
                    $"  (2) 다른 층 선택\n\n" +
                    "  (B) 마을로 돌아가기\n\n");
                }
                else
                {                    
                    for (int i = 1; i <= DungeonInfo.HighestFloor; i++)
                        Console.WriteLine($"({i}) {i}층 진입");
                    Console.WriteLine("(0) 취소\n");
                }

                Console.ForegroundColor = ConsoleColor.Red;
                if (hpZero)
                Console.WriteLine("  체력이 없습니다. 여관에서 휴식을 취하세요.\n");
                Console.ResetColor();

                string input = UI.UserInput(alertMsg);
                alertMsg = "";
                isAlertPositive = false;

                Console.Clear();

                startHp = player.Hp;
                startMp = player.Mp;

                switch (input)
                {
                    case "1":
                        if (choiceFloorPanel)
                            chooseFloor = 1;
                        else
                            chooseFloor = DungeonInfo.HighestFloor;
                        if (player.Hp > 0)
                        {
                            EnterDungeon(startHp, startMp);
                            choiceFloorPanel = false;
                        }                            
                        else
                            hpZero = true;
                        break;
                    case "2":
                        if (!choiceFloorPanel)
                        {
                            choiceFloorPanel = true;
                            continue;
                        }                            
                        else
                            chooseFloor = 2;

                        if (player.Hp > 0)
                        {
                            EnterDungeon(startHp, startMp);
                            choiceFloorPanel = false;
                        }
                        else
                            hpZero = true;
                        break;
                    case "0":
                        if (choiceFloorPanel)
                            choiceFloorPanel = false;
                        else
                        {
                            alertMsg = "잘못된 입력입니다!";
                        }
                        break;
                    case "E":
                    case "e":
                        status = !status;
                        break;
                    case "B":
                    case "b":
                        return;
                    default:
                        if (choiceFloorPanel)
                        {
                            int inputNum;
                            bool isDigit = int.TryParse(input, out inputNum);
                            if (isDigit && inputNum > 0 && inputNum <= DungeonInfo.HighestFloor)
                            {
                                chooseFloor = inputNum;
                                if (player.Hp > 0)
                                {
                                    EnterDungeon(startHp, startMp);
                                    choiceFloorPanel = false;
                                    break;
                                }
                                else
                                {
                                    hpZero = true;
                                    continue;
                                }                                    
                            }
                        }
                        alertMsg = "잘못된 입력입니다!";
                        break;
                }
            }
        }
        #endregion

        #region 배틀
        private void EnterDungeon(int startHp, int startMp)
        {
            #region 몬스터 스폰
            Monster[] monsters = DungeonInfo.MonsterSpawn(chooseFloor); 
            #endregion
            bool invalid = false;

            bool isSkillShow = false;
            bool isManaLack = false;
            bool isUseItem = false;

            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                UI.AsciiArt(UI.AsciiPreset.Battle);

                Console.WriteLine($"현재 층 : {DungeonInfo.CurrentFloor} / 최고 층 : {DungeonInfo.HighestFloor}");
                Console.WriteLine($"" +
                    $"====================\n" +
                    $"현재 던전 : {DungeonInfo.CurrentFloor}층\n" +
                    $"====================\n");

                Console.WriteLine("=============[몬스터 목록]============\n");
                for (int i = 0; i < monsters.Length; i++)
                    monsters[i].ShowText();
                Console.WriteLine("\n======================================\n");

                Console.WriteLine($"\n" +
                    $"[내정보]\n" +
                    $"Lv.{player.Lv}\t{player.Name}\n" +
                    $"HP {player.Hp}/{player.MaxHp}\n" +
                    $"MP {player.Mp}/{player.MaxMp}\n\n");
                
                if(!(isSkillShow||isUseItem))
                {
                    Console.WriteLine("1. 공격\n");
                    Console.WriteLine("2. 스킬\n");
                    Console.WriteLine("3. 회복\n");
                    Console.WriteLine("0. 도망친다!\n");

                    string input = UI.UserInput(alertMsg, isAlertPositive);
                    alertMsg = "";
                    isAlertPositive = false;

                    Console.Clear();
                    switch (input)
                    {
                        case "1":
                            Fight(monsters, startHp, startMp, 0);
                            invalid = false;
                            break;
                        case "2":
                            isSkillShow = true; //스킬 선택 화면으로
                            break;
                        case "3":
                            isUseItem = true; //회복 화면으로
                            break;
                        case "0":
                            if (TryRun(monsters)) // 도망에 성공하면 던전 탈출
                                return;
                            break;
                        default:
                            alertMsg = "잘못된 입력입니다!";
                            continue;
                    }
                }
                else if(isSkillShow) // 스킬 선택 화면
                {

                    for(int i = 0; i < player.Skills.Count; ++i)
                    {
                        player.Skills[i].ShowText();
                    }
                    Console.WriteLine("0. 취소\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n");

                    if (invalid)
                        Console.WriteLine("잘못된 입력입니다.");
                    if (isManaLack)
                        UI.ColoredWriteLine("마나가 부족합니다.", ConsoleColor.Red);
                    invalid = false;
                    isManaLack = false;

                    string input = UI.UserInput(alertMsg, isAlertPositive);
                    alertMsg = "";
                    isAlertPositive = false;

                    Console.Clear();
                    switch (input)
                    {
                        case "0":
                            isSkillShow = false; //공격, 스킬 선택 화면으로
                            continue;
                        default:
                            int skillNum; // 선택한 스킬 번호

                            //입력 값이 (숫자 and 1 이상 and 스킬 개수 이하) 인 경우
                            if (int.TryParse(input,out skillNum) && skillNum >= 1 && (skillNum - 1) < player.Skills.Count)
                            {
                                if (!player.Skills[skillNum-1].IsAvailable())
                                {
                                    isManaLack = true;
                                    continue;
                                }
                                    
                                // Fight 메서드에 스킬 넘버(1~N) 전달
                                Fight(monsters, startHp, startMp, skillNum);
                                isSkillShow = false;
                                break;
                            }
                            else
                                alertMsg = "잘못된 입력입니다!";
                            continue;
                    }
                }
                else if (isUseItem)
                {
                    Potion? potion = shop.items.OfType<Potion>().FirstOrDefault(p => p.ID == 1000);
                    Console.WriteLine($"1. {potion.Name}을 사용 ( 남은 수량 : {potion.count} )\n");
                    Console.WriteLine("0. 취소\n");
                    Console.WriteLine("");
                    Console.WriteLine("");

                    string input = UI.UserInput(alertMsg, isAlertPositive);
                    alertMsg = "";
                    isAlertPositive = false;

                    Console.Clear();
                    switch (input)
                    {
                        case "0": // 이전 메뉴로
                            isUseItem = false;
                            alertMsg = "";
                            continue;
                        case "1": // 회복약 사용 시도
                            int preHp = player.Hp;
                            bool isPotionUsed = potion.Use(player);
                            if (isPotionUsed){
                                alertMsg = $"체력을 {player.Hp-preHp} 회복하여 [{preHp} → {player.Hp}] 이 되었습니다.";
                                isAlertPositive = true;
                            }
                            else
                            {
                                if (player.Hp == player.MaxHp) alertMsg = $"체력이 이미 모두 회복되어 회복약을 사용 할 수 없습니다.";
                                if (potion.count <= 0) alertMsg = $"{potion.Name}을 소지하고 있지 않습니다.";
                            }
                            startHp = player.Hp;
                            continue;
                        default:
                            alertMsg = "잘못된 입력입니다.";
                            continue;
                    }
                }
                if (monsters.All(x => x.IsDead) || player.Hp <= 0)
                    return;
            }                   
        }

        private void Fight(Monster[] monsters, int startHp, int startMp, int skillNum)
        {            
            bool invalid = false;    

            bool isMultiTarget = false; // 선택한 스킬이 다중 타격인지
            if (skillNum > 0 && player.Skills[skillNum-1].NumberTargets > 1) // 
                isMultiTarget = true;

            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {

                UI.AsciiArt(UI.AsciiPreset.Battle);

                Console.WriteLine($"" +
                    $"====================\n" +
                    $"현재 던전 : {DungeonInfo.CurrentFloor}층\n" +
                    $"====================\n");

                Console.WriteLine("=============[몬스터 목록]============\n");
                for (int i = 0; i < monsters.Length; i++)
                    monsters[i].ShowText(i + 1);
                Console.WriteLine("\n======================================\n");

                // 전투 종료
                if (monsters.All(x => x.IsDead))
                {
                    Victory(monsters.Length, startHp, startMp, monsters);   
                    return;
                }                        
                else if (player.Hp <= 0)
                {
                    Lose(startHp, startMp);
                    return;
                }

                // 단일 타겟인 경우
                if (!isMultiTarget)
                {
                    Console.WriteLine($"\n" +
                    $"[내정보]\n" +
                    $"Lv.{player.Lv}\t{player.Name}\n" +
                    $"HP {player.Hp}/{player.MaxHp}\n\n");

                    Console.WriteLine("0. 취소\n");
                    Console.WriteLine("대상을 선택해주세요.\n");

                    if (invalid)
                        alertMsg = "잘못된 입력입니다!";

                    string input = UI.UserInput(alertMsg, isAlertPositive);
                    alertMsg = "";
                    isAlertPositive = false;

                    int inputNum;
                    bool isNum = int.TryParse(input, out inputNum);
                    if (!isNum)
                    {
                        invalid = true;
                        continue;
                    }

                    if (inputNum > 0 && inputNum <= monsters.Length)
                    {
                        if (monsters[inputNum - 1].IsDead) //몬스터 생존 여부
                        {
                            invalid = true;
                            continue;
                        }
                        else
                        {
                            invalid = false;
                            PlayerPhase(monsters[inputNum - 1], skillNum);
                        }
                    }
                    else if (inputNum == 0)
                        return;
                    else
                    {
                        invalid = true;
                        continue;
                    }
                }
                // 다중 타겟인 경우
                else
                {
                    PlayerPhase(monsters, skillNum);
                }

                MonsterPhase(monsters);
                
                if (monsters.All(x => x.IsDead))
                    Victory(monsters.Length, startHp, startMp, monsters);                

                return;
            }            
        }

        /// <summary>
        /// 40% 확률로 도망치기에 성공. 성공 시, 던전 입구로 복귀
        /// </summary>        
        private bool TryRun(Monster[] monsters)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }

            int randRun = new Random().Next(0, 10);
            if (randRun < 6)
            {
                UI.ColoredWriteLine("\n스파르타 교육을 뿌리칠 수 없었습니다... [몬스터의 턴으로 넘어갑니다]", ConsoleColor.DarkRed);
                UI.UserInput(reqMsg: "아무 키나 입력해주세요.");
                MonsterPhase(monsters);
            }
            else
            {
                UI.ColoredWriteLine("\n뒤도 돌아보지 않고 도망쳤습니다. [잠시 후 던전입구에 도착합니다]",ConsoleColor.Green);
                UI.UserInput(reqMsg: "아무 키나 입력해주세요.");
                return true;
            }                
            return false;                            
        }
        #endregion

        #region 공격
        //기본 공격 or 단일 공격 스킬 사용 시 호출
        private void PlayerPhase(Monster monster, int skillNum)
        {
            int error = player.Atk * 0.1f % 1 != 0 ? (int)(player.Atk * 0.1f) + 1 : (int)(player.Atk * 0.1f);
            float damage;
            if (skillNum <= 0)
                damage = player.Atk + new Random().Next(-error, error + 1);
            else//skillNum(선택한 스킬 번호)가 1 이상이면 데미지에 스킬 배수 곱해주기
                damage = player.Skills[skillNum - 1].UseSkill(player.Atk + new Random().Next(-error, error + 1)); 
            
            // 크리티컬, 몬스터 회피, 실데미지 계산
            (bool isCritical, bool isDodged, damage) = player.CalculateExDamage(damage, skillNum > 0);

            string prevHp = monster.Hp.ToString();
            monster.GetDamage(damage);
            string currentHp = monster.IsDead ? "Dead" : monster.Hp.ToString();

            UI.AsciiArt(UI.AsciiPreset.Battle);

            Console.WriteLine($"{player.Name} 의 공격!\n" +
            $"\n" +
            $"{monster.Name} {(isDodged ? "은(는) 공격을 피했습니다!" : "을(를) 맞췄습니다.")} [데미지{(isCritical ? "(크리티컬!!)" : "")} : {damage}]\n" +
            $"\n" +
            $"{monster.Name}\n");
            UI.ColoredWriteLine($"HP {prevHp} -> {currentHp}\n", ConsoleColor.Red);
            Console.WriteLine("\n계속\n");

            Console.ReadLine();
        }

        //다중 공격 스킬 사용 시 호출 (오버로드)
        private void PlayerPhase(Monster[] monsters, int skillNum) 
        {
            Random random = new Random();

            int error = player.Atk * 0.1f % 1 != 0 ? (int)(player.Atk * 0.1f) + 1 : (int)(player.Atk * 0.1f);
            
            float damage = player.Skills[skillNum - 1].UseSkill(player.Atk + new Random().Next(-error, error + 1));

            // 크리티컬, 몬스터 회피, 실데미지 계산
            (bool isCritical, bool isDodged, damage) = player.CalculateExDamage(damage, skillNum > 0);

            int MonsterNum = monsters.Count(x => !x.IsDead);            // 살아있는 몬스터 수
            int TargetNum = player.Skills[skillNum - 1].NumberTargets;  // 스킬로 공격할 몬스터 수

            // 다중 공격 타겟 수가 살아있는 몬스터 수 이상일 때
            if (TargetNum >= MonsterNum)
            {
                UI.AsciiArt(UI.AsciiPreset.Battle);

                // => 살아있는 것 전부 공격
                foreach (Monster monster in monsters.Where(x => !x.IsDead)) 
                {
                    string prevHp = monster.Hp.ToString();
                    monster.GetDamage(damage);
                    string currentHp = monster.IsDead ? "Dead" : monster.Hp.ToString();

                    Console.WriteLine($"{player.Name} 의 공격!\n" +
                        $"\n" +
                        $"{monster.Name} {(isDodged ? "은(는) 공격을 피했습니다!" : "을(를) 맞췄습니다.")} [데미지{(isCritical ? "(크리티컬!!)" : "")} : {damage}]\n" +
                        $"\n" +
                        $"{monster.Name}\n");
                    UI.ColoredWriteLine($"HP {prevHp} -> {currentHp}\n", ConsoleColor.Red);
                    Console.WriteLine("\n계속\n");

                    Console.ReadLine();
                }
            }
            else // 다중 공격 타겟 수가 살아있는 몬스터 수 미만일 때
            {
                // => 다중 공격 타겟 수만큼 랜덤으로 공격

                //임시 배열에 몬스터 복제하고 셔플
                Monster[] shuffledMonsters = monsters.ToArray();  
                for(int i = shuffledMonsters.Length - 1; i > 0; i--)
                {
                    int j = random.Next(0, i + 1);
                    Monster tempMonster = shuffledMonsters[i];
                    shuffledMonsters[i] = shuffledMonsters[j];
                    shuffledMonsters[j] = tempMonster;
                }

                UI.AsciiArt(UI.AsciiPreset.Battle);

                //살아있는 몬스터 중에 다중 공격 타겟 수 만큼 순회
                foreach (Monster monster in shuffledMonsters.Where(x => !x.IsDead).Take(TargetNum))
                {
                    string prevHp = monster.Hp.ToString();
                    monster.GetDamage(damage);
                    string currentHp = monster.IsDead ? "Dead" : monster.Hp.ToString();

                    Console.WriteLine($"{player.Name} 의 공격!\n" +
                        $"\n" +
                        $"{monster.Name} {(isDodged ? "은(는) 공격을 피했습니다!" : "을(를) 맞췄습니다.")} [데미지{(isCritical ? "(크리티컬!!)" : "")} : {damage}]\n" +
                        $"\n" +
                        $"{monster.Name}\n");
                    UI.ColoredWriteLine($"HP {prevHp} -> {currentHp}\n", ConsoleColor.Red);
                    Console.WriteLine("\n계속\n");

                    Console.ReadLine();
                }
            }
        }

        private void MonsterPhase(Monster[] monsters)
        {
            UI.AsciiArt(UI.AsciiPreset.Battle);

            foreach (Monster monster in monsters)
            {
                if (!monster.IsDead && player.Hp > 0)
                {
                    int prevHp = player.Hp;

                    float damage = monster.Atk;
                    int reduceDmg = (int)(player.Def * 0.5f) >= damage ? (int)damage - 1 : (int)(player.Def * 0.5f);
                    int applyDmg = damage - reduceDmg <= 0 ? 1 : (int)(damage - reduceDmg);

                    player.GetDamage(applyDmg);

                    Console.WriteLine($"{monster.Name} 의 공격!\n" +
                    $"{player.Name} 을(를) 맞췄습니다. [받은 데미지 : {applyDmg}] [데미지 경감 : {reduceDmg}]\n" +
                    $"\n" +
                    $"{player.Name}\n");
                    UI.ColoredWriteLine($"HP {prevHp} -> {player.Hp}",ConsoleColor.Red);
                    
                    Console.ReadLine();
                }                    
            }

            if (player.Hp <= 0)
                Lose(startHp, startMp);
        }
        #endregion

        #region 결과
        
        // 레벨업 기능 구현
        public void LevelCal(Monster[] monsters, Player player)
        {
            foreach(Monster monster in monsters)
            {
                player.Point += monster.Lv;
            }

            if (player.Point >= 10 && player.Point < 35)
            {
                player.Lv = 2;
                player.StatUp();
            }
            else if (player.Point >= 35 && player.Point < 65)
            {
                player.Lv = 3;
                player.StatUp();
            } 
            else if (player.Point >= 65 && player.Point < 100)
            {
                player.Lv = 4;
                player.StatUp();
            }
            else if (player.Point >= 100)
            {
                player.Lv = 5;
                player.StatUp();
            }            
        }

        private void Victory(int monsterCount, int startHp, int startMp, Monster[] monsters)
        {
            // Potion 드랍 획득. 드랍 기능의 확장이 필요하다면, 이후 따로 클래스나 메서드를 두는 것을 추천.
            int nPotionDrop = 0;
            Potion? potion = shop.items.OfType<Potion>().FirstOrDefault(p => p.ID == 1000);
            if (potion != null) nPotionDrop = potion.Get(0, monsterCount);
            // Gold 보상 획득
            int addGold = 0;
            addGold = new Random().Next((int)(0.5f * 500), monsterCount * 500);
            player.GetGold(addGold);


            UI.AsciiArt(UI.AsciiPreset.Battle);

            DungeonInfo.UpdateInfo();
            Console.WriteLine($"현재 층 : {DungeonInfo.CurrentFloor} / 최고 층 : {DungeonInfo.HighestFloor}");
            Console.WriteLine("Result\n");

            UI.ColoredWriteLine("Victory\n", ConsoleColor.Green);
            int preLv = player.Lv;
            LevelCal(monsters, player); 
            Console.WriteLine(
                $"스파르타의 괴물 {monsterCount} 마리 조차 당신을 막을 순 없었습니다.\n" +
                $"\n" +
                $"Lv.{preLv} {player.Name} -> Lv.{player.Lv} {player.Name}\n" +
                $"HP {startHp} -> {player.Hp}\n" +
                $"HP {startMp} -> {player.Mp}\n" +
                $"\n" +
                $"\n" +
                $"{addGold} G 를 획득했습니다. [ {player.Gold} G ]\n" +
                (nPotionDrop!=0?$"\n{potion.Name}을 {nPotionDrop} 개 획득했습니다. [ {potion.count} 개 ]\n":"") +
                $"\n" +
                $"\n" +
                $">>> 계속");
             
            Console.ReadLine();
        }

        private void Lose(int startHp, int startMp)
        {
            UI.AsciiArt(UI.AsciiPreset.Battle);
            Console.WriteLine("Result\n");
            UI.ColoredWriteLine("스파르타의 힘 앞에 굴복했습니다.\n", ConsoleColor.DarkRed);

            Console.WriteLine(
                $"Lv.{player.Lv} {player.Name}\n" +
                $"HP {startHp} -> {player.Hp}\n" +
                $"HP {startMp} -> {player.Mp}\n" +
                $"\n" +
                $">>> 계속");

            Console.ReadLine();
        }
        #endregion

        #region 회복 아이템
        private void PotionInventory()
        {
            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                UI.AsciiArt(UI.AsciiPreset.PotionInventory);
                Console.WriteLine("");

                // Potion 객체가 shop.items에 없을 경우를 위해 대비한 로직
                Potion? potion = shop.items.OfType<Potion>().FirstOrDefault(p => p.ID == 1000);
                if (potion != null)
                {
                    Console.WriteLine($"  {potion.Name}을 사용하면 체력을 {potion.heal} 회복 할 수 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine();
                    UI.ColoredWriteLine($"  [ 남은 {potion.Name} : {potion.count} 개 ]", ConsoleColor.Yellow);
                }
                else
                {
                    Console.WriteLine("  게임에 포션이 구현되지 않아 갯수를 표시 할 수 없습니다.");
                }

                Console.WriteLine("");
                UI.ColoredWriteLine($"  [ 현재 체력 : {player.Hp} / {player.MaxHp} ]\n", ConsoleColor.Green);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("  (P) : [사용]");
                Console.WriteLine("");
                Console.WriteLine("  (B) : [마을로 돌아가기]");
                Console.WriteLine("");

                string input = UI.UserInput(alertMsg, isAlertPositive);
                alertMsg = "";
                isAlertPositive = false;

                Console.Clear();

                switch (input)
                {
                    case "P":
                    case "p":

                        if (potion == null) // shop.items 배열에 Potion 객체가 존재하지 않을 경우
                        {
                            alertMsg = "게임에 포션이 구현되지 않아 사용 할 수 없습니다.";
                            break;
                        }

                        if (potion.Use(player)) // 포션사용여부(bool) 반환
                        {
                            alertMsg = $"{potion.Name}을 사용하여 체력이 {player.Hp} 이 되었습니다.";
                            isAlertPositive = true;
                        }
                        else
                        {
                            if (player.Hp == player.MaxHp)
                            {
                                alertMsg = $"이미 컨디션이 최상입니다!";
                                isAlertPositive = true;
                            }
                            if (potion.count <= 0) alertMsg = $"현재 소지한 {potion.Name}이 없습니다.";
                        }
                        break;

                    case "B":
                    case "b":
                        return;

                    default:
                        alertMsg = "잘못된 입력입니다!";
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
            bool isMeditated = false;
            bool fullCondition = player.Hp == player.MaxHp;

            string alertMsg = "";
            bool isAlertPositive = false;

            while (true)
            {
                Console.Clear();
                UI.AsciiArt(UI.AsciiPreset.Inn);

                Console.WriteLine($"  500 G 를 지불하시면 체력을 회복할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine( "  명상을 통해 마나를 회복하세요. (무료)");
                Console.WriteLine();
                Console.WriteLine();
                UI.ColoredWriteLine($"  [ 보유골드 : {player.Gold} G ]",ConsoleColor.Yellow);
                Console.WriteLine();
                UI.ColoredWriteLine($"  [ 현재 체력 : {player.Hp} / {player.MaxHp} ]\n", ConsoleColor.Green);
                UI.ColoredWriteLine($"  [ 현재 마나 : {player.Mp} / {player.MaxMp} ]\n", ConsoleColor.Blue);
                Console.WriteLine();
                Console.WriteLine();
                if (rest)
                {
                    if (fullCondition)
                    {
                        //UI.ColoredWriteLine($"  이미 컨디션이 최상입니다!\n", ConsoleColor.Green);
                        alertMsg = "이미 컨디션이 최상입니다!";
                        isAlertPositive = true;
                    }
                    else
                    {
                        if (canRest)
                        {
                            //UI.ColoredWriteLine($"  회복되었습니다!\n", ConsoleColor.Green);
                            alertMsg = "회복되었습니다!";
                            fullCondition = true;
                        }
                        else
                        {
                            //UI.ColoredWriteLine($"  숙박 비용이 모자랍니다!\n", ConsoleColor.DarkRed);
                            alertMsg = "숙박 비용이 모자랍니다!";
                        }
                    }
                }
                else if(isMeditated)
                {
                    alertMsg = "마나가 회복됐습니다.";
                    isAlertPositive = true;
                    isMeditated = false;
                }    

                Console.WriteLine("  (R) : [휴식]\n\n  (M) : [명상]\n\n  (B) : [마을로 돌아가기]\n");

                string input = UI.UserInput(alertMsg, isAlertPositive);
                alertMsg = "";
                isAlertPositive = false;

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
                    case "M":
                    case "m":
                        player.Meditate();
                        isMeditated = true;
                        break;
                    default:
                        alertMsg = "잘못된 입력입니다!";
                        break;
                }
            }
        }
        #endregion

    }
}
