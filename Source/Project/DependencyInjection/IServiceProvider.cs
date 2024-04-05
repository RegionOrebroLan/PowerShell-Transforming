using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using RegionOrebroLan.Transforming;
using RegionOrebroLan.Transforming.Configuration;
using RegionOrebroLan.Transforming.IO;

namespace RegionOrebroLan.PowerShell.Transforming.DependencyInjection
{
	public interface IServiceProvider
	{
		#region Properties

		IFileSearcher FileSearcher { get; }
		IFileSystem FileSystem { get; }
		IOptionsMonitor<TransformingOptions> OptionsMonitor { get; }
		IPackageHandlerLoader PackageHandlerLoader { get; }

		#endregion

		#region Methods

		IFileTransformerFactory GetFileTransformerFactory(ILoggerFactory loggerFactory);
		ILoggerFactory GetLoggerFactory(ICommand command);
		IPackageTransformer GetPackageTransformer(ILoggerFactory loggerFactory);

		#endregion
	}
}