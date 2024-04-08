using Microsoft.Extensions.Options;
using RegionOrebroLan.Transforming.Configuration;

namespace RegionOrebroLan.PowerShell.Transforming.Configuration
{
	public class OptionsMonitor : IOptionsMonitor<TransformingOptions>
	{
		#region Properties

		public TransformingOptions CurrentValue { get; } = new();

		#endregion

		#region Methods

		public TransformingOptions Get(string? name)
		{
			return this.CurrentValue;
		}

		public IDisposable? OnChange(Action<TransformingOptions, string?> listener)
		{
			return null;
		}

		#endregion
	}
}