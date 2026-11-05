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

                // 保存されている数値に合致するenumを取り出す
                var oldMember = oldCache.Get(enumType.FullName)?.Enums?
                    .Find(member => member.Value == iterator.intValue);

                // 該当するものがなければスルー
                if (oldMember == null || !Enum.IsDefined(enumType, oldMember.Name))
                    continue;

                // 新しいenumの値を取得
                var newValue = Convert.ToInt32(Enum.Parse(enumType, oldMember.Name));

                // インデックスが変わる場合のみ更新
                if (newValue != iterator.intValue)
                {
                    iterator.intValue = newValue;
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
