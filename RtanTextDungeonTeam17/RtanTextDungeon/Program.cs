using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.RegularExpressions;
// 이도현 테스트 주석

namespace RtanTextDungeon
{
    internal class Program  // test 주석
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 200;
            Console.WindowHeight = 60;



            // 저장할 폴더의 위치
            string saveFolderName = "C:\\Users\\User\\Documents\\GitHub\\SpatranProject";            

            // 아이템 상점에 쓰일 Shop 클래스
            Shop shop = new Shop();
            // 플레이어를 불러오기를 통해 생성한다.
            Console.WriteLine("데이터를 불러오는중...");
            Player player = LoadPlayer(saveFolderName, shop);            
            Thread.Sleep(1000);

            // 인 게임 Dungeon클래스
            Dungeon dungeon = new Dungeon();
            // EnterGame 메서드로 게임에 입장한다. 들고갈 정보는 Player와 Shop
            dungeon.EnterGame(player, shop);

            // 저장하기 전, 플레이어의 능력치 중 아이템 보정을 받는 ATK, DEF의 보정수치를 제거한다.
            // 저장하면 가지고 있던 아이템 정보는 사라지지만 보정 수치는 남기 때문.
            player.RemoveItemAbilityBeforeSave();
            // 플레이어 저장
            SavePlayer(player, saveFolderName);
        }

        // 불러오기
        static Player LoadPlayer(string saveFolderName, Shop shop)
        {
            // 불러올 파일은 세 가지.
            // 1. 플레이어 Status를 담은 파일
            // 2. 플레이어가 보유중인 Item 의 ID를 담은 파일
            // 3. 플레이어가 장착중인 Item 의 ID를 담은 파일
            string playerDataName       = "PlayerData.json";
            string playerItemsName      = "PlayerItems.json";
            string equippedItemsName    = "EquippedItems.json";

            // 각각의 경로.
            string playerDataPath       = Path.Combine(saveFolderName, playerDataName);
            string playerItemsPath      = Path.Combine(saveFolderName, playerItemsName);
            string equippedItemsPath    = Path.Combine(saveFolderName, equippedItemsName);

            // 경로에 모든 파일들이 다 있을 때 불러오기. (하나라도 없으면 파일이 손상된 것으로 간주, 플레이어를 새로 생성한다.)
            if (File.Exists(playerDataPath) && File.Exists(playerItemsPath) && File.Exists(equippedItemsPath))
            {
                Console.WriteLine("데이터가 존재합니다.");
                // 플레이어 스테이터스
                string playerStatusData     = File.ReadAllText(playerDataPath);
                // 플레이어 보유장비
                string playerItemsIndex     = File.ReadAllText(playerItemsPath);
                // 플레이어 장착장비
                string playerEquippedIndex  = File.ReadAllText(equippedItemsPath);

                // 플레이어 스테이터스 파일을 역직렬화를 통해 Player 타입으로 반환.
                Player? loadedPlayer            = JsonSerializer.Deserialize<Player>(playerStatusData);
                // 불러온 플레이어는 스테이터스 정보만 가지고 있으므로 나머지 정보를 할당.
                // 1. 보유중인 Item ID 리스트를 같은 방법으로 List<int> 타입으로 반환.
                // 2. 장착중인 Item Type의 string, ID 딕셔너리를 같은 방법으로 Dictionary<string, int> 타입으로 반환.
                loadedPlayer.hasItems           = JsonSerializer.Deserialize<List<int>>(playerItemsIndex);
                loadedPlayer.equippedItemIndex  = JsonSerializer.Deserialize<Dictionary<string, int>>(playerEquippedIndex);
                // 불러온 정보들을 가지고 보유 및 장착중인 아이템을 불러오기.
                LoadPlayerItemsInfo(loadedPlayer, shop);

                return loadedPlayer;
            }
            else
            {
                Console.WriteLine("데이터가 없습니다.");
                return new Player(1, "르탄이", Define.PlayerClass.Worrior, 10, 5, 100, 100, 1500);
            }                
        }
        
        static void LoadPlayerItemsInfo(Player player, Shop shop)
        {
            // 이중 for문으로 보유중인 Item의 ID가 상점에 진열된 Item의 ID와 일치하는지 검사.
            // 일치하면 해당 아이템을 구매상태로 전환.
            for (int i = 0; i < shop.items.Length; i++)
            {
                for (int j = 0; j < player.hasItems.Count; j++)
                {
                    if (shop.items[i].ID == player.hasItems[j])
                        shop.Restore(player, shop.items[i]);
                }
            }

            string weaponKey = Define.ItemType.Weapon.ToString();
            string armorKey  = Define.ItemType.Armor.ToString();
            string amuletKey = Define.ItemType.Amulet.ToString();

            for (int i = 0; i < player.items.Count; i++)
            {
                if (player.equippedItemIndex.ContainsKey(weaponKey) && player.items[i] is Weapon)
                    if (player.items[i].ID == player.equippedItemIndex[weaponKey])
                        player.EquipOrUnequipItem(player.items[i]);

                if (player.equippedItemIndex.ContainsKey(armorKey) && player.items[i] is Armor)
                    if (player.items[i].ID == player.equippedItemIndex[armorKey])
                        player.EquipOrUnequipItem(player.items[i]);

                if (player.equippedItemIndex.ContainsKey(amuletKey) && player.items[i] is Amulet)
                    if (player.items[i].ID == player.equippedItemIndex[amuletKey])
                        player.EquipOrUnequipItem(player.items[i]);
            }
        }

        static void SavePlayer(Player player, string saveFolderName)
        {
            string playerDataName       = "PlayerData.json";
            string playerItemsName      = "PlayerItems.json";
            string equippedItemsName    = "EquippedItems.json";

            string playerDataPath       = Path.Combine(saveFolderName, playerDataName);
            string playerItemsPath      = Path.Combine(saveFolderName, playerItemsName);
            string equippedItemsPath    = Path.Combine(saveFolderName, equippedItemsName);

            // 플레이어 정보 저장
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true};
            string playerSaveData = JsonSerializer.Serialize(player, options);
            playerSaveData = Regex.Unescape(playerSaveData);
            File.WriteAllText(playerDataPath, playerSaveData);

            // 플레이어 보유 장비 정보 저장
            string playerItemsSaveData = JsonSerializer.Serialize(player.hasItems, options);
            playerItemsSaveData = Regex.Unescape(playerItemsSaveData);
            File.WriteAllText(playerItemsPath, playerItemsSaveData);

            // 플레이어 장착 장비 정보 저장
            string equippedItemsSaveData = JsonSerializer.Serialize(player.equippedItemIndex, options);
            equippedItemsSaveData = Regex.Unescape(equippedItemsSaveData);
            File.WriteAllText(equippedItemsPath, equippedItemsSaveData);

            Console.WriteLine("데이터가 저장되었습니다.");
        }
    }
}
