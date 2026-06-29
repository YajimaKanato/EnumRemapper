using System;
using System.Reflection;

namespace EnumRemapping
{
    /// <summary>SerializedPropertyから実際の型を取得する処理を持つクラス</summary>
    public static class PropertyTypeResolver
    {
        /// <summary>
        /// propertyPathから最終的なフィールドの型（enum）を取得するメソッド
        /// </summary>
        /// <param name="rootType">探索を開始するフィールドの型</param>
        /// <param name="propertyPath">SerializedPropertyのパス</param>
        /// <returns>enumの型もしくはnull</returns>
        public static Type Resolve(Type rootType, string propertyPath)
        {
            // 探索するデータ型
            Type currentType = rootType;

            // パスを分解
            string[] elements = propertyPath.Split('.');

            // パスを順に探索
            for (int i = 0; i < elements.Length; i++)
            {
                // 現在見ているプロパティの情報
                string element = elements[i];

                // List・配列
                if (element == "Array")
                {
                    // data[x] を飛ばす
                    i++;

                    // 配列・リストの要素のデータ型を取得
                    // 配列かリストかで次に見る情報の取得方法を変える
                    if (currentType.IsArray)
                    {
                        currentType = currentType.GetElementType();
                    }
                    else
                    {
                        currentType = currentType.GetGenericArguments()[0];
                    }

                    // 欲しい型はArrayな型ではないので次へ
                    continue;
                }

                // 現在のPropertyPathからフィールドの情報を取得
                FieldInfo field = GetField(currentType, element);

                // フィールドがなければ終了
                if (field == null)
                    return null;

                // 次の型を調べる
                currentType = field.FieldType;
            }

            return currentType;
        }

        /// <summary>
        /// 継承元も含めてフィールド情報を取得するメソッド
        /// </summary>
        /// <param name="type">フィールドを調べる型</param>
        /// <param name="name">フィールド名</param>
        /// <returns>フィールドの情報</returns>
        private static FieldInfo GetField(Type type, string name)
        {
            while (type != null)
            {
                // 条件にあるフィールド情報を取得
                // インスタンスフィールド・publicフィールド・private, protectedフィールドを見つける
                FieldInfo field = type.GetField(
                    name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

                // 取得出来たら終了
                if (field != null)
                    return field;

                // 継承元も調べる
                type = type.BaseType;
            }

            return null;
        }
    }
}
