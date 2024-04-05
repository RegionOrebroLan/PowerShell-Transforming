using RegionOrebroLan.PowerShell.Transforming.Configuration;

namespace IntegrationTests.Configuration
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
		public async Task Get_IfTheNameParameterIsAnEmptyString_ShouldReturnTheCurrentValueInstance()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();
			var options = optionsMonitor.Get(string.Empty);
			Assert.NotNull(options);
			Assert.True(ReferenceEquals(options, optionsMonitor.CurrentValue));
		}

		[Fact]
		public async Task Get_IfTheNameParameterIsNeitherNullNorEmpty_ShouldReturnNull()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();
			Assert.Null(optionsMonitor.Get(" "));
			Assert.Null(optionsMonitor.Get("a"));
			Assert.Null(optionsMonitor.Get("Something"));
		}

		[Fact]
		public async Task Get_IfTheNameParameterIsNull_ShouldReturnTheCurrentValueInstance()
		{
			await Task.CompletedTask;

			var optionsMonitor = new OptionsMonitor();
			var options = optionsMonitor.Get(null);
			Assert.NotNull(options);
			Assert.True(ReferenceEquals(options, optionsMonitor.CurrentValue));
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