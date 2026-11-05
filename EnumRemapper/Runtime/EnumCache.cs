using System;
using System.Collections.Generic;

namespace EnumRemapper.Runtime
{
    /// <summary>enumの情報を集めたクラス</summary>
    [Serializable]
    public class EnumCache
    {
        /// <summary>バージョン管理用数値</summary>
        public const int CurrentVersion = 1;

        /// <summary>現在のバージョン</summary>
        public int Version;

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
        public List<EnumMemberInfo> Enums = new();
    }

    /// <summary>enumの情報を保持するクラス</summary>
    [Serializable]
    public class EnumMemberInfo
    {
        /// <summary>enumの名前</summary>
        public string Name;

        /// <summary>enumで割り当てられている値</summary>
        public int Value;
    }
}
