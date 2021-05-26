using System;
using System.Collections.Generic;

namespace Relikt_2_Models
{
    public class Page<T>
    {
        public int Size { get; set; }

        public int Number { get; set; }

        public IReadOnlyCollection<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount => (int)Math.Ceiling(TotalCount * 1.0 / Size) ;
    }
}
