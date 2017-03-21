using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace QuickMapper
{
    internal class QuickMapperCompiler
    {
        private static readonly IEnumerable<string> DefaultNamespaces =
            new[]
            {
                        "System",
                        "System.IO",
                        "System.Net",
                        "System.Linq",
                        "System.Text",
                        "System.Text.RegularExpressions",
                        "System.Collections.Generic"
            };

        private static readonly IEnumerable<MetadataReference> DefaultReferences =
            new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

        private static readonly CSharpCompilationOptions DefaultCompilationOptions =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithUsings(DefaultNamespaces);

        public QuickMapperWrapper Compile()
        {
            var mapper = new InferredTypeMap();
            mapper.Config();

            var codeFormatter = new QuickMapperCodeFormatter();
            var code = codeFormatter.Format(mapper.MapTypes);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>(DefaultReferences);
            foreach (var kvp in mapper.MapTypes)
            {
                references.Add(MetadataReference.CreateFromFile(kvp.Key.Assembly.Location));
                references.Add(MetadataReference.CreateFromFile(kvp.Value.Assembly.Location));
            }

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                DefaultCompilationOptions);

            using (var ms = new MemoryStream())
            {

                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    return new QuickMapperWrapper(assembly);
                }
            }

            return null;
        }
    }
}
