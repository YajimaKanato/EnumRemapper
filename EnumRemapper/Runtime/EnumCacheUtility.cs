using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace EnumRemapping
{
    /// <summary>enumのキャッシュに関する処理を持つクラス</summary>
    public static class EnumCacheUtility
    {
        /// <summary>保存先パス</summary>
        private const string CachePath = "ProjectSettings/EnumCache.json";

        /// <summary>
        /// 現在のすべてのenumの情報を作成するメソッド
        /// </summary>
        /// <returns>すべてのenumの情報</returns>
        public static EnumCache CreateCurrentCache()
        {
            // enumの情報を集めるクラスを作成
            EnumCache cache = new();

            // 現在実行中のアプリケーションからAssemblyを取得
            // EnumRemapping属性がついたenumを集める
            var enumTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsEnum && t.IsDefined(typeof(EnumRemappingAttribute), false));

            // 集めたenumの情報をキャッシュに登録
            foreach (var type in enumTypes)
            {
                cache.Enums.Add(new EnumInfo
                {
                    FullName = type.FullName,
                    Names = Enum.GetNames(type).ToList()
                });
            }

            return cache;
        }

        /// <summary>
        /// 現在保存されているキャッシュを取得するメソッド
        /// </summary>
        /// <returns>現在保存されているキャッシュ</returns>
        public static EnumCache Load()
        {
            // キャッシュがなければ空のキャッシュを返す
            if (!File.Exists(CachePath))
                return new EnumCache();

            // JSONの中身を読み、キャッシュに変換して返す
            string json = File.ReadAllText(CachePath);
            return JsonUtility.FromJson<EnumCache>(json);
        }

        /// <summary>
        /// キャッシュを保存するメソッド
        /// </summary>
        /// <param name="cache">保存するキャッシュ</param>
        public static void Save(EnumCache cache)
        {
            // キャッシュをJSON形式に変換して保存
            string json = JsonUtility.ToJson(cache, true);
            File.WriteAllText(CachePath, json);
        }
    }
}
