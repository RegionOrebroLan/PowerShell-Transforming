using System.Management.Automation;
using Microsoft.Extensions.Logging;

namespace RegionOrebroLan.PowerShell.Transforming.Logging
{
	public class CommandLogger(string categoryName, Cmdlet command) : ILogger
	{
		#region Properties

		public virtual string CategoryName { get; } = categoryName;
		public virtual Cmdlet Cmdlet { get; } = command ?? throw new ArgumentNullException(nameof(command));

		#endregion

		#region Methods

		public virtual IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			return Scope.Instance;
		}

		public virtual bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if(this.Cmdlet == null)
				return;

			switch(logLevel)
			{
				case LogLevel.Debug:
					this.Cmdlet.WriteDebug("Debug");
					break;
				case LogLevel.Critical:
				case LogLevel.Error:
					this.Cmdlet.WriteError(new ErrorRecord(exception, eventId.Name, ErrorCategory.NotSpecified, this.Cmdlet));
					break;
				case LogLevel.Information:
					this.Cmdlet.WriteInformation(null, null);
					break;
				case LogLevel.None:
					break;
				case LogLevel.Trace:
					this.Cmdlet.WriteVerbose("Trace");
					break;
				case LogLevel.Warning:
					this.Cmdlet.WriteWarning("Warning");
					break;
				default:
					break;
			}
		}

		#endregion
	}
}