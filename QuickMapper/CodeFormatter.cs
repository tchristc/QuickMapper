using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickMapper
{
    internal class QuickMapperNamespaceFormatter
    {
        public const string DEFAULT_MAPPER_NAMESPACE_FORMAT = "using {0};";

        public string Format(string name)
        {
            var result = string.Format(DEFAULT_MAPPER_NAMESPACE_FORMAT, name);
            return result;
        }
    }

    internal class QuickMapperPropertyFormatter
    {
        public const string DEFAULT_MAPPER_PROPERTY_FORMAT = "{0} = right.{0},";

        public string Format(string property, string comma = ",")
        {
            var result = string.Format(DEFAULT_MAPPER_PROPERTY_FORMAT, property);
            return result;
        }
    }

    internal class QuickMapperClassCodeFormatter
    {
        public const string DEFAULT_MAPPER_CLASS_FORMAT = @"
                public partial class MapperImplementation
                        : IMapperImplementation<{0}, {1}>
                {{
                    public {0} Map({1} right)
                    {{
                        return new {0}
                        {{
                            {2}
                        }};
                    }}
                }}";

        public string Format(string left, string right, string properties)
        {
            var result = string.Format(DEFAULT_MAPPER_CLASS_FORMAT, left, right, properties);
            return result;
        }
    }

    internal class QuickMapperCodeFormatter
    {
        public string Format(Dictionary<Type, Type> mapTypes)
        {
            var classSb = new StringBuilder();
            var namespaceSb = new StringBuilder("using System;");
            foreach (var kvp in mapTypes)
            {
                var namespaceFormatter = new QuickMapperNamespaceFormatter();
                namespaceSb.Append(namespaceFormatter.Format(kvp.Key.Namespace));
                namespaceSb.Append(namespaceFormatter.Format(kvp.Value.Namespace));

                classSb.Append(FormatClass(kvp.Key, kvp.Value));
                classSb.Append(FormatClass(kvp.Value, kvp.Key));
            }

            namespaceSb.Append(@"
            namespace QuickMapper{
                public interface IMapperImplementation<L, R>
                {
                    L Map(R right);
                }
            ");
            namespaceSb.Append(classSb);
            namespaceSb.Append(@"}");

            var code = namespaceSb.ToString();
            return code;
        }

        private string FormatClass(Type leftType, Type rightType)
        {
            var propertyFormatter = new QuickMapperPropertyFormatter();
            var propertySb = new StringBuilder();
            var leftProperties = leftType.GetProperties();
            var rightProperties = rightType.GetProperties();
            foreach (var prop in leftProperties)
            {
                if (rightProperties.Any(r => r.Name == prop.Name && r.PropertyType == prop.PropertyType))
                    propertySb.Append(propertyFormatter.Format(prop.Name));

            }
            var codeFormatter = new QuickMapperClassCodeFormatter();
            var code = codeFormatter.Format(leftType.Name, rightType.Name, propertySb.ToString());

            return code;
        }
    }
}
