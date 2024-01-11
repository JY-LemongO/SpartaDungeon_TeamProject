using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RtanTextDungeon.Define;

namespace RtanTextDungeon
{
    internal static class DungeonInfo
    {
        public static int HighestFloor { get; private set; } = 1;
        public static int CurrentFloor { get; private set; }

        public static void UpdateInfo()
        {
            // 현재 선택한 층이 최고도달 층일 때, 최고층 갱신

            if (HighestFloor == CurrentFloor)
                HighestFloor++;            
        }

        private static int SpawnCount()
        {         
            // 5층 단위로 최대 스폰 1 ~ 5마리 
            // 최소 스폰 1마리, 최대 스폰이 3마리 이상일 땐 2마리

            int maxCount = CurrentFloor % 5 == 0 ? 5 : CurrentFloor % 5;
            int minCount = maxCount > 3 ? 2 : 1 ;
            Random rnd = new Random();            
            return rnd.Next(minCount, maxCount + 1);
        }

        private static (int, MonsterType) MonsterInfo()
        {
            Random rnd = new Random();

            // 최소 레벨 : 1 * [x]층
            // 최대 레벨 : 3 * [x]층 - [x]층
            // 예) 1층 : 최소/최대 = 1 / 2
            // 예) 4층 : 최소/최대 = 4 / 8

            int minLv = 1 * CurrentFloor;
            int maxLv = 3 * CurrentFloor - CurrentFloor;            
            int randLv = rnd.Next(minLv, maxLv + 1);

            // 최소 타입 : 스켈레톤 워리어
            // 최고 타입 : 5층 마다 3가지 타입 추가 / 초과 시 현재 존재하는 마지막 타입
            // 예) 3층 : 최소/최대 = 0(스켈레톤 워리어) / 3 - 1(스켈레톤 마법사)
            // 예) 6층 : 최소/최대 = 0(스켈레톤 워리어) / 6 - 1(고블린 마법사)
            // 예) 12층 : 초소/최대 = 0(스켈레톤 워리어) / 12 - 1(목록에 없음) >> (고블린 마법사)

            int minType = 0;
            int maxType = 3 + CurrentFloor / 5 * 3;            
            int randType = rnd.Next(minType, Math.Min(maxType, Enum.GetValues(typeof(MonsterType)).Length));

            return (randLv, (MonsterType)randType);
        }

        public static Monster[] MonsterSpawn(int floor)
        {            
            CurrentFloor = floor;

            // 몬스터 생성

            int spawnCount = SpawnCount();
            Monster[] monsters = new Monster[spawnCount];

            for (int i = 0; i < monsters.Length; i++)
                monsters[i] = new Monster(MonsterInfo().Item1, MonsterInfo().Item2);

            return monsters;
        }
    }
}
