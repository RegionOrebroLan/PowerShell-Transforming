using System.Management.Automation;
using RegionOrebroLan.Transforming;
using RegionOrebroLan.Transforming.Configuration;
using IServiceProvider = RegionOrebroLan.PowerShell.Transforming.DependencyInjection.IServiceProvider;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	[Cmdlet(VerbsCommon.New, "PackageTransform")]
	public class NewPackageTransformCommand(IServiceProvider serviceProvider) : BasicTransformCommand(serviceProvider)
	{
		#region Fields

		private IPackageTransformer? _packageTransformer;

		#endregion

		#region Constructors

		public NewPackageTransformCommand() : this(DependencyInjection.ServiceProvider.Instance) { }

		#endregion

		#region Properties

		[Parameter(Position = 20)]
		public virtual bool? Cleanup { get; set; }

		[Parameter(Position = 2)]
		public virtual string[]? FileToTransformPatterns { get; set; }

		protected internal virtual IPackageTransformer PackageTransformer => this._packageTransformer ??= this.ServiceProvider.GetPackageTransformer(this.ServiceProvider.GetLoggerFactory(this));

		[Parameter(Position = 4)]
		public virtual string[]? PathToDeletePatterns { get; set; }

		[Parameter(Position = 3)]
		public virtual string[]? TransformationNames { get; set; }

		#endregion

		#region Methods

		protected internal override TransformingOptions CreateOptions()
		{
			var options = base.CreateOptions();

			if(this.Cleanup != null)
				options.Package.Cleanup = this.Cleanup.Value;

			return options;
		}

		protected override void ProcessRecord()
		{
			try
			{
				this.PackageTransformer.Transform(this.Destination, this.FileToTransformPatterns, this.PathToDeletePatterns, this.Source, this.TransformationNames, this.CreateOptions());

				this.WriteObject($"The transformation was successful: \"{this.Destination}\"");
			}
			catch(MissingMethodException missingMethodException)
			{
				throw new InvalidOperationException(this.MissingMethodExceptionMessage, missingMethodException);
			}
		}

		#endregion
	}
}