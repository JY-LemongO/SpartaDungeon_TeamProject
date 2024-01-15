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
    internal class Player : IDamagable
    {
        public int Lv                       { get; set; }
        public string Name                  { get; private set; }        
        public PlayerClass MyClass          { get; private set; }
        public float Atk                    { get; private set; }
        public int Def                      { get; private set; }
        public int Hp                       { get; private set; }
        public int MaxHp                    { get; private set; }
        public int Mp                       { get; set; }
        public int MaxMp                    { get; private set; }
        public int Gold                     { get; private set; }
        public int EXP                      { get; private set; }        
        public List<Skill> Skills           { get; protected set; }
        public List<int> Items              { get; private set; }
        public List<int> EquippedItemsIndex { get; private set; }
        public int Point                    { get; set; }        
        
        // 런타임에서 가지고 있을 장착 아이템 정보들
        public Dictionary<string, Item>       equippedItems = new Dictionary<string, Item>();

        public Player(int Lv, string Name, PlayerClass MyClass, float Atk, int Def, int MaxHp, int MaxMp, int Gold, int point, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null)
        {
            this.Lv = Lv;
            this.Name = Name;
            this.MyClass = MyClass;
            this.Atk = Atk;
            this.Def = Def;
            this.Hp = MaxHp;
            this.MaxHp = MaxHp;
            this.Mp = MaxMp;
            this.MaxMp = MaxMp;
            this.Gold = Gold;
            this.Point = Point;
            this.Skills = Skills;
            this.Items = Items;
            this.EquippedItemsIndex = EquippedItemsIndex;

            if (this.Items == null)
                this.Items = new List<int>();
            if (this.EquippedItemsIndex == null)
                this.EquippedItemsIndex = new List<int>();
        }   

        public void BuyOrSell(int price, Item item, bool isSell = false)
        {            
            Gold += price;
            // 구매 시
            if (!isSell)
            {
                Items.Add(item.ID);
            }
            else
            {
                // 만약 판매하는 아이템이 장착 중이면 장착해제를 먼저 한다.
                if (item.IsEquip)
                    EquipOrUnequipItem(item);
                // 플레이어 아이템 리스트에서 제거한다.
                Items.Remove(item.ID);
            }
        }

        public void EquipOrUnequipItem(Item item)
        {
            // item의 Type이(Weapon or Armor) 이미 있을 때 = 뭔가 장착 중일 때
            if (equippedItems.ContainsKey(item.TypeName))
            {
                // 동일 아이템이면
                // 단순 장착해제
                if (equippedItems[item.TypeName].ID == item.ID)
                    UnequipItem(item);
                // 동일 아이템이 아니면
                // 현재 아이템 장착해제 후 선택한 아이템 장착
                else
                {
                    UnequipItem(equippedItems[item.TypeName]);
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

            EquippedItemsIndex.Add(item.ID);
            equippedItems[item.TypeName] = item;
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

            EquippedItemsIndex.Remove(item.ID);
            equippedItems.Remove(item.TypeName);
        }

        public (bool, bool, float) CalculateExDamage(float originDamage, bool isSkill)
        {
            // 입력: 원래의 데미지, 스킬사용여부
            // 반환: 치명타 성공 여부, 회피 여부, 실 데미지

            // 반환값 목록 선언 및 초기화
            bool isCritical = false;
            bool isDodged = false;
            float calculatedDamage = originDamage;

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
                isCritical = false;
                calculatedDamage = 0; // 0의 데미지
            }

            return (isCritical, isDodged, calculatedDamage);
            // (bool val1, bool val2, int val3) = player.CalculateExDamage(originDamage, isSkill); 와 같이 사용
        }

        public void GetGold(int gold) => Gold += gold;

        public void Rest() => Hp = MaxHp;

        public void Meditate() => Mp = MaxMp;

        public void SetHp(int hp) => Hp = hp;

        public void GetDamage(float damage)
        {
            Hp -= (int)damage;
            if(Hp < 0)
                Hp = 0;
        }

        public void StatUp()
        {
            Atk += 0.5f;
            Def += 1;
        }

        public virtual void CreateSkills()
        {
            Skills = new List<Skill>()
            {
                new Skill(this, "힘없는 휘두르기",10, 1, "공격력 * 1 로 하나의 적을 공격합니다.", 1),
                new Skill(this, "별반 차이 없는 휘두르기", 10, 1, "그냥 때리는게 마나도 안들고 좋아보입니다.", 1)
            };
    }

        public void ShowText()
        {
            Console.WriteLine($"\n" +
                $"[내정보]\n" +
                $"Lv.{Lv}\t{Name}\n" +
                $"HP {Hp} / {MaxHp}\n\n");
        }

        public virtual string GetClassName() { return "잘못된 접근"; }
    }

    #region 하위 직업들
    internal class Warrior : Player
    {
        public Warrior(string name, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null) : base(1, name, PlayerClass.Warrior, 10, 5, 100, 100, 1500, 0, Skills, Items, EquippedItemsIndex)
        {
            CreateSkills();
        }

        public override void CreateSkills()
        {
            Skills = new List<Skill>() 
            {
                new Skill(this, "알파 스트라이크", 10, 2, "하나의 적을 공격합니다. / 공격력 * 2", 1),
                new Skill(this, "짱센 스트라이크", 20, 1, "둘의 적을 랜덤으로 공격합니다. / 공격력 * 1", 2)
            };
        }

        public override string GetClassName() { return "전사"; }
    }

    internal class Archer : Player
    {
        public Archer(string name, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null) : base(1, name, PlayerClass.Archer, 12, 3, 90, 100, 1500, 0, Skills, Items, EquippedItemsIndex)
        {
            CreateSkills();
        }

        public override void CreateSkills()
        {
            Skills = new List<Skill>()
            {
                new Skill(this, "급소 조준", 10, 2, "급소에 화살을 꽂습니다. / 공격력 * 2", 1),
                new Skill(this, "몰아치는 화살", 20, 1, "3명의 적을 공격합니다. / 공격력 * 1", 3)
            };
        }

        public override string GetClassName() { return "궁수"; }
    }

    internal class Magic : Player
    {
        public Magic(string name, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null) : base(1, name, PlayerClass.Magic, 10, 4, 80, 120, 1500, 0, Skills, Items, EquippedItemsIndex)
        {
            CreateSkills();
        }

        public override void CreateSkills()
        {
            Skills = new List<Skill>()
            {
                new Skill(this, "마법 화살", 10, 2, "마력 화살 하나를 날립니다. / 공격력 * 2", 1),
                new Skill(this, "번개 사슬", 35, 2, "3명의 적에게 전기 피해룰 줍니다. / 공격력 * 2", 3)
            };
        }

        public override string GetClassName() { return "마법사"; }
    }

    internal class Thief : Player
    {
        public Thief(string name, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null) : base(1, name, PlayerClass.Thief, 11, 4, 80, 100, 1500, 0, Skills, Items, EquippedItemsIndex)
        {
            CreateSkills();
        }

        public override void CreateSkills()
        {
            Skills = new List<Skill>()
            {
                new Skill(this, "힘줄 자르기", 10, 2, "강하게 공격합니다. / 공격력 *2", 1),
                new Skill(this, "암습", 30, 4, "뒤에서 급소를 공격합니다. / 공격력 * 4", 1)
            };
    }

        public override string GetClassName() { return "도둑"; }
    }

    internal class Deadbeat : Player
    {
        public Deadbeat(string name, List<Skill> Skills = null, List<int> Items = null, List<int> EquippedItemsIndex = null) : base(1, name, PlayerClass.Deadbeat, 6, 5, 100, 100, 500, 0, Skills, Items, EquippedItemsIndex)
        {
            CreateSkills();
        }

        public override string GetClassName() { return "무직백수"; }
    }
    #endregion
}
