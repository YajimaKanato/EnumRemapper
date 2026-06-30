using System;
using System.Collections.Generic;

namespace EnumRemapper.Runtime
{
    /// <summary>enumの情報を集めたクラス</summary>
    [Serializable]
    public class EnumCache
    {
        /// <summary>enumの情報を集めたリスト</summary>
        public List<EnumInfo> Enums = new();

        /// <summary>
        /// 指定したenumの情報を取得するメソッド
        /// </summary>
        /// <param name="fullName">取得したいenumの名前</param>
        /// <returns>enumの情報</returns>
        public EnumInfo Get(string fullName)
        {
            return Enums.Find(e => e.FullName == fullName);
        }
    }

    /// <summary>enumの情報を持つクラス</summary>
    [Serializable]
    public class EnumInfo
    {
        /// <summary>enumの名前</summary>
        public string FullName;

        /// <summary>enumに含まれる要素を集めたリスト</summary>
        public List<string> Names = new();
    }
}
