﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    internal class Monster
    {
        public string   Name    { get; private set; }
        public int      Lv      { get; private set; }        
        public int      Atk     { get; private set; }
        /// <summary>
        /// get 시 Hp 반환
        /// set 시 Hp = value 값 및 0 이하 시 Hp = 0, Dead 메서드 호출
        /// </summary>
        public float    Hp 
        {
            get => Hp;
            private set
            {                
                Hp = value;
                if(Hp <= 0)
                {
                    Hp = 0;
                    Dead();
                }
            }
        }

        public bool     IsDead  { get; private set; } = false;

        private Define.MonsterType _type;
        private readonly int[]      ATKs    = [10, 15, 25];
        private readonly float[]    HPs     = [30f, 20f, 10f];

        public Monster(int lv, Define.MonsterType type)
        {
            Lv      = lv;            
            _type   = type;

            switch (_type)
            {
                case Define.MonsterType.SkeletonWorrior:
                    Name = "스켈레톤 전사";
                    break;
                case Define.MonsterType.SkeletonArcher:
                    Name = "스켈레톤 궁수";
                    break;
                case Define.MonsterType.SkeletonWizard:
                    Name = "스켈레톤 마법사";
                    break;
            }

            Setup(_type);            
        }

        // _type 에 따라 Atk, Hp 셋업
        private void Setup(Define.MonsterType type)
        {
            Atk = ATKs[(int)type];
            Hp  = HPs[(int)type];
        }

        public void GetDamage(float damage) => Hp -= damage;

        public void Dead() => IsDead = true;
    }
}