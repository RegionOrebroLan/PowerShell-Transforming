using System.Management.Automation;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.PowerShell.Transforming.Logging;
using Tests.Helpers.Management.Automation.Extensions;
using Tests.Mocks.Commands;
using Tests.Mocks.Management.Automation;

namespace Tests.Logging
{
	public class CommandLoggerTest
	{
		#region Methods

		private static CommandMock CreateCommand(ICommandRuntime? commandRuntime = null)
		{
			var command = new CommandMock
			{
				CommandRuntime = commandRuntime
			};

			command.PrepareForTest();

			return command;
		}

		[Fact]
		public async Task Log_ShouldUseTheCommandToWrite()
		{
			await Task.CompletedTask;

			var commandRuntime = new CommandRuntime2Mock();

			var command = CreateCommand(commandRuntime);
			var commandLoggerFactory = new CommandLoggerFactory(command);
			var commandLogger = commandLoggerFactory.CreateLogger(command.GetType());

			commandLogger.LogCritical("Critical log");
			commandLogger.LogCritical(new InvalidOperationException("Critical exception"), "Critical log");
			commandLogger.LogDebug("Debug log");
			commandLogger.LogDebug(new InvalidOperationException("Debug exception"), "Debug log");
			commandLogger.LogError("Error log");
			commandLogger.LogError(new InvalidOperationException("Error exception"), "Error log");
			commandLogger.LogInformation("Information log");
			commandLogger.LogInformation(new InvalidOperationException("Information exception"), "Information log");
			commandLogger.LogTrace("Trace log");
			commandLogger.LogTrace(new InvalidOperationException("Trace exception"), "Trace log");
			commandLogger.LogWarning("Warning log");
			commandLogger.LogWarning(new InvalidOperationException("Warning exception"), "Warning log");

			Assert.Equal(2, commandRuntime.Debugs.Count);
			Assert.Equal(4, commandRuntime.Errors.Count);
			Assert.Equal(2, commandRuntime.Informations.Count);
			Assert.Equal(2, commandRuntime.Verboses.Count);
			Assert.Equal(2, commandRuntime.Warnings.Count);
		}

		[Fact]
		public async Task LogInformation_IfTheCommandRuntimeIsICommandRuntime_ShouldThrowANotImplementedException()
		{
			await Task.CompletedTask;

			var commandRuntime = new CommandRuntimeMock();

			var command = CreateCommand(commandRuntime);
			var commandLoggerFactory = new CommandLoggerFactory(command);
			var commandLogger = commandLoggerFactory.CreateLogger(command.GetType());

			Assert.Throws<NotImplementedException>(() => commandLogger.LogInformation("Information log"));
		}

		#endregion
	}
}