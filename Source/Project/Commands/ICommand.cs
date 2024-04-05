using System.Collections;
using System.Management.Automation;

namespace RegionOrebroLan.PowerShell.Transforming.Commands
{
	public interface ICommand
	{
		#region Properties

		CommandOrigin CommandOrigin { get; }
		ICommandRuntime CommandRuntime { get; set; }
		bool Stopping { get; }

		#endregion

		#region Methods

		string GetResourceString(string baseName, string resourceId);
		IEnumerable Invoke();
		bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll);
		bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll);
		bool ShouldContinue(string query, string caption);
		bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason);
		bool ShouldProcess(string target);
		bool ShouldProcess(string target, string action);
		bool ShouldProcess(string verboseDescription, string verboseWarning, string caption);
		void ThrowTerminatingError(ErrorRecord errorRecord);
		bool TransactionAvailable();
		void WriteCommandDetail(string text);
		void WriteDebug(string text);
		void WriteError(ErrorRecord errorRecord);
		void WriteInformation(object messageData, string[] tags);
		void WriteInformation(InformationRecord informationRecord);
		void WriteObject(object sendToPipeline, bool enumerateCollection);
		void WriteObject(object sendToPipeline);
		void WriteProgress(ProgressRecord progressRecord);
		void WriteVerbose(string text);
		void WriteWarning(string text);

		#endregion
	}
}