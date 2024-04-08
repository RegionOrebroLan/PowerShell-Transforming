using RegionOrebroLan.PowerShell.Transforming.Configuration;

namespace Tests.Configuration
{
	public class OptionsMonitorTest
	{
		#region Methods

		[Fact]
		public async Task CurrentValue_ShouldAlwaysReturnTheSameInstance()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();
			var firstCurrentValue = optionsMonitor.CurrentValue;
			var secondCurrentValue = optionsMonitor.CurrentValue;
			Assert.NotNull(firstCurrentValue);
			Assert.NotNull(secondCurrentValue);
			Assert.True(ReferenceEquals(firstCurrentValue, secondCurrentValue));
		}

		[Fact]
		public async Task Get_ShouldReturnTheCurrentValueInstance()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();

			foreach(var name in new[] { null, string.Empty, " ", "a", "Something" })
			{
				var options = optionsMonitor.Get(name);
				Assert.NotNull(options);
				Assert.True(ReferenceEquals(options, optionsMonitor.CurrentValue));
			}
		}

		[Fact]
		public async Task OnChange_ShouldReturnNull()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();
			var onChange = optionsMonitor.OnChange((_, _) => { });
			Assert.Null(onChange);
		}

		#endregion
	}
}