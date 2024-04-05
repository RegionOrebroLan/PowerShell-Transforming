using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using RegionOrebroLan.PowerShell.Transforming.Configuration;
using RegionOrebroLan.PowerShell.Transforming.Logging;
using RegionOrebroLan.Transforming;
using RegionOrebroLan.Transforming.Configuration;
using RegionOrebroLan.Transforming.IO;

namespace RegionOrebroLan.PowerShell.Transforming.DependencyInjection
{
	public class ServiceProvider(IFileSearcher fileSearcher, IFileSystem fileSystem, IOptionsMonitor<TransformingOptions> optionsMonitor, IPackageHandlerLoader packageHandlerLoader) : IServiceProvider
	{
		#region Fields

		private static readonly IFileSearcher _fileSearcher = new FileSearcher();
		private static readonly IFileSystem _fileSystem = new FileSystem();
		private static readonly IOptionsMonitor<TransformingOptions> _optionsMonitor = new OptionsMonitor();
		private static readonly IPackageHandlerLoader _packageHandlerLoader = new PackageHandlerLoader(_fileSystem);

		#endregion

		#region Properties

		public virtual IFileSearcher FileSearcher => fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
		public virtual IFileSystem FileSystem => fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
		public static ServiceProvider Instance { get; } = new(_fileSearcher, _fileSystem, _optionsMonitor, _packageHandlerLoader);
		public virtual IOptionsMonitor<TransformingOptions> OptionsMonitor => optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		public virtual IPackageHandlerLoader PackageHandlerLoader => packageHandlerLoader ?? throw new ArgumentNullException(nameof(packageHandlerLoader));

		#endregion

		#region Methods

		public virtual IFileTransformerFactory GetFileTransformerFactory(ILoggerFactory loggerFactory)
		{
			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			return new FileTransformerFactory(this.FileSystem, loggerFactory, this.OptionsMonitor);
		}

		public virtual ILoggerFactory GetLoggerFactory(ICommand command)
		{
			if(command == null)
				throw new ArgumentNullException(nameof(command));

			return new CommandLoggerFactory(command);
		}

		public virtual IPackageTransformer GetPackageTransformer(ILoggerFactory loggerFactory)
		{
			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			return new PackageTransformer(this.FileSearcher, this.FileSystem, this.GetFileTransformerFactory(loggerFactory), loggerFactory, this.OptionsMonitor, this.PackageHandlerLoader);
		}

		#endregion
	}
}