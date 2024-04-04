using System.Collections;
using System.Management.Automation;

namespace IntegrationTests
{
	public static class Global
	{
		#region Fields

		public static DirectoryInfo OutputDirectory = GetOutputDirectory();
		public static DirectoryInfo ResourcesDirectory = GetResourcesDirectory();

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

			return new DirectoryInfo(Path.Combine(baseDirectory.Parent!.Parent!.Parent!.Parent!.FullName, "PowerShell-5-tests", "Resources"));
		}

		public static string GetUniqueSuffix()
		{
			return $"-{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
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