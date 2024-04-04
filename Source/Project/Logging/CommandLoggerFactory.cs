using System.Collections.Concurrent;
using System.Management.Automation;
using Microsoft.Extensions.Logging;

namespace RegionOrebroLan.PowerShell.Transforming.Logging
{
	public class CommandLoggerFactory(Cmdlet command) : ILoggerFactory
	{
		#region Properties

		public virtual Cmdlet Command { get; } = command ?? throw new ArgumentNullException(nameof(command));
		protected internal virtual ConcurrentDictionary<string, ILogger> Loggers { get; } = new(StringComparer.OrdinalIgnoreCase);

		#endregion

		#region Methods

		public virtual void AddProvider(ILoggerProvider provider) { }

		public virtual ILogger CreateLogger(string categoryName)
		{
			return this.Loggers.GetOrAdd(categoryName, key => new CommandLogger(key, this.Command));
		}

		public virtual void Dispose() { }

		#endregion
	}
}