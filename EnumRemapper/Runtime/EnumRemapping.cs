using System.Collections.Generic;
using UnityEngine;

namespace EnumRemapping
{
    /// <summary>enumのRemap処理を持つクラス</summary>
    public static class EnumRemapping
    {
        /// <summary>
        /// enumをインデックスに変換するメソッド
        /// </summary>
        /// <param name="oldNames">古いenumの情報</param>
        /// <param name="newNames">新しいenumの情報</param>
        /// <param name="oldIndex">古いインデックス</param>
        /// <returns>新しいインデックス</returns>
        public static int ConvertIndex(
            List<string> oldNames,
            List<string> newNames,
            int oldIndex)
        {
            // enumの情報が何もなければ古いインデックスを返す
            if (oldNames == null || newNames == null)
                return oldIndex;

            // 古いインデックスが範囲外なら古いインデックスを返す
            if (oldIndex < 0 || oldIndex >= oldNames.Count)
                return oldIndex;

            // 古いインデックスに該当するenumの名前を取得
            string oldName = oldNames[oldIndex];

            // 新しいインデックスを取得
            int newIndex = newNames.IndexOf(oldName);

            // 選択していた古いenumが新しいenumに含まれていたら新しいインデックスを返す
            if (newIndex >= 0)
                return newIndex;

            // 選択していた古いenumが新しいenumに含まれていなかったら
            // 最初の要素か最後の要素を返す
            return Mathf.Clamp(oldIndex, 0, newNames.Count - 1);
        }
    }
}
