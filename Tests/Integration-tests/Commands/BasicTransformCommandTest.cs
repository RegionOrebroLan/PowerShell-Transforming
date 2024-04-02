namespace IntegrationTests.Commands
{
	public abstract class BasicTransformCommandTest : BasicTest
	{
		#region Properties

		protected internal override string ProjectRelativeTestResourceDirectoryPath => Path.Combine("Commands", "Test-resources");

		#endregion
	}
}