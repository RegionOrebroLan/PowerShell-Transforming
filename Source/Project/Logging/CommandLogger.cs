using System.Management.Automation;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.PowerShell.Transforming.Commands;

namespace RegionOrebroLan.PowerShell.Transforming.Logging
{
	public class CommandLogger(string categoryName, ICommand command) : ILogger
	{
		#region Properties

		public virtual string CategoryName { get; } = categoryName;
		public virtual ICommand Command { get; } = command ?? throw new ArgumentNullException(nameof(command));

		#endregion

		#region Methods

		public virtual IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			return Scope.Instance;
		}

		protected internal virtual ErrorRecord CreateErrorRecord(EventId eventId, Exception? exception, LogLevel logLevel, string? message)
		{
			return new ErrorRecord(new Exception(message, exception), eventId.ToString(), ErrorCategory.NotSpecified, this.Command);
		}

		protected internal virtual InformationRecord CreateInformationRecord(LogLevel logLevel, string? message)
		{
			var informationRecord = new InformationRecord(message, this.Command.ToString());

			informationRecord.Tags.Add(logLevel.ToString());
			return informationRecord;
		}

		protected internal virtual string CreateMessage<TState>(EventId eventId, Exception? exception, Func<TState, Exception?, string> formatter, LogLevel logLevel, TState state)
		{
			return $"{logLevel}-log ({eventId}): {formatter(state, exception)}{(exception != null ? $" -> {exception}" : null)}";
		}

		public virtual bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			var message = this.CreateMessage(eventId, exception, formatter, logLevel, state);

			switch(logLevel)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					this.Command.WriteError(this.CreateErrorRecord(eventId, exception, logLevel, message));
					break;
				case LogLevel.Debug:
					this.Command.WriteDebug(message);
					break;
				case LogLevel.Information:
					this.Command.WriteInformation(this.CreateInformationRecord(logLevel, message));
					break;
				case LogLevel.None:
					break;
				case LogLevel.Trace:
					this.Command.WriteVerbose(message);
					break;
				case LogLevel.Warning:
					this.Command.WriteWarning(message);
					break;
				default:
					break;
			}
		}

		#endregion
	}
}