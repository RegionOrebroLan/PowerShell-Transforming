using System.Management.Automation;

namespace Tests.Mocks.Management.Automation
{
	public class CommandRuntime2Mock : CommandRuntimeMock, ICommandRuntime2
	{
		#region Properties

		public virtual IList<InformationRecord> Informations { get; } = [];

		#endregion

		#region Methods

		public virtual bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
		{
			return this.Continue;
		}

		public virtual void WriteInformation(InformationRecord informationRecord)
		{
			this.Informations.Add(informationRecord);
		}

		#endregion
	}
}