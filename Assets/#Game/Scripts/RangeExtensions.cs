
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public static class RangeExtensions
{
    //public static RangeEnumerator GetEnumerator(this Range range)
    //{
    //    if (range.Start.IsFromEnd || range.End.IsFromEnd)
    //    {
    //        throw new NotSupportedException("^ index is not supported");
    //    }

    //    return new RangeEnumerator(range.Start.value, range.End.value, range.Start.value > range.End.value);
    //}

    public struct RangeEnumerator : IEnumerator<int>
    {
        readonly int end;
        readonly bool reverse;
        int current;

        public RangeEnumerator(int start, int end, bool reverse)
        {
            this.end = end;
            this.current = (reverse) ? start + 1 : start -1;
            this.reverse = reverse;
        }

        public int Current => current;

        object IEnumerator.Current => current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if(reverse)
            {
                if(current > end)
                {
                    current--;
                    return true;
                }
            }
            else
            {
                if (current < end)
                {
                    current++;
                    return true;
                }
            }

            return false;
        }


        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }



}
