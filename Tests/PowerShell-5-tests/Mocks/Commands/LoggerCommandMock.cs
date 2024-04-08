using System.Collections;
using System.Management.Automation;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using IServiceProvider = RegionOrebroLan.PowerShell.Transforming.DependencyInjection.IServiceProvider;

namespace Tests.Mocks.Commands
{
	public class LoggerCommandMock(IServiceProvider serviceProvider) : ICommand
	{
		#region Fields

		private ILogger _logger;
		private ILoggerFactory _loggerFactory;
		private Lazy<Action> _processRecordAction;

		#endregion

		#region Constructors

		public LoggerCommandMock() : this(RegionOrebroLan.PowerShell.Transforming.DependencyInjection.ServiceProvider.Instance) { }

		#endregion

		#region Properties

		public virtual CommandOrigin CommandOrigin { get; set; }
		public virtual ICommandRuntime CommandRuntime { get; set; }
		public virtual IList<string> Debug { get; } = [];
		public virtual IList<InformationRecord> Information { get; } = [];
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
		public virtual bool Stopping { get; set; }
		public virtual IList<string> Trace { get; } = [];
		public virtual IList<string> Warning { get; } = [];

		#endregion

		#region Methods

		public virtual string GetResourceString(string baseName, string resourceId)
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable Invoke()
		{
			return this.Invoke<object>();
		}

		public virtual IEnumerable<T> Invoke<T>()
		{
			this.ProcessRecord();

			return [];
		}

		protected virtual void ProcessRecord()
		{
			this.ProcessRecordAction();
		}

		public virtual bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldContinue(string query, string caption)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldProcess(string target)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldProcess(string target, string action)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			throw new NotImplementedException();
		}

		public virtual void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			throw new NotImplementedException();
		}

		public virtual bool TransactionAvailable()
		{
			throw new NotImplementedException();
		}

		public virtual void WriteCommandDetail(string text)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteDebug(string text)
		{
			this.Debug.Add(text);
		}

		public virtual void WriteError(ErrorRecord errorRecord)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteInformation(object messageData, string[] tags)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteInformation(InformationRecord informationRecord)
		{
			this.Information.Add(informationRecord);
		}

		public virtual void WriteObject(object sendToPipeline, bool enumerateCollection)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteObject(object sendToPipeline)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteProgress(ProgressRecord progressRecord)
		{
			throw new NotImplementedException();
		}

		public virtual void WriteVerbose(string text)
		{
			this.Trace.Add(text);
		}

		public virtual void WriteWarning(string text)
		{
			this.Warning.Add(text);
		}

		#endregion
	}
}