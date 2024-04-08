using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using Moq;

namespace Tests.Helpers.Management.Automation.Extensions
{
	public static class InvocationInfoExtension
	{
		#region Fields

		private static readonly FieldInfo _scriptPositionField = GetScriptPositionField();

		#endregion

		#region Methods

		private static IScriptExtent CreateScriptExtent(string file)
		{
			var scriptExtentMock = new Mock<IScriptExtent>();

			scriptExtentMock.Setup(scriptExtent => scriptExtent.File).Returns(file);

			return scriptExtentMock.Object;
		}

		private static FieldInfo GetScriptPositionField()
		{
			return typeof(InvocationInfo).GetField("_scriptPosition", BindingFlags.Instance | BindingFlags.NonPublic)!;
		}

		public static void SetPsCommandPath(this InvocationInfo invocationInfo, string value)
		{
			if(invocationInfo == null)
				throw new ArgumentNullException(nameof(invocationInfo));

			_scriptPositionField.SetValue(invocationInfo, CreateScriptExtent(value));
		}

		#endregion
	}
}