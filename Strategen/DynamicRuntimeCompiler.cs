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

		// Set up compilation options
		var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

		// Add references to standard assemblies
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
            //MetadataReference.CreateFromFile(typeof().Assembly.Location),
			// Add other necessary references
			};

		// Compile the code
		var compilation = CSharpCompilation.Create("DynamicAsembly")
			.WithOptions(compilationOptions)
			.AddReferences(references)
			.AddSyntaxTrees(syntaxTree);

		using (var memoryStream = new MemoryStream()) {
			// Emit the compiled assembly to memory
			EmitResult result = compilation.Emit(memoryStream);
			if (result.Success) {
				// Load the assembly
				Assembly assembly = Assembly.Load(memoryStream.ToArray());

				// Get the dynamically compiled type
				Type type = assembly.GetType(className);

				if (type != null) {
					// Create an instance of the dynamically compiled type
					object instance = Activator.CreateInstance(type);

					// Return the instance
					return (StrategyBase)instance;
				} else {
					throw new InvalidOperationException($"Class '{className}' not found in the dynamically compiled assembly.");
				}
			} else {
				string err = "";
				// Compilation failed, handle errors
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

