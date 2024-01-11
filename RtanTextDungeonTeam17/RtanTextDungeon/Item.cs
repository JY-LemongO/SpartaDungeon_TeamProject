﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Item
    {
        public int      ID              { get; protected set; }        
        public string   Name            { get; protected set; }
        public string   AdditionalATK   { get; protected set; }
        public string   AdditionalDEF   { get; protected set; }        
        public string   AbilityName     { get; }
        public string   Desc            { get; }        
        public int      Price           { get; }
        public bool     IsEquip         { get; protected set; } = false;
        public bool     IsBuy           { get; protected set; } = false;

        public Item(int id, string name, string abilityName, string desc, int price)
        {
            ID = id;
            Name = name;
            AbilityName = abilityName;
            Desc = desc;
            Price = price;            
        }

        public void GetItem() => IsBuy = true;

        public void RemoveItem() => IsBuy = false;

        public virtual void EquipItem()
        {            
            Name = "[E]" + Name;
            IsEquip = true;
        }

        public virtual void UnequipItem() 
        {
            string subString = "[E]";            
            int index = Name.IndexOf(subString);
            Name = Name.Remove(index, subString.Length);
            IsEquip = false;
        }          
    }

    class Weapon : Item
    {
        public int damage { get; private set; }

        public override void EquipItem()
        {            
            AdditionalATK = $"(+{damage})";
            base.EquipItem();
        } 
        public override void UnequipItem()
        {
            AdditionalATK = $"";
            base.UnequipItem();
        }

        public Weapon(int id, string name, string desc, int price, int damage) : base(id, name, "공격력", desc, price)
        {           
            this.damage = damage;            
        }
    }

    class Armor : Item
    {
        public int defense { get; private set; }
        public override void EquipItem()
        {            
            AdditionalDEF = $"(+{defense})";
            base.EquipItem();
        }
        public override void UnequipItem()
        {
            AdditionalDEF = $"";
            base.UnequipItem();
        }

        public Armor(int id, string name, string desc, int price, int defense) : base(id, name, "방어력", desc, price)
        {
            this.defense = defense;            
        }
    }

    class Amulet : Item
    {
        public int damage { get; private set; }
        public int defense { get; private set; }

        public override void EquipItem()
        {
            AdditionalATK = $"(+{damage})";
            AdditionalDEF = $"(+{defense})";
            base.EquipItem();
        }
        public override void UnequipItem()
        {
            AdditionalATK = $"";
            AdditionalDEF = $"";
            base.UnequipItem();
        }

        public Amulet(int id, string name, string desc, int price, int damage, int defense) : base(id, name, "공격력/방어력", desc, price)
        {
            this.damage = damage;
            this.defense = defense;
        }
    }

    class Potion : Item
    {
        public int heal { get; private set; }
        public int count { get; private set; }
        public Potion(int id, string name, string desc, int price, int heal) : base(id, name, "체력회복", desc, price)
        {
            this.heal = heal;
            this.count = 3;
        }
        public void Use() => this.count--;
        public bool Use(Player player)
        {
            // 입력: (Player)플레이어 객체
            // 출력: (bool)회복여부
            if (player.Hp == player.MaxHp) return false;
            if (this.count <= 0) return false;

            this.count--;
            player.SetHp(Math.Min(player.Hp + heal, player.MaxHp));
            return true;
        }
        public void Get() => this.count++;
        public void Get(int n) => this.count += n;
        public int Get(int min, int max)
        {
            // 입력: (int, int)랜덤 획득 범위
            // 출력: (int)획득한 포션 갯수
            int n = new Random().Next(Math.Min(min,max), Math.Max(min, max) + 1);
            this.count += n;
            return n;
        }
    }

}
