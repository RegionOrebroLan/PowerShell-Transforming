using System.IO.Compression;
using IntegrationTests.Fixtures;
using IntegrationTests.Helpers;
using RegionOrebroLan.PowerShell.Transforming.Commands;

namespace IntegrationTests.Commands
{
	[Collection(FixtureCollection.Name)]
	public class NewPackageTransformCommandTest(Fixture fixture)
	{
		#region Fields

		private readonly Fixture _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

		#endregion

		#region Methods

		protected internal virtual IEnumerable<string> GetFileSystemEntries(string path)
		{
			return Directory.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories).OrderBy(entry => entry, StringComparer.OrdinalIgnoreCase);
		}

		private string GetOutputPath(params string[] paths)
		{
			return this._fixture.GetOutputPath(paths);
		}

		private static string GetResourcePath(params string[] paths)
		{
			return Global.GetResourcePath(ResolvePaths(paths));
		}

		private static void Invoke(string destination, string[] fileToTransformPatterns, string[] pathToDeletePatterns, string source, string[] transformationNames)
		{
			Global.InvokeCommand(new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = fileToTransformPatterns,
				PathToDeletePatterns = pathToDeletePatterns,
				Source = source,
				TransformationNames = transformationNames
			});
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationIsADirectoryAndTheSourceIsAZipFile_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"], GetResourcePath("Package.zip"), ["Release", "Test"]);
			var expected = GetResourcePath("Package-Expected");
			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationIsAZipFileAndTheSourceIsADirectory_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}.zip");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/**", "**/File-To-Delete.*"], GetResourcePath("Package"), ["Release", "Test"]);
			var extractedDestination = this.GetOutputPath($"Extracted-Transformed-Package{Global.GetUniqueSuffix()}");
			ZipFile.ExtractToDirectory(destination, extractedDestination);
			var expected = GetResourcePath("Package-Expected");
			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("destination", () => Invoke(string.Empty, null, null, GetResourcePath("Empty"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentNullException>("destination", () => Invoke(null, null, null, GetResourcePath("Empty"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("destination", () => Invoke(" ", null, null, GetResourcePath("Empty"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceIsADirectory_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/*", "**/File-To-Delete.*"], GetResourcePath("Package"), ["Release", "Test"]);
			var expected = GetResourcePath("Package-Expected");
			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceIsAnEmptyDirectory_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"], GetResourcePath("Empty"), ["Release", "Test"]);
			var expected = GetResourcePath("Empty");
			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceIsAnEmptyZipFile_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}.zip");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"], GetResourcePath("Empty.zip"), ["Release", "Test"]);
			var extractedDestination = this.GetOutputPath($"Extracted-Transformed-Package{Global.GetUniqueSuffix()}");
			ZipFile.ExtractToDirectory(destination, extractedDestination);

			// To handle NET Core 3.1 and NET 5.0 and up.
			// Extracting an empty archive does not create an empty directory.
#if !NETFRAMEWORK
			if(!Directory.Exists(extractedDestination))
				Directory.CreateDirectory(extractedDestination);
#endif

			var expected = GetResourcePath("Empty");
			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceIsAZipFile_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}.zip");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"], GetResourcePath("Package.zip"), ["Release", "Test"]);
			var extractedDestination = this.GetOutputPath($"Extracted-Transformed-Package{Global.GetUniqueSuffix()}");
			ZipFile.ExtractToDirectory(destination, extractedDestination);
			var expected = GetResourcePath("Package-Expected");
			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterDoesNotExistAsDirectory_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, GetResourcePath("Non-existing-directory"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, GetResourcePath("Non-existing-file.txt"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, string.Empty, null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsNeitherADirectoryNorAZipFile_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			var source = GetResourcePath("File.txt");

			if(!File.Exists(source))
				Assert.Fail($"The file \"{source}\" does not exist.");

			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, source, null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentNullException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, null, null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}"), null, null, " ", null));
		}

		[Fact]
		public async Task ProcessRecord_ShouldTransformWithTheTransformationNamesInTheDeclaredOrderAndNotAlphabetically()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Transformed-Package{Global.GetUniqueSuffix()}");
			Invoke(destination, ["**/*.config*", "**/*.json", "**/*.xml"], null, GetResourcePath("Alphabetical-Test"), ["C", "A", "B"]);
			var expected = GetResourcePath("Alphabetical-Test-Expected");
			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();
			Assert.True(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		private static string[] ResolvePaths(params string[] paths)
		{
			return new[] { "NewPackageTransformCommandTest" }.Concat(paths).ToArray();
		}

		#endregion
	}
}