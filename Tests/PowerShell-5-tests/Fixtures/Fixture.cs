namespace IntegrationTests.Fixtures
{
	/// <summary>
	/// https://xunit.net/docs/shared-context
	/// </summary>
	public class Fixture : IDisposable
	{
		#region Fields

		private string _outputDirectoryPath;

		#endregion

		#region Properties

		public string OutputDirectoryPath
		{
			get
			{
				if(this._outputDirectoryPath == null)
				{
					var path = Path.Combine(Global.OutputDirectory.FullName, Environment.Version.ToString());

					if(!Directory.Exists(path))
						Directory.CreateDirectory(path);

					this._outputDirectoryPath = path;
				}

				return this._outputDirectoryPath;
			}
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			// The output-directory may still be locked by the tests, therefore we wait a bit and try again.
			if(Directory.Exists(this._outputDirectoryPath))
			{
				for(var i = 0; i < 20; i++)
				{
					try
					{
						Directory.Delete(this._outputDirectoryPath, true);
						break;
					}
					catch
					{
						Thread.Sleep(10);
					}
				}
			}

			GC.SuppressFinalize(this);
		}

		public string GetOutputPath(params string[] paths)
		{
			return Path.Combine(new[] { this.OutputDirectoryPath }.Concat(paths).ToArray());
		}

		#endregion
	}
}