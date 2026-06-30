using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using EnumRemapper.Runtime;

namespace EnumRemapper.Editor
{
    /// <summary>アセットの更新処理を持つクラス</summary>
    public static class AssetUpdater
    {
        /// <summary>
        /// アセットを走査して更新処理を行うメソッド
        /// </summary>
        /// <param name="oldCache">古いenumのキャッシュ</param>
        /// <param name="changedEnums">変更があったenumの集合</param>
        public static void Update(EnumCache oldCache, HashSet<string> changedEnums)
        {
            // すべてのアセットを取得
            // パスを変更することで探索範囲を制限できる
            string[] guids = AssetDatabase.FindAssets("");

            foreach (string guid in guids)
            {
                // パスからアセットを取得
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);

                if (obj == null)
                    continue;
                // アセットに更新処理を実行
                UpdateObject(obj, oldCache, changedEnums);
            }

            // アセットの保存
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// アセットの更新処理を実行するメソッド
        /// </summary>
        /// <param name="obj">更新するアセット</param>
        /// <param name="oldCache">古いキャッシュ</param>
        /// <param name="changedEnums">変更があったenumの集合</param>
        private static void UpdateObject(UnityEngine.Object obj, EnumCache oldCache, HashSet<string> changedEnums)
        {
            // アセットのSerializeFieldを見られるようにする
            SerializedObject so = new SerializedObject(obj);

            // アセット内のプロパティ（SerializeFiled）を順にみるためのイテレータを取得
            SerializedProperty iterator = so.GetIterator();

            // 値を変更したかどうかのフラグ
            bool changed = false;

            // アセット内のプロパティを走査
            while (iterator.NextVisible(true))
            {
                // enumでなければ飛ばす
                if (iterator.propertyType != SerializedPropertyType.Enum)
                    continue;

                // enumの型を取得
                Type enumType = PropertyTypeResolver.Resolve(obj.GetType(), iterator.propertyPath);

                // ちゃんとenumとして取得できたかを確認
                if (enumType?.IsEnum != true)
                    continue;

                // 変更があったenumでなければ飛ばす
                if (!changedEnums.Contains(enumType.FullName))
                    continue;

                // 変更前のenumの定義を取得
                List<string> oldInfo = oldCache.Get(enumType.FullName)?.Names;

                // 現在のenumの定義を取得
                string[] newNames = Enum.GetNames(enumType);

                // 現在アセットに保存されているenumのインデックス
                int oldIndex = iterator.intValue;

                // 名前から新しいインデックスに変換
                int newIndex = EnumRemapping.ConvertIndex(
                    oldInfo,
                    newNames.ToList(),
                    oldIndex);

                // インデックスが変わる場合のみ更新
                if (newIndex != iterator.intValue)
                {
                    iterator.intValue = newIndex;
                    changed = true;
                }
            }

            if (changed)
            {
                // 更新をアセットに反映
                so.ApplyModifiedProperties();

                // Unityに変更を知らせる
                EditorUtility.SetDirty(obj);
            }
        }
    }
}
