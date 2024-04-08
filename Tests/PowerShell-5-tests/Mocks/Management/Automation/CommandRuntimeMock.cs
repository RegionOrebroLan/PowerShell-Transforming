using System.Management.Automation;
using System.Management.Automation.Host;

namespace Tests.Mocks.Management.Automation
{
	public class CommandRuntimeMock : ICommandRuntime
	{
		#region Properties

		public virtual IList<string> CommandDetails { get; } = [];
		public virtual bool Continue { get; set; } = true;
		public virtual PSTransactionContext? CurrentPSTransaction { get; set; }
		public virtual IList<string> Debugs { get; } = [];
		public virtual IList<ErrorRecord> Errors { get; } = [];
		public virtual PSHost? Host { get; set; }
		public virtual IList<object> Objects { get; } = [];
		public virtual IList<Tuple<object, bool>> ObjectTuples { get; } = [];
		public virtual bool Process { get; set; } = true;
		public virtual IList<ProgressRecord> Progresses { get; } = [];
		public virtual IList<Tuple<long, ProgressRecord>> ProgressTuples { get; } = [];
		public virtual bool TransactionIsAvailable { get; set; }
		public virtual IList<string> Verboses { get; } = [];
		public virtual IList<string> Warnings { get; } = [];

		#endregion

		#region Methods

		public virtual bool ShouldContinue(string query, string caption)
		{
			return this.Continue;
		}

		public virtual bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			return this.Continue;
		}

		public virtual bool ShouldProcess(string target)
		{
			return this.Process;
		}

		public virtual bool ShouldProcess(string target, string action)
		{
			return this.Process;
		}

		public virtual bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			return this.Process;
		}

		public virtual bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			shouldProcessReason = ShouldProcessReason.None;
			return this.Process;
		}

		public virtual void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			throw errorRecord.Exception;
		}

		public virtual bool TransactionAvailable()
		{
			return this.TransactionIsAvailable;
		}

		public virtual void WriteCommandDetail(string text)
		{
			this.CommandDetails.Add(text);
		}

		public virtual void WriteDebug(string text)
		{
			this.Debugs.Add(text);
		}

		public virtual void WriteError(ErrorRecord errorRecord)
		{
			this.Errors.Add(errorRecord);
		}

		public virtual void WriteObject(object sendToPipeline, bool enumerateCollection)
		{
			this.ObjectTuples.Add(Tuple.Create(sendToPipeline, enumerateCollection));
		}

		public virtual void WriteObject(object sendToPipeline)
		{
			this.Objects.Add(sendToPipeline);
		}

		public virtual void WriteProgress(long sourceId, ProgressRecord progressRecord)
		{
			this.ProgressTuples.Add(Tuple.Create(sourceId, progressRecord));
		}

		public virtual void WriteProgress(ProgressRecord progressRecord)
		{
			this.Progresses.Add(progressRecord);
		}

		public virtual void WriteVerbose(string text)
		{
			this.Verboses.Add(text);
		}

		public virtual void WriteWarning(string text)
		{
			this.Warnings.Add(text);
		}

		#endregion
	}
}