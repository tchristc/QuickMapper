using System;
using System.Collections.Generic;

namespace QuickMapper
{
    public class Mapper
    {
        public Dictionary<Type, Type> MapTypes { get; set; } = new Dictionary<Type, Type>();
    }
}
