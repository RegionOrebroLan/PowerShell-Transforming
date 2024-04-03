using System.Collections;
using System.Management.Automation;

namespace IntegrationTests
{
	public static class Global
	{
		#region Fields

		public static DirectoryInfo ResourcesDirectory = GetResourcesDirectory();
		public static DirectoryInfo OutputDirectory = GetOutputDirectory();

		#endregion

		#region Methods

		private static DirectoryInfo GetOutputDirectory()
		{
			var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			return new DirectoryInfo(Path.Combine(baseDirectory.Parent!.Parent!.Parent!.FullName, "Test-output"));
		}

		public static string GetResourcePath(params string[] paths)
		{
			return Path.Combine(new[] { ResourcesDirectory.FullName }.Concat(paths).ToArray());
		}

		private static DirectoryInfo GetResourcesDirectory()
		{
			var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			return new DirectoryInfo(Path.Combine(baseDirectory.Parent!.Parent!.Parent!.Parent!.FullName, "Integration-tests", "Resources"));
		}

		public static void InvokeCommand(Cmdlet command)
		{
			if(command == null)
				throw new ArgumentNullException(nameof(command));

			IEnumerator result = null;

			try
			{
				result = command.Invoke().GetEnumerator();

				result.MoveNext();
			}
			finally
			{
				if(result is IDisposable disposable)
					disposable.Dispose();
			}
		}

		#endregion
	}
}