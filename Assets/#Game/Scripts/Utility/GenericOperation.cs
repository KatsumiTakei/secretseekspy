
//  @note   !<   https://qiita.com/Zuishin/items/61fc8807d027d5cea329
namespace STLExtensiton
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericOperation<T>
    {
        public GenericOperation()
        {
            var availableT = new Type[]
            {
            typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(byte),
            typeof(decimal), typeof(double)
            };
            if (!availableT.Contains(typeof(T)))
            {
                throw new NotSupportedException();
            }
            var p1 = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(T));
            Add = Expression.Lambda<Func<T, T, T>>(Expression.Add(p1, p2), p1, p2).Compile();
            Subtract = Expression.Lambda<Func<T, T, T>>(Expression.Subtract(p1, p2), p1, p2).Compile();
            Multiply = Expression.Lambda<Func<T, T, T>>(Expression.Multiply(p1, p2), p1, p2).Compile();
            Divide = Expression.Lambda<Func<T, T, T>>(Expression.Divide(p1, p2), p1, p2).Compile();
            Modulo = Expression.Lambda<Func<T, T, T>>(Expression.Modulo(p1, p2), p1, p2).Compile();
            Equal = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2).Compile();
            GreaterThan = Expression.Lambda<Func<T, T, bool>>(Expression.GreaterThan(p1, p2), p1, p2).Compile();
            GreaterThanOrEqual = Expression.Lambda<Func<T, T, bool>>(Expression.GreaterThanOrEqual(p1, p2), p1, p2).Compile();
            LessThan = Expression.Lambda<Func<T, T, bool>>(Expression.LessThan(p1, p2), p1, p2).Compile();
            LessThanOrEqual = Expression.Lambda<Func<T, T, bool>>(Expression.LessThanOrEqual(p1, p2), p1, p2).Compile();
        }

        public Func<T, T, T> Add { get; private set; }                  // +
        public Func<T, T, T> Subtract { get; private set; }             // -
        public Func<T, T, T> Multiply { get; private set; }             // *
        public Func<T, T, T> Divide { get; private set; }               // /
        public Func<T, T, T> Modulo { get; private set; }               // %
        public Func<T, T, bool> Equal { get; private set; }             // ==
        public Func<T, T, bool> GreaterThan { get; private set; }       // >
        public Func<T, T, bool> GreaterThanOrEqual { get; private set; }// >=
        public Func<T, T, bool> LessThan { get; private set; }          // <
        public Func<T, T, bool> LessThanOrEqual { get; private set; }   // <=
    }
}
