using System;
using System.Collections;
using System.Collections.Generic;

namespace SRMAgreement.Class
{
    public class _2D
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string NameGroup { get; set; }
        public string? PIBS { get; set; }
        public string address { get; set; }
        public double? area { get; set; }
        public string? rent { get; set; }

        public bool? isAlert { get; set; }
        public DateTime? DateCloseDepartment { get; set; }

        public int? SubleaseDopId { get; set; }
        public SubleaseDop? SubleaseDop { get; set; }
    }

    public class _2E : IEnumerable<_2D>
    {
        private List<_2D> _D2List;
        public _2E()
        {
            _D2List = new List<_2D>();
        }
        public _2E(List<_2D> D2List)
        {
            _D2List = D2List;
        }

        public IEnumerator<_2D> GetEnumerator()
        {
            return new _2DEnum(_D2List);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class _2DEnum : IEnumerator<_2D>
    {
        private readonly List<_2D> _D2List;
        private int position = -1;

        public _2DEnum(List<_2D> list)
        {
            _D2List = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _D2List.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        public _2D Current
        {
            get
            {
                try
                {
                    return _D2List[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }


        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
        }
    }
}
