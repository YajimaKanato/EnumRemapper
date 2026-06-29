using System;
using UnityEditor;
using UnityEngine;

namespace EnumRemapping
{
    /// <summary>EnumRemappingの実行処理を持つクラス</summary>
    public static class EnumRemappingRunner
    {
        /// <summary>
        /// 初期化時に呼ばれるメソッド
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            // コンパイルのたびに実行
            EditorApplication.delayCall += Execute;
        }

        /// <summary>
        /// EnumRemappingを実行するメソッド
        /// </summary>
        [MenuItem("Tools/Enum Remap/Update")]
        public static void Execute()
        {
            // 現在のキャッシュを取得
            EnumCache cache = EnumCacheUtility.Load();

            // 現在のenumの情報を新規キャッシュとして作成
            EnumCache newCache = EnumCacheUtility.CreateCurrentCache();

            if (cache.Enums.Count == 0)
            {
                // 古いキャッシュが存在しなければ新しいキャッシュを保存して終了
                EnumCacheUtility.Save(newCache);
                return;
            }

            // 変更されたenum取得
            var changedEnums = EnumComparer.GetChangedEnums(cache, newCache);

            // 変更されたenumがなければ終了
            if (changedEnums.Count == 0) return;

            try
            {
                // 変更したenumを使っているアセットにenumの値参照の復元処理
                AssetUpdater.Update(cache, changedEnums);

                // 新しいキャッシュを保存
                EnumCacheUtility.Save(newCache);

                Debug.Log("Enum Remapping Complete");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
