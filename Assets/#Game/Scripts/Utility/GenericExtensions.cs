
namespace STLExtensiton
{
    using System.Linq;

    /// <summary>
    /// ジェネリックの拡張メソッドを管理するクラス
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// オブジェクトが指定されたいずれかのオブジェクトと等しいかどうかを返します
        /// </summary>
        public static bool ContainsAny<T>(this T self, params T[] values)
        {
            return values.Any(c => c.Equals(self));
        }
    }
}
