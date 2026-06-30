using System;
using System.Collections.Generic;
using System.Linq;
using EnumRemapper.Runtime;

namespace EnumRemapper.Editor
{
    /// <summary>変更されたenumがあるかどうかを判定するクラス</summary>
    public static class EnumComparer
    {
        /// <summary>
        /// 変更されたenumを取得するメソッド
        /// </summary>
        /// <param name="oldCache">古いenumのキャッシュ</param>
        /// <param name="newCache">新しいenumのキャッシュ</param>
        /// <returns>変更されたenumの集合</returns>
        public static HashSet<string> GetChangedEnums(EnumCache oldCache, EnumCache newCache)
        {
            // 変更されたenumの名前を集めるためのコレクション
            HashSet<string> changed = new();

            // 新しいキャッシュがなければ変更なしで終了
            if (newCache == null) return changed;

            // 新しいキャッシュのenumを順に比較
            foreach (var cache in newCache.Enums)
            {
                // enumの情報を取得
                EnumInfo oldInfo = oldCache.Get(cache.FullName);

                // 新しいenumの情報がなければ飛ばす
                if (oldInfo == null)
                    continue;

                // enumの定義が変わっていた場合はコレクションに追加
                if (!oldInfo.Names.SequenceEqual(cache.Names))
                {
                    changed.Add(cache.FullName);
                }
            }

            return changed;
        }
    }
}