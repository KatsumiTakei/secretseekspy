namespace STLExtensiton
{
    using System.Collections.Generic;

    public class Loop
    {
        private int value, min, max;

        public void Reset(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void Reset(int value, int min, int max)
        {
            this.value = value;
            this.min = min;
            this.max = max;
        }

        #region constructor

        public Loop(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public Loop(int value, int min, int max)
        {
            this.value = value;
            this.min = min;
            this.max = max;
        }

        #endregion constructor

        #region operator


        #region Cast
        public static implicit operator int(Loop self)
        {
            return self.value;
        }

        public static implicit operator uint(Loop self)
        {
            return (uint)self.value;
        }

        public static implicit operator short(Loop self)
        {
            return (short)self.value;
        }

        public static implicit operator double(Loop self)
        {
            return (double)self.value;
        }

        public static implicit operator ushort(Loop self)
        {
            return (ushort)self.value;
        }

        public static implicit operator long(Loop self)
        {
            return (long)self.value;
        }

        public static implicit operator ulong(Loop self)
        {
            return (ulong)self.value;
        }

        public static implicit operator byte(Loop self)
        {
            return (byte)self.value;
        }

        public static implicit operator decimal(Loop self)
        {
            return (decimal)self.value;
        }
        #endregion Cast

        #region LeftEval

        public static Loop operator +(Loop self, int other)
        {
            self.value += other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator -(Loop self, int other)
        {
            self.value -= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator *(Loop self, int other)
        {
            self.value *= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator /(Loop self, int other)
        {
            self.value /= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator %(Loop self, int other)
        {
            self.value %= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static bool operator <(Loop self, int other)
        {
            return (self.value.CompareTo(other) < 0);
        }

        public static bool operator >(Loop self, int other)
        {
            return (self.value.CompareTo(other) > 0);
        }
        public static bool operator <=(Loop self, int other)
        {
            return (self.value.CompareTo(other) <= 0);
        }

        public static bool operator >=(Loop self, int other)
        {
            return (self.value.CompareTo(other) >= 0);
        }

        public static bool operator ==(Loop self, int other)
        {
            return self.value == other;
        }

        public static bool operator !=(Loop self, int other)
        {
            return self.value != other;
        }

        #endregion LeftEval
        #region RightEval

        public static Loop operator +(int other, Loop self)
        {
            self.value += other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator -(int other, Loop self)
        {
            self.value -= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator *(int other, Loop self)
        {
            self.value *= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator /(int other, Loop self)
        {
            self.value /= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static Loop operator %(int other, Loop self)
        {
            self.value %= other;
            if (self.value > self.max)
                self.value = self.min;
            else if (self.value < self.min)
                self.value = self.max;

            return self;
        }

        public static bool operator <(int other, Loop self)
        {
            return other < self.value;
        }

        public static bool operator >(int other, Loop self)
        {
            return other > self.value;
        }

        public static bool operator <=(int other, Loop self)
        {
            return other <= self.value;
        }

        public static bool operator >=(int other, Loop self)
        {
            return other >= self.value;
        }


        public static bool operator ==(int other, Loop self)
        {
            return self.value == other;
        }

        public static bool operator !=(int other, Loop self)
        {
            return self.value != other;
        }
        #endregion RightEval


        public override bool Equals(object obj)
        {
            var loop = obj as Loop;
            return loop != null && value == loop.value;
        }

        public override int GetHashCode()
        {
            return -1584136870 + value.GetHashCode();
        }

        public override string ToString()
        {
            return value.ToString();
        }

        #endregion operator
    }
}