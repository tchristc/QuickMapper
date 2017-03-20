using System;

namespace QuickMapper
{
    public class StaticMapper : Mapper
    {
        public void Config(Type domain, Type model)
        {
            MapTypes[domain] = model;
        }
    }
}
