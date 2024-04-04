namespace RegionOrebroLan.PowerShell.Transforming.Logging
{
	public sealed class Scope : IDisposable
	{
		#region Constructors

		private Scope() { }

		#endregion

		#region Properties

		public static Scope Instance { get; } = new();

		#endregion

		#region Methods

		public void Dispose() { }

		#endregion
	}
}