using System.Management.Automation;
using Tests.Helpers.Management.Automation.Internal.Extensions;

namespace Tests.Helpers.Management.Automation.Extensions
{
	public static class CmdletExtension
	{
		#region Methods

		public static void PrepareForTest(this Cmdlet command)
		{
			if(command == null)
				throw new ArgumentNullException(nameof(command));

			command.GetMyInvocation().SetPsCommandPath("command-path");
		}

		#endregion
	}
}