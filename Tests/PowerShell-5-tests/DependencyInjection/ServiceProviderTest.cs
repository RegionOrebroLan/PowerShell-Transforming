using Moq;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using RegionOrebroLan.PowerShell.Transforming.DependencyInjection;
using RegionOrebroLan.PowerShell.Transforming.Logging;
using RegionOrebroLan.Transforming;

namespace Tests.DependencyInjection
{
	public class ServiceProviderTest
	{
		#region Methods

		[Fact]
		public async Task GetFileTransformerFactory_ShouldReturnAFileTransformerFactory()
		{
			await Task.CompletedTask;

			var fileTransformerFactory = ServiceProvider.Instance.GetFileTransformerFactory(ServiceProvider.Instance.GetLoggerFactory(Mock.Of<ICommand>()));
			Assert.NotNull(fileTransformerFactory);
			Assert.True(fileTransformerFactory is FileTransformerFactory);
		}

		[Fact]
		public async Task GetLoggerFactory_ShouldReturnALoggerFactory()
		{
			await Task.CompletedTask;

			var loggerFactory = ServiceProvider.Instance.GetLoggerFactory(Mock.Of<ICommand>());
			Assert.NotNull(loggerFactory);
			Assert.True(loggerFactory is CommandLoggerFactory);
		}

		[Fact]
		public async Task GetPackageTransformer_ShouldReturnAPackageTransformer()
		{
			await Task.CompletedTask;

			var packageTransformer = ServiceProvider.Instance.GetPackageTransformer(ServiceProvider.Instance.GetLoggerFactory(Mock.Of<ICommand>()));
			Assert.NotNull(packageTransformer);
			Assert.True(packageTransformer is PackageTransformer);
		}

		[Fact]
		public async Task Instance_ShouldAlwaysReturnTheSameInstance()
		{
			await Task.CompletedTask;

			var firstInstance = ServiceProvider.Instance;
			var secondInstance = ServiceProvider.Instance;
			Assert.NotNull(firstInstance);
			Assert.NotNull(secondInstance);
			Assert.True(ReferenceEquals(firstInstance, secondInstance));
		}

		#endregion
	}
}