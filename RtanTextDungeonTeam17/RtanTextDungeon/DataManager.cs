using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RtanTextDungeon
{
    internal static class DataManager
    {
        private static readonly string? DIRECTORY_NAME   = Path.GetDirectoryName(Directory.GetCurrentDirectory());        
        
        #region Save
        // 게임 세이브 - T 타입 정보 저장
        public static void SaveGame<T>(T data, string fileName)
        {            
            string savePath = Path.Combine(DIRECTORY_NAME, fileName);
            string dataFile = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            dataFile = Regex.Unescape(dataFile);

            File.WriteAllText(savePath, dataFile);
        }
        #endregion

        #region Load
        // 게임 로드 - T타입 정보 로드
        public static T? LoadGame<T>(string fileName)
        {
            string savePath = Path.Combine(DIRECTORY_NAME, fileName);

            if (!File.Exists(savePath))
                return default;

            string data = File.ReadAllText(savePath);
            T? type = JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions() { IncludeFields = true });

            return type;
        }
        #endregion
    }
}
