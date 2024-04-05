using System.Management.Automation;
using RegionOrebroLan.Transforming.Configuration;
using IServiceProvider = RegionOrebroLan.PowerShell.Transforming.DependencyInjection.IServiceProvider;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	public abstract class BasicTransformCommand(IServiceProvider serviceProvider) : BasicCommand
	{
		#region Fields

		private const string _missingMethodExceptionMessage = "A method is missing. Make sure you have .NET Framework 4.6.2 or .NET Core 3.0 or .NET 5.0, or higher, installed. This PowerShell-module is built with .NET Standard 2.0.";

		#endregion

		#region Properties

		[Parameter(Position = 10)]
		public virtual bool? AvoidByteOrderMark { get; set; }

		[Parameter(Position = 1, Mandatory = true)]
		public virtual string Destination { get; set; }

		protected internal virtual string MissingMethodExceptionMessage => _missingMethodExceptionMessage;
		protected internal virtual IServiceProvider ServiceProvider => serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		[Parameter(Position = 0, Mandatory = true)]
		public virtual string Source { get; set; }

		#endregion

		#region Methods

		protected internal virtual TransformingOptions CreateOptions()
		{
			var options = new TransformingOptions();

			if(this.AvoidByteOrderMark != null)
				options.File.AvoidByteOrderMark = this.AvoidByteOrderMark.Value;

			return options;
		}

		#endregion
	}
}