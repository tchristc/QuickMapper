using System;
using System.Reflection;

namespace QuickMapper
{
    public class Mapper
    {
        private readonly QuickMapperWrapper _wrapper;

        public Mapper()
        {
            var compiler = new QuickMapperCompiler();
            _wrapper = compiler.Compile();
        }

        public TL Map<TL, TR>(TR right)
        {
            return _wrapper.Map<TL, TR>(right);
        }
    }

    internal class QuickMapperWrapper
    {
        private readonly Type _quickerMapperType;
        private readonly object _quickerMapper;

        public QuickMapperWrapper(Assembly assembly)
        {
            _quickerMapperType = assembly.GetType("QuickMapper.MapperImplementation");
            _quickerMapper = Activator.CreateInstance(_quickerMapperType);
        }

        public TL Map<TL, TR>(TR right)
        {
            var method = _quickerMapperType.GetMethod("Map", new[] { typeof(TR) });
            var func = (Func<TR, TL>)Delegate.CreateDelegate(typeof(Func<TR, TL>), _quickerMapper, method);
            var result = func(right);
            return result;
        }
    }
}
