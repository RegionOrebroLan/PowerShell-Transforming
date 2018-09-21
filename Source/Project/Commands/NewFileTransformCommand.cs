using System;
using System.IO.Abstractions;
using System.Management.Automation;
using RegionOrebroLan.Transforming;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsCommon.New, "FileTransform")]
	public class NewFileTransformCommand : BasicTransformCommand
	{
		#region Fields

		private static readonly IFileTransformerFactory _fileTransformerFactory = new FileTransformerFactory(new FileSystem());

		#endregion

		#region Constructors

		public NewFileTransformCommand() : this(_fileTransformerFactory) { }

		protected internal NewFileTransformCommand(IFileTransformerFactory fileTransformerFactory)
		{
			this.FileTransformerFactory = fileTransformerFactory ?? throw new ArgumentNullException(nameof(fileTransformerFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IFileTransformerFactory FileTransformerFactory { get; }

		[Parameter(Position = 2, Mandatory = true)]
		public virtual string Transformation { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			try
			{
				this.FileTransformerFactory.Create(this.Source).Transform(this.Destination, this.Source, this.Transformation);
			}
			catch(MissingMethodException missingMethodException)
			{
				throw new InvalidOperationException(this.MissingMethodExceptionMessage, missingMethodException);
			}
		}

		#endregion
	}
}