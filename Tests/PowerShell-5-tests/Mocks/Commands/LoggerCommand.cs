using System.Management.Automation;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using IServiceProvider = RegionOrebroLan.PowerShell.Transforming.DependencyInjection.IServiceProvider;

namespace IntegrationTests.Mocks.Commands
{
	public class LoggerCommand(IServiceProvider serviceProvider) : BasicCommand
	{
		#region Fields

		private ILogger _logger;
		private ILoggerFactory _loggerFactory;
		private Lazy<Action> _processRecordAction;

		#endregion

		#region Constructors

		public LoggerCommand() : this(RegionOrebroLan.PowerShell.Transforming.DependencyInjection.ServiceProvider.Instance) { }

		#endregion

		#region Properties

		public virtual ILogger Logger => this._logger ??= this.LoggerFactory.CreateLogger(this.GetType());
		public virtual ILoggerFactory LoggerFactory => this._loggerFactory ??= this.ServiceProvider.GetLoggerFactory(this);

		public virtual Action ProcessRecordAction
		{
			get
			{
				this._processRecordAction ??= new Lazy<Action>(() =>
				{
					return () =>
					{
						this.Logger.LogCritical("Critical log");
						this.Logger.LogCritical(new CmdletInvocationException("Critical exception"), "Critical log");
						this.Logger.LogDebug("Debug log");
						this.Logger.LogDebug(new CmdletInvocationException("Debug exception"), "Debug log");
						this.Logger.LogError("Error log");
						this.Logger.LogError(new CmdletInvocationException("Error exception"), "Error log");
						this.Logger.LogInformation("Information log");
						this.Logger.LogInformation(new CmdletInvocationException("Information exception"), "Information log");
						this.Logger.LogTrace("Trace log");
						this.Logger.LogTrace(new CmdletInvocationException("Trace exception"), "Trace log");
						this.Logger.LogWarning("Warning log");
						this.Logger.LogWarning(new CmdletInvocationException("Warning exception"), "Warning log");
					};
				});

				return this._processRecordAction.Value;
			}
			set => this._processRecordAction = new Lazy<Action>(() => { return value ?? (() => { }); });
		}

		public virtual IServiceProvider ServiceProvider => serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			this.ProcessRecordAction();
		}

		#endregion
	}
}