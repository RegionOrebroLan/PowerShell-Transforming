using System.IO.Compression;
using IntegrationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.PowerShell.Transforming.Commands;

namespace IntegrationTests.Commands
{
	public abstract class BasicNewPackageTransformCommandTest : BasicTransformCommandTest
	{
		#region Methods

		protected internal virtual IEnumerable<string> GetFileSystemEntries(string path)
		{
			return Directory.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories).OrderBy(entry => entry, StringComparer.OrdinalIgnoreCase);
		}

		[TestMethod]
		public void ProcessRecord_IfTheDestinationIsADirectoryAndTheSourceIsAZipFile_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Package.zip"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var expected = this.GetTestResourcePath("Package-Expected");

			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[TestMethod]
		public void ProcessRecord_IfTheDestinationIsAZipFileAndTheSourceIsADirectory_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package.zip");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/**", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Package"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var extractedDestination = this.GetOutputPath("Extracted-Transformed-Package");

			ZipFile.ExtractToDirectory(destination, extractedDestination);

			var expected = this.GetTestResourcePath("Package-Expected");

			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheDestinationParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			this.ValidateDestinationParameterException<ArgumentException>(string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRecord_IfTheDestinationParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			this.ValidateDestinationParameterException<ArgumentNullException>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheDestinationParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			this.ValidateDestinationParameterException<ArgumentException>(" ");
		}

		[TestMethod]
		public void ProcessRecord_IfTheSourceIsADirectory_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/*", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Package"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var expected = this.GetTestResourcePath("Package-Expected");

			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[TestMethod]
		public void ProcessRecord_IfTheSourceIsAnEmptyDirectory_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Empty-directory"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var expected = this.GetTestResourcePath("Empty-directory");

			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		[TestMethod]
		public void ProcessRecord_IfTheSourceIsAnEmptyZipFile_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package.zip");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Empty.zip"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var extractedDestination = this.GetOutputPath("Extracted-Transformed-Package");

			ZipFile.ExtractToDirectory(destination, extractedDestination);

			// To handle NET 5.0 and NET Core 3.1.
			// Extracting an empty archive does not create an empty directory.
#if !NETFRAMEWORK
			if(!Directory.Exists(extractedDestination))
				Directory.CreateDirectory(extractedDestination);
#endif

			var expected = this.GetTestResourcePath("Empty-directory");

			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[TestMethod]
		public void ProcessRecord_IfTheSourceIsAZipFile_ShouldTransformCorrectly()
		{
			var destination = this.GetOutputPath("Transformed-Package.zip");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				PathToDeletePatterns = ["**/Directory-To-Delete/**/*", "**/File-To-Delete.*"],
				Source = this.GetTestResourcePath("Package.zip"),
				TransformationNames = ["Release", "Test"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var extractedDestination = this.GetOutputPath("Extracted-Transformed-Package");

			ZipFile.ExtractToDirectory(destination, extractedDestination);

			var expected = this.GetTestResourcePath("Package-Expected");

			var actualItems = this.GetFileSystemEntries(extractedDestination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(extractedDestination, expected)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterDoesNotExistAsDirectory_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-directory"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-file.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterIsNeitherADirectoryNorAZipFile_ShouldThrowAnArgumentException()
		{
			try
			{
				var newPackageTransformCommand = new NewPackageTransformCommand
				{
					Destination = this.GetOutputPath("Transformed-Package"),
					Source = this.GetTestResourcePath("File.txt")
				};

				this.InvokeCommand(newPackageTransformCommand);
			}
			catch(ArgumentException argumentException)
			{
				if(string.Equals(argumentException.ParamName, "source", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRecord_IfTheSourceParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			this.ValidateSourceParameterException<ArgumentNullException>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(" ");
		}

		[TestMethod]
		public void ProcessRecord_ShouldTransformWithTheTransformationNamesInTheDeclaredOrderAndNotAlphabetically()
		{
			var destination = this.GetOutputPath("Transformed-Package");

			var newPackageTransformCommand = new NewPackageTransformCommand
			{
				Destination = destination,
				FileToTransformPatterns = ["**/*.config*", "**/*.json", "**/*.xml"],
				Source = this.GetTestResourcePath("Alphabetical-Test"),
				TransformationNames = ["C", "A", "B"]
			};

			this.InvokeCommand(newPackageTransformCommand);

			var expected = this.GetTestResourcePath("Alphabetical-Test-Expected");

			var actualItems = this.GetFileSystemEntries(destination).ToArray();
			var expectedItems = this.GetFileSystemEntries(expected).ToArray();

			Assert.IsTrue(actualItems.SequenceEqual(expectedItems, new FileComparer(destination, expected)));
		}

		protected internal virtual void ValidateDestinationParameterException<T>(string destination) where T : ArgumentException
		{
			this.ValidateArgumentException<T>(() =>
			{
				var newPackageTransformCommand = new NewPackageTransformCommand
				{
					Destination = destination,
					Source = this.GetTestResourcePath("Empty-directory")
				};

				this.InvokeCommand(newPackageTransformCommand);
			}, "destination");
		}

		protected internal virtual void ValidateSourceParameterException<T>(string source) where T : ArgumentException
		{
			this.ValidateArgumentException<T>(() =>
			{
				var newPackageTransformCommand = new NewPackageTransformCommand
				{
					Destination = this.GetOutputPath("Transformed-Package"),
					Source = source
				};

				this.InvokeCommand(newPackageTransformCommand);
			}, "source");
		}

		#endregion
	}
}