using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace QuickMapper.Compiler
{
    public class QuickMapperCompiler
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
                        "System.Collections.Generic",
                        "System.Diagnostics"
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
                    .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release)
                    .WithUsings(DefaultNamespaces);

        public static SyntaxTree Parse(string text, string filename = "", CSharpParseOptions options = null)
        {
            var stringText = SourceText.From(text, Encoding.UTF8);
            return SyntaxFactory.ParseSyntaxTree(stringText, options, filename);
        }

        public void Config()
        {
            InferredMapper mapper = new InferredMapper();
            mapper.Config();
            foreach (var kvp in mapper.MapTypes)
            {
                
                var sb = new StringBuilder();
                sb.Append(@"using System;

                namespace QuickMapper
                {
                    public class QuickMapper
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }");

                var syntaxTree = CSharpSyntaxTree.ParseText(sb.ToString());

                var assemblyName = Path.GetRandomFileName();
                var references = new List<MetadataReference>(DefaultReferences);

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

                        var type = assembly.GetType("RoslynCompileSample.Writer");
                        var obj = Activator.CreateInstance(type);
                        type.InvokeMember("Write",
                            BindingFlags.Default | BindingFlags.InvokeMethod,
                            null,
                            obj,
                            new object[] { "Hello World" });
                    }
                }
            }
            
        }

        public void Test()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }");

            var assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>(DefaultReferences);

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

                    var type = assembly.GetType("RoslynCompileSample.Writer");
                    var obj = Activator.CreateInstance(type);
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello World" });
                }
            }
        }
    }
}
