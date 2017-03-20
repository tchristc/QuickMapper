using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickMapper
{
    public class InferredMapper : Mapper
    {
        public const string DTO = "Dto";
        public const string MODEL = "Model";
        public const string VIEW_MODEL = "ViewModel";

        public static Dictionary<string,Type> MapTypeNames { get; set; } = new Dictionary<string, Type>();

        public static List<string> ModelPostFix { get; set; } = new List<string>
        {
            DTO,
            MODEL,
            VIEW_MODEL
        };

        public void Config()
        {
            MapTypes = new Dictionary<Type, Type>();
            MapTypeNames = new Dictionary<string, Type>();

            var assemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              where
                                assembly != null
                                && !assembly.IsDynamic
                                && assembly.Location != null
                                && assembly.ManifestModule.Name != "<In Memory Module>"
                                && !assembly.FullName.StartsWith("System")
                                && !assembly.FullName.StartsWith("Microsoft")
                                && !assembly.FullName.StartsWith("mscorlib")
                                && !assembly.FullName.EndsWith("Tests")
                                && assembly.Location.IndexOf("App_Web", StringComparison.Ordinal) == -1
                                && assembly.Location.IndexOf("App_global", StringComparison.Ordinal) == -1
                                && assembly.FullName.IndexOf("CppCodeProvider", StringComparison.Ordinal) == -1
                                && assembly.FullName.IndexOf("WebMatrix", StringComparison.Ordinal) == -1
                                && assembly.FullName.IndexOf("SMDiagnostics", StringComparison.Ordinal) == -1
                                && !string.IsNullOrEmpty(assembly.Location)
                              select assembly).ToList();

            //var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            foreach (var assembly in assemblies)
            {
                var mapAssemblyTypeNames = assembly.GetTypes().ToDictionary(x => x.Name, x => x);
                foreach (var kvp in mapAssemblyTypeNames)
                {
                    if (MapTypeNames.ContainsKey(kvp.Key))
                        continue;
                    MapTypeNames.Add(kvp.Key, kvp.Value);
                }
            }

            var listTypeNames = MapTypeNames.Keys.ToList();
            foreach (var name in listTypeNames)
            {
                var keyType = MapTypeNames[name];
                if (MapTypes.ContainsKey(keyType))
                    continue;//already mapped in MapInferredTypes

                foreach (var modelPostFix in ModelPostFix)
                {
                    var typeName = name + modelPostFix;
                    if (MapTypes.ContainsKey(keyType))
                        continue;
                    if (listTypeNames.Contains(typeName))
                        MapTypes[keyType] = MapTypeNames[typeName];
                }
                
            }
        }
    }
}
