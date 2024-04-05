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

		protected internal virtual InformationRecord CreateInformationRecord(LogLevel logLevel, string message)
		{
			var informationRecord = new InformationRecord(message, this.Cmdlet?.ToString());

			informationRecord.Tags.Add(logLevel.ToString());

			return informationRecord;
		}

		public virtual bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if(this.Cmdlet == null)
				return;

			var message = $"{logLevel}-log ({eventId}): {formatter(state, exception)}";

			switch(logLevel)
			{
				case LogLevel.Debug:
					this.Cmdlet.WriteDebug(message);
					break;
				case LogLevel.Critical:
				case LogLevel.Error:
					//// this.Cmdlet.WriterError, commented out below, seems to throw the exception. So we use this.Cmdlet.WriteInformation instead.
					// this.Cmdlet.WriteError(new ErrorRecord(new Exception(message, exception), eventId.ToString(), ErrorCategory.NotSpecified, this.Cmdlet));
					this.Cmdlet.WriteInformation(this.CreateInformationRecord(logLevel, message));
					break;
				case LogLevel.Information:
					this.Cmdlet.WriteInformation(this.CreateInformationRecord(logLevel, message));
					break;
				case LogLevel.None:
					break;
				case LogLevel.Trace:
					this.Cmdlet.WriteVerbose(message);
					break;
				case LogLevel.Warning:
					this.Cmdlet.WriteWarning(message);
					break;
				default:
					break;
			}
		}

		#endregion
	}
}