using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

static class DynamicRuntimeCompiler {
	public static StrategyBase ExecuteCode(string code, string className) {
		SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

		var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

		var references = new List<MetadataReference>
		{
            MetadataReference.CreateFromFile(typeof(StrategyBase).Assembly.Location),
            MetadataReference.CreateFromFile(Path.GetDirectoryName(typeof(object).Assembly.Location) + "/System.Runtime.dll"),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Int32).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enum).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Gameboard).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Unit).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.RuntimeHelpers).Assembly.Location),

			};

		var compilation = CSharpCompilation.Create("DynamicAsembly")
			.WithOptions(compilationOptions)
			.AddReferences(references)
			.AddSyntaxTrees(syntaxTree);

		using (var memoryStream = new MemoryStream()) {

			EmitResult result = compilation.Emit(memoryStream);
			if (result.Success) { 
				Assembly assembly = Assembly.Load(memoryStream.ToArray());

				Type type = assembly.GetType(className);

				if (type != null) {
					object instance = Activator.CreateInstance(type);

					return (StrategyBase) instance;
				} else {
					throw new InvalidOperationException($"Class '{className}' not found in the dynamically compiled assembly.");
				}
			} else {
				string err = "";
				// Compilation fail
				IEnumerable <Diagnostic> errors = result.Diagnostics.Where(diagnostic =>
				diagnostic.IsWarningAsError ||
				diagnostic.Severity == DiagnosticSeverity.Error);

				foreach (Diagnostic error in errors) {
					err += $"{error.Id}: {error.GetMessage()} \n";
				}
				

				throw new InvalidOperationException(err);
			}
		}
	}
}

