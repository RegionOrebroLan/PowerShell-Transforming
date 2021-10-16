using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.PowerShell.Transforming.Commands;

namespace IntegrationTests.Commands
{
	public abstract class BasicNewFileTransformCommandTest : BasicTransformCommandTest
	{
		#region Methods

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
			this.ValidateDestinationParameterException<ArgumentNullException>(" ");
		}

		[TestMethod]
		public void ProcessRecord_IfThereAreNoTransformations_ShouldTransformCorrectly()
		{
			var fileName = "Web.config";
			var destination = this.GetOutputPath("Web.Not-Transformed.config");

			var newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = this.GetTestResourcePath(fileName),
				Transformation = this.GetTestResourcePath("Web.No-Transformation.config")
			};

			this.InvokeCommand(newFileTransformCommand);

			var expectedContent = File.ReadAllText(this.GetTestResourcePath("Web.No-Transformation.Expected.config"));
			var actualContent = File.ReadAllText(destination);

			Assert.AreEqual(expectedContent, actualContent);

			fileName = "AppSettings.json";
			destination = this.GetOutputPath("AppSettings.Not-Transformed.json");

			newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = this.GetTestResourcePath(fileName),
				Transformation = this.GetTestResourcePath("AppSettings.No-Transformation.json")
			};

			this.InvokeCommand(newFileTransformCommand);

			expectedContent = File.ReadAllText(this.GetTestResourcePath("AppSettings.No-Transformation.Expected.json"));
			actualContent = File.ReadAllText(destination);

			Assert.AreEqual(expectedContent, actualContent);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-file.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterIsADirectory_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(this.GetTestResourcePath("Package"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheSourceParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			this.ValidateSourceParameterException<ArgumentException>(string.Empty);
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
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheTransformationParameterDoesNotExistAsFile_ShouldThrowAnArgumentException()
		{
			this.ValidateTransformationParameterException<ArgumentException>(this.GetTestResourcePath("Non-existing-file.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheTransformationParameterIsADirectory_ShouldThrowAnArgumentException()
		{
			this.ValidateTransformationParameterException<ArgumentException>(this.GetTestResourcePath("Package"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheTransformationParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			this.ValidateTransformationParameterException<ArgumentException>(string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRecord_IfTheTransformationParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			this.ValidateTransformationParameterException<ArgumentNullException>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfTheTransformationParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			this.ValidateTransformationParameterException<ArgumentException>(" ");
		}

		[TestMethod]
		public void ProcessRecord_ShouldTransformCorrectly()
		{
			var fileName = "Web.config";
			var destination = this.GetOutputPath(fileName);

			var newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = this.GetTestResourcePath(fileName),
				Transformation = this.GetTestResourcePath("Web.Transformation.config")
			};

			this.InvokeCommand(newFileTransformCommand);

			var expectedContent = File.ReadAllText(this.GetTestResourcePath("Web.Expected.config"));
			var actualContent = File.ReadAllText(destination);

			Assert.AreEqual(expectedContent, actualContent);

			fileName = "AppSettings.json";
			destination = this.GetOutputPath(fileName);

			newFileTransformCommand = new NewFileTransformCommand
			{
				Destination = destination,
				Source = this.GetTestResourcePath(fileName),
				Transformation = this.GetTestResourcePath("AppSettings.Transformation.json")
			};

			this.InvokeCommand(newFileTransformCommand);

			expectedContent = File.ReadAllText(this.GetTestResourcePath("AppSettings.Expected.json"));
			actualContent = File.ReadAllText(destination);

			Assert.AreEqual(expectedContent, actualContent);
		}

		protected internal virtual void ValidateDestinationParameterException<T>(string destination) where T : ArgumentException
		{
			this.ValidateArgumentException<T>(() =>
			{
				var newFileTransformCommand = new NewFileTransformCommand
				{
					Destination = destination,
					Source = this.GetTestResourcePath("Web.config"),
					Transformation = this.GetTestResourcePath("Web.Transformation.config")
				};

				this.InvokeCommand(newFileTransformCommand);
			}, "destination");
		}

		protected internal virtual void ValidateSourceParameterException<T>(string source) where T : ArgumentException
		{
			this.ValidateArgumentException<T>(() =>
			{
				var newFileTransformCommand = new NewFileTransformCommand
				{
					Destination = this.GetOutputPath("Web.config"),
					Source = source,
					Transformation = this.GetTestResourcePath("Web.Transformation.config")
				};

				this.InvokeCommand(newFileTransformCommand);
			}, "source");
		}

		protected internal virtual void ValidateTransformationParameterException<T>(string transformation) where T : ArgumentException
		{
			this.ValidateArgumentException<T>(() =>
			{
				var newFileTransformCommand = new NewFileTransformCommand
				{
					Destination = this.GetOutputPath("Web.config"),
					Source = this.GetTestResourcePath("Web.config"),
					Transformation = transformation
				};

				this.InvokeCommand(newFileTransformCommand);
			}, "transformation");
		}

		#endregion
	}
}