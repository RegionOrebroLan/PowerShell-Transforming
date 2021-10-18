using System.Management.Automation;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	public abstract class BasicTransformCommand : Cmdlet
	{
		#region Fields

		private const string _missingMethodExceptionMessage = "A method is missing. Make sure you have .NET Framework 4.6.2 or .NET Core 3.0 or .NET 5.0, or higher, installed. This PowerShell-module is built with .NET Standard 2.0.";

		#endregion

		#region Properties

		[Parameter(Position = 1, Mandatory = true)]
		public virtual string Destination { get; set; }

		protected internal virtual string MissingMethodExceptionMessage => _missingMethodExceptionMessage;

		[Parameter(Position = 0, Mandatory = true)]
		public virtual string Source { get; set; }

		#endregion
	}
}