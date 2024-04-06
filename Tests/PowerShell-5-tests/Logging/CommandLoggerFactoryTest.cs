using Moq;
using RegionOrebroLan.PowerShell.Transforming.Commands;
using RegionOrebroLan.PowerShell.Transforming.Logging;

namespace Tests.Logging
{
	public class CommandLoggerFactoryTest
	{
		#region Methods

		[Fact]
		public async Task CreateLogger_IfNotCreatedByCategoryNameBefore_ShouldAddTheCreatedLoggerToLoggers()
		{
			await Task.CompletedTask;

			var command = Mock.Of<ICommand>();
			var commandLoggerFactory = new CommandLoggerFactory(command);
			Assert.Empty(commandLoggerFactory.Loggers);
			var logger = commandLoggerFactory.CreateLogger("Test");
			Assert.Single(commandLoggerFactory.Loggers);
			Assert.Equal("Test", commandLoggerFactory.Loggers.ElementAt(0).Key);
			Assert.True(ReferenceEquals(commandLoggerFactory.Loggers.ElementAt(0).Value, logger));
		}

		[Fact]
		public async Task CreateLogger_IfTheCategoryNameIsNotTheSame_ShouldNotReturnTheSameInstance()
		{
			await Task.CompletedTask;

			var command = Mock.Of<ICommand>();
			var commandLoggerFactory = new CommandLoggerFactory(command);
			var firstCommandLogger = commandLoggerFactory.CreateLogger("1");
			var secondCommandLogger = commandLoggerFactory.CreateLogger("2");
			Assert.False(ReferenceEquals(firstCommandLogger, secondCommandLogger));
		}

		[Fact]
		public async Task CreateLogger_IfTheCategoryNameIsTheSame_ShouldReturnTheSameInstance()
		{
			await Task.CompletedTask;

			var command = Mock.Of<ICommand>();
			var commandLoggerFactory = new CommandLoggerFactory(command);
			var firstCommandLogger = commandLoggerFactory.CreateLogger("1");
			var secondCommandLogger = commandLoggerFactory.CreateLogger("1");
			Assert.True(ReferenceEquals(firstCommandLogger, secondCommandLogger));
		}

		[Fact]
		public async Task CreateLogger_ShouldReturnACommandLogger()
		{
			await Task.CompletedTask;

			var command = Mock.Of<ICommand>();
			var commandLoggerFactory = new CommandLoggerFactory(command);
			var commandLogger = commandLoggerFactory.CreateLogger("Test");
			Assert.True(commandLogger is CommandLogger);
		}

		#endregion
	}
}