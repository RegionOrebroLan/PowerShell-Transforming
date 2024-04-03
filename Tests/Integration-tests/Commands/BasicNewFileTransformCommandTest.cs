using IntegrationTests.Fixtures;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using Xunit;

namespace IntegrationTests.Commands
{
	/// <summary>
	/// We need to use IClassFixture and not ICollectionFixture when using this "base"-project.
	/// https://xunit.net/docs/shared-context#collection-fixture:
	/// - Important note: Fixtures can be shared across assemblies, but collection definitions must be in the same assembly as the test that uses them.
	/// </summary>
	public abstract class BasicNewFileTransformCommandTest(FileTransformFixture fixture) : IClassFixture<FileTransformFixture>
	{
		private readonly FileTransformFixture _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

		#region Methods

		private static void Invoke(string destination, string source, string transformation)
		{
			Global.InvokeCommand(new NewFileTransformCommand
			{
				Destination = destination,
				Source = source,
				Transformation = transformation
			});
		}

		private string GetOutputPath(params string[] paths)
		{
			return this._fixture.GetOutputPath(paths);
		}

		private static string GetResourcePath(params string[] paths)
		{
			return Global.GetResourcePath(ResolvePaths(paths));
		}

		private static string[] ResolvePaths(params string[] paths)
		{
			return new[] { "NewFileTransformCommandTest" }.Concat(paths).ToArray();
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("destination", () => Invoke(string.Empty, GetResourcePath("Web.config"), GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentNullException>("destination", () => Invoke(null, GetResourcePath("Web.config"), GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheDestinationParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("destination", () => Invoke(" ", GetResourcePath("Web.config"), GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public void ProcessRecord_IfThereAreNoTransformations_ShouldTransformCorrectly()
		{
			var fileName = "Web.config";
			var destination = this.GetOutputPath("Web.Not-Transformed.config");

			var newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = GetResourcePath(fileName),
				Transformation = GetResourcePath("Web.No-Transformation.config")
			};

			Global.InvokeCommand(newFileTransformCommand);

			var expectedContent = File.ReadAllText(GetResourcePath("Web.No-Transformation.Expected.config"));
			var actualContent = File.ReadAllText(destination);

			Assert.Equal(expectedContent, actualContent);

			fileName = "appsettings.json";
			destination = this.GetOutputPath("appsettings.Not-Transformed.json");

			newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = GetResourcePath(fileName),
				Transformation = GetResourcePath("appsettings.No-Transformation.json")
			};

			Global.InvokeCommand(newFileTransformCommand);

			expectedContent = File.ReadAllText(GetResourcePath("appsettings.No-Transformation.Expected.json"));
			actualContent = File.ReadAllText(destination);

			Assert.Equal(expectedContent, actualContent);
		}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheSourceParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-file.txt"));
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheSourceParameterIsADirectory_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Package"));
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheSourceParameterIsEmpty_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateSourceParameterException<ArgumentException>(string.Empty);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentNullException))]
		//public void ProcessRecord_IfTheSourceParameterIsNull_ShouldThrowAnArgumentNullException()
		//{
		//	this.ValidateSourceParameterException<ArgumentNullException>(null);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheSourceParameterIsWhitespace_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateSourceParameterException<ArgumentException>(" ");
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheTransformationParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateTransformationParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-file.txt"));
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheTransformationParameterIsADirectory_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateTransformationParameterException<ArgumentException>(this.GetTestResourcePath("Package"));
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheTransformationParameterIsEmpty_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateTransformationParameterException<ArgumentException>(string.Empty);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentNullException))]
		//public void ProcessRecord_IfTheTransformationParameterIsNull_ShouldThrowAnArgumentNullException()
		//{
		//	this.ValidateTransformationParameterException<ArgumentNullException>(null);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ProcessRecord_IfTheTransformationParameterIsWhitespace_ShouldThrowAnArgumentException()
		//{
		//	this.ValidateTransformationParameterException<ArgumentException>(" ");
		//}

		//[TestMethod]
		//public void ProcessRecord_ShouldTransformCorrectly()
		//{
		//	var fileName = "Web.config";
		//	var destination = this.GetOutputPath(fileName);

		//	var newFileTransformCommand = new NewFileTransformCommand
		//	{
		//		Destination = destination,
		//		Source = this.GetTestResourcePath(fileName),
		//		Transformation = this.GetTestResourcePath("Web.Transformation.config")
		//	};

		//	this.InvokeCommand(newFileTransformCommand);

		//	var expectedContent = File.ReadAllText(this.GetTestResourcePath("Web.Expected.config"));
		//	var actualContent = File.ReadAllText(destination);

		//	Assert.AreEqual(expectedContent, actualContent);

		//	fileName = "AppSettings.json";
		//	destination = this.GetOutputPath(fileName);

		//	newFileTransformCommand = new NewFileTransformCommand
		//	{
		//		Destination = destination,
		//		Source = this.GetTestResourcePath(fileName),
		//		Transformation = this.GetTestResourcePath("AppSettings.Transformation.json")
		//	};

		//	this.InvokeCommand(newFileTransformCommand);

		//	expectedContent = File.ReadAllText(this.GetTestResourcePath("AppSettings.Expected.json"));
		//	actualContent = File.ReadAllText(destination);

		//	Assert.AreEqual(expectedContent, actualContent);
		//}

		//protected internal virtual void ValidateDestinationParameterException<T>(string destination) where T : ArgumentException
		//{
		//	this.ValidateArgumentException<T>(() =>
		//	{
		//		var newFileTransformCommand = new NewFileTransformCommand
		//		{
		//			Destination = destination,
		//			Source = this.GetTestResourcePath("Web.config"),
		//			Transformation = this.GetTestResourcePath("Web.Transformation.config")
		//		};

		//		this.InvokeCommand(newFileTransformCommand);
		//	}, "destination");
		//}

		//protected internal virtual void ValidateSourceParameterException<T>(string source) where T : ArgumentException
		//{
		//	this.ValidateArgumentException<T>(() =>
		//	{
		//		var newFileTransformCommand = new NewFileTransformCommand
		//		{
		//			Destination = this.GetOutputPath("Web.config"),
		//			Source = source,
		//			Transformation = this.GetTestResourcePath("Web.Transformation.config")
		//		};

		//		this.InvokeCommand(newFileTransformCommand);
		//	}, "source");
		//}

		//protected internal virtual void ValidateTransformationParameterException<T>(string transformation) where T : ArgumentException
		//{
		//	this.ValidateArgumentException<T>(() =>
		//	{
		//		var newFileTransformCommand = new NewFileTransformCommand
		//		{
		//			Destination = this.GetOutputPath("Web.config"),
		//			Source = this.GetTestResourcePath("Web.config"),
		//			Transformation = transformation
		//		};

		//		this.InvokeCommand(newFileTransformCommand);
		//	}, "transformation");
		//}

		#endregion
	}
}