using RegionOrebroLan.PowerShell.Transforming.Commands;
using Tests.Fixtures;

namespace Tests.Commands
{
	[Collection(FixtureCollection.Name)]
	public class NewFileTransformCommandTest(Fixture fixture)
	{
		#region Fields

		private readonly Fixture _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

		#endregion

		#region Methods

		private string GetOutputPath(params string[] paths)
		{
			return this._fixture.GetOutputPath(paths);
		}

		private static string GetResourcePath(params string[] paths)
		{
			return Global.GetResourcePath(ResolvePaths(paths));
		}

		private static void Invoke(string? destination, string? source, string? transformation)
		{
			Global.InvokeCommand(new NewFileTransformCommand
			{
				Destination = destination,
				Source = source,
				Transformation = transformation
			});
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
		public async Task ProcessRecord_IfThereAreNoTransformations_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Web.Not-Transformed{Global.GetUniqueSuffix()}.config");
			Invoke(destination, GetResourcePath("Web.config"), GetResourcePath("Web.No-Transformation.config"));
			var expectedContent = File.ReadAllText(GetResourcePath("Web.No-Transformation.Expected.config"));
			var actualContent = File.ReadAllText(destination);
			Assert.Equal(expectedContent, actualContent);

			destination = this.GetOutputPath($"appsettings.Not-Transformed{Global.GetUniqueSuffix()}.json");
			Invoke(destination, GetResourcePath("appsettings.json"), GetResourcePath("appsettings.No-Transformation.json"));
			expectedContent = File.ReadAllText(GetResourcePath("appsettings.No-Transformation.Expected.json"));
			actualContent = File.ReadAllText(destination);
			Assert.Equal(expectedContent, actualContent);
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Non-existing-file.txt"), GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsADirectory_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Package"), GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath("Web.config"), string.Empty, GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentNullException>("source", () => Invoke(this.GetOutputPath("Web.config"), null, GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheSourceParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("source", () => Invoke(this.GetOutputPath("Web.config"), " ", GetResourcePath("Web.Transformation.config")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheTransformationParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("transformation", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Web.config"), GetResourcePath("Non-existing-file.txt")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheTransformationParameterIsADirectory_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("transformation", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Web.config"), GetResourcePath("Package")));
		}

		[Fact]
		public async Task ProcessRecord_IfTheTransformationParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("transformation", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Web.config"), string.Empty));
		}

		[Fact]
		public async Task ProcessRecord_IfTheTransformationParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentNullException>("transformation", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Web.config"), null));
		}

		[Fact]
		public async Task ProcessRecord_IfTheTransformationParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			Assert.Throws<ArgumentException>("transformation", () => Invoke(this.GetOutputPath("Web.config"), GetResourcePath("Web.config"), " "));
		}

		[Fact]
		public async Task ProcessRecord_ShouldTransformCorrectly()
		{
			await Task.CompletedTask;

			var destination = this.GetOutputPath($"Web{Global.GetUniqueSuffix()}.config");
			Invoke(destination, GetResourcePath("Web.config"), GetResourcePath("Web.Transformation.config"));
			var expectedContent = File.ReadAllText(GetResourcePath("Web.Expected.config"));
			var actualContent = File.ReadAllText(destination);
			Assert.Equal(expectedContent, actualContent);

			destination = this.GetOutputPath($"appsettings{Global.GetUniqueSuffix()}.json");
			Invoke(destination, GetResourcePath("appsettings.json"), GetResourcePath("appsettings.Transformation.json"));
			expectedContent = File.ReadAllText(GetResourcePath("appsettings.Expected.json"));
			actualContent = File.ReadAllText(destination);
			Assert.Equal(expectedContent, actualContent);
		}

		private static string[] ResolvePaths(params string[] paths)
		{
			return new[] { "NewFileTransformCommandTest" }.Concat(paths).ToArray();
		}

		#endregion
	}
}