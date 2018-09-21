using System;
using System.Management.Automation;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	[CLSCompliant(false)]
	public abstract class BasicTransformCommand : Cmdlet
	{
		#region Fields

		private const string _missingMethodExceptionMessage = "A method is missing. Make sure you have .NET Framework 4.6, or higher, installed.";

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