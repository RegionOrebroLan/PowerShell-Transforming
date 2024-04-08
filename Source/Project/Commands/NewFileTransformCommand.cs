using System.Management.Automation;
using RegionOrebroLan.Transforming;
using IServiceProvider = RegionOrebroLan.PowerShell.Transforming.DependencyInjection.IServiceProvider;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	[Cmdlet(VerbsCommon.New, "FileTransform")]
	public class NewFileTransformCommand(IServiceProvider serviceProvider) : BasicTransformCommand(serviceProvider)
	{
		#region Fields

		private IFileTransformerFactory? _fileTransformerFactory;

		#endregion

		#region Constructors

		public NewFileTransformCommand() : this(DependencyInjection.ServiceProvider.Instance) { }

		#endregion

		#region Properties

		protected internal virtual IFileTransformerFactory FileTransformerFactory => this._fileTransformerFactory ??= this.ServiceProvider.GetFileTransformerFactory(this.ServiceProvider.GetLoggerFactory(this));

		[Parameter(Position = 2, Mandatory = true)]
		public virtual string? Transformation { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			try
			{
				this.FileTransformerFactory.Create(this.Source).Transform(this.Destination, this.Source, this.Transformation, this.CreateOptions().File);
			}
			catch(MissingMethodException missingMethodException)
			{
				throw new InvalidOperationException(this.MissingMethodExceptionMessage, missingMethodException);
			}
		}

		#endregion
	}
}