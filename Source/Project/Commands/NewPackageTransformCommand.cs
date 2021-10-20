using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using RegionOrebroLan.Transforming;
using RegionOrebroLan.Transforming.IO;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	[Cmdlet(VerbsCommon.New, "PackageTransform")]
	public class NewPackageTransformCommand : BasicTransformCommand
	{
		#region Fields

		private static readonly IFileSystem _fileSystem = new FileSystem();
		private static readonly IPackageTransformer _packageTransformer = new PackageTransformer(new FileSearcher(), _fileSystem, new FileTransformerFactory(_fileSystem), new PackageHandlerLoader(_fileSystem));

		#endregion

		#region Constructors

		public NewPackageTransformCommand() : this(_packageTransformer) { }

		protected internal NewPackageTransformCommand(IPackageTransformer packageTransformer)
		{
			this.PackageTransformer = packageTransformer ?? throw new ArgumentNullException(nameof(packageTransformer));
		}

		#endregion

		#region Properties

		[Parameter(Position = 5)]
		public virtual bool Cleanup { get; set; } = true;

		[Parameter(Position = 2)]
		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual string[] FileToTransformPatterns { get; set; }

		protected internal virtual IPackageTransformer PackageTransformer { get; }

		[Parameter(Position = 4)]
		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual string[] PathToDeletePatterns { get; set; }

		[Parameter(Position = 3)]
		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual string[] TransformationNames { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			try
			{
				this.PackageTransformer.Transform(this.Cleanup, this.Destination, this.FileToTransformPatterns, this.PathToDeletePatterns, this.Source, this.TransformationNames);
			}
			catch(MissingMethodException missingMethodException)
			{
				throw new InvalidOperationException(this.MissingMethodExceptionMessage, missingMethodException);
			}
		}

		#endregion
	}
}