using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static RtanTextDungeon.Define;

namespace RtanTextDungeon
{    
    internal class Player
    {
        public int Lv                       { get; private set; }
        public string Name                  { get; private set; }        
        public PlayerClass m_Class          { get; private set; }
        public int Atk                      { get; private set; }
        public int Def                      { get; private set; }
        public int Hp                       { get; private set; }
        public int MaxHp                    { get; private set; }
        public int Mp                       { get; set; }
        public int MaxMp                    { get; private set; }
        public int Gold                     { get; private set; }
        public int EXP                      { get; private set; }        
        public List<Skill> Skills           { get; private set; }

        // 런타임에서 가지고 있을 아이템 정보들
        public Dictionary<Type, Item>   equippedItems       = new Dictionary<Type, Item>();        
        public List<Item>               items               = new List<Item>();

        // 저장시 가지고 있을 아이템 ID
        public Dictionary<string, int>  equippedItemIndex   = new Dictionary<string, int>();
        public List<int>                hasItems            = new List<int>();

        public int NeedEXP { get; private set; } = 10;
        
        public Player(int Lv, string Name, PlayerClass m_Class, int Atk, int Def, int MaxHp, int MaxMp, int Gold)
        {
            this.Lv = Lv;
            this.Name = Name;
            this.m_Class = m_Class;
            this.Atk = Atk;
            this.Def = Def;
            this.Hp = MaxHp;
            this.MaxHp = MaxHp;
            this.Mp = MaxMp;
            this.MaxMp = MaxMp;
            this.Gold = Gold;

            //스킬 생성용 임시 함수
            CreateSkills();
        }
        
        public void BuyOrSell(int price, Item item, bool isSell = false)
        {            
            Gold += price;
            // 구매 시
            if (!isSell)
            {
                // 불러오기 시 hasItems에 포함된 ID인지 체크없이 Add 하게되면 무한루프에 빠지게된다.
                if (!hasItems.Contains(item.ID))
                    // 아이템의 ID를 리스트에 추가
                    hasItems.Add(item.ID);
                // 플레이어 아이템 리스트에 추가
                items.Add(item);
            }
            else
            {
                // 해당 아이템의 ID를 리스트에서 제거
                hasItems.Remove(item.ID);
                // 만약 판매하는 아이템이 장착 중이면 장착해제를 먼저 한다.
                if (item.IsEquip)
                    EquipOrUnequipItem(item);
                // 플레이어 아이템 리스트에서 제거한다.
                items.Remove(item);
            }
        }

        public void EquipOrUnequipItem(Item item)
        {
            // item의 Type이(Weapon or Armor) 이미 있을 때 = 뭔가 장착 중일 때
            if (equippedItems.ContainsKey(item.GetType()))
            {
                // 동일 아이템이면
                // 단순 장착해제
                if (equippedItems[item.GetType()].ID == item.ID)
                    UnequipItem(item);
                // 동일 아이템이 아니면
                // 현재 아이템 장착해제 후 선택한 아이템 장착
                else
                {
                    UnequipItem(equippedItems[item.GetType()]);
                    EquipItem(item);
                }
            }
            // 장착중인 아이템이 없을 때
            else
                EquipItem(item);
        }

        private void EquipItem(Item item)
        {
            // 아이템 타입의 item 변수가 세부타입 Weapon/Armor/Amulet 에 따라 보정수치 증가
            switch (item)
            {
                case Weapon weapon:
                    weapon.EquipItem();
                    Atk += weapon.damage;
                    break;
                case Armor armor:
                    armor.EquipItem();
                    Def += armor.defense;
                    break;
                case Amulet amulet:
                    amulet.EquipItem();
                    Atk += amulet.damage;
                    Def += amulet.defense;
                    break;
            }

            // 불러오기 시 장착중인 아이템이 저장 되어있으면 키 중복으로 에러발생.
            if (!equippedItemIndex.ContainsKey(item.GetType().Name))
                // 장착중인 아이템 인덱스 딕셔너리에 ID 추가 (예 : 키 = "Weapon", 값 = 1)
                equippedItemIndex.Add(item.GetType().Name, item.ID);
            equippedItems[item.GetType()] = item;            
        }

        private void UnequipItem(Item item)
        {
            switch (item)
            {
                case Weapon weapon:
                    weapon.UnequipItem();
                    Atk -= weapon.damage;
                    break;
                case Armor armor:
                    armor.UnequipItem();
                    Def -= armor.defense;
                    break;
                case Amulet amulet:
                    amulet.UnequipItem();
                    Atk -= amulet.damage;
                    Def -= amulet.defense;
                    break;
            }

            equippedItemIndex.Remove(item.GetType().Name);
            equippedItems.Remove(item.GetType());
        }

        public (bool, bool, int) CalculateExDamage(int originDamage, bool isSkill)
        {
            // 입력: 원래의 데미지, 스킬사용여부
            // 반환: 치명타 성공 여부, 회피 여부, 실 데미지

            // 반환값 목록 선언 및 초기화
            bool isCritical = false;
            bool isDodged = false;
            int calculatedDamage = originDamage;

            // 치명타 계산
            Random random = new Random();
            double chance = random.NextDouble();

            if (chance <= 0.15) // 15% 의 확률로 치명타 발생
            {
                isCritical = true;
                calculatedDamage = (int)(originDamage * 1.6f); // 160% 데미지
            }

            // 회피 계산
            chance = random.NextDouble();

            if (chance <= 0.1 && !isSkill) // 스킬공격이 아닐 시, 10% 의 확률로 회피
            {
                isDodged = true;
                calculatedDamage = 0; // 0의 데미지
            }

            return (isCritical, isDodged, calculatedDamage);
            // (string val1, string val2, int val3) = player.CalculateExDamage(originDamage, isSkill); 와 같이 사용
        }

        public void GetGold(int gold) => Gold += gold;

        public void Rest() => Hp = MaxHp;

        public void GetDamage(int damage)
        {
            Hp -= damage;
            if(Hp < 0)
                Hp = 0;
        }        

        // 레벨업 체크
        public bool IsLevelUp(int exp)
        {
            // 매개변수로 받은 경험치 만큼 플레이어 경험치 증가.
            EXP += exp;
            if(EXP >= NeedEXP)
            {
                // 레벨업에 필요한 경험치 보다 크거나 같으면 레벨업
                // 레벨업 시 남은 경험치를 매개변수로 재귀호출, 다중레벨업 가능                
                LevelUp();
                int remainExp = EXP - NeedEXP;
                NeedEXP *= 2;
                EXP = 0;
                IsLevelUp(remainExp);
                return true;
            }
            return false;
        }

        private void LevelUp()
        {            
            Lv++;
            Atk += 3;
            Def += 1;            
        }

        public void CreateSkills()
        {
            Skills = new List<Skill>();

            Skill skill_1 = new Skill("알파 스트라이크", 10, 2, "공격력 * 2 로 하나의 적을 공격합니다.");
            Skill skill_2 = new Skill("짱센 스트라이크", 15, 3, "공격력 * 3 로 하나의 적을 랜덤으로 공격합니다.");
            
            Skills.Add(skill_1);
            Skills.Add(skill_2);
        }

        public void ShowText()
        {
            Console.WriteLine($"\n" +
                $"[내정보]\n" +
                $"Lv.{Lv}\t{Name}\n" +
                $"HP {Hp} / {MaxHp}\n\n");
        }
    }    
}
