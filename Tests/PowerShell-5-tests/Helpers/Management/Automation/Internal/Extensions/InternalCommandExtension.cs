using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Reflection;

namespace Tests.Helpers.Management.Automation.Internal.Extensions
{
	public static class InternalCommandExtension
	{
		#region Fields

		private static readonly PropertyInfo _myInvocationProperty = GetMyInvocationProperty();

		#endregion

		#region Methods

		public static InvocationInfo GetMyInvocation(this InternalCommand internalCommand)
		{
			return (InvocationInfo)_myInvocationProperty.GetValue(internalCommand)!;
		}

		private static PropertyInfo GetMyInvocationProperty()
		{
			return typeof(InternalCommand).GetProperty("MyInvocation", BindingFlags.Instance | BindingFlags.NonPublic)!;
		}

		#endregion
	}
}