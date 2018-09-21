using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.IO;
using RegionOrebroLan.PowerShell.IO.Commands;

namespace RegionOrebroLan.PowerShell.IO.UnitTests.Commands
{
	[TestClass]
	public class GetFileSystemEntryPathMatchCommandTest
	{
		#region Methods

		[CLSCompliant(false)]
		protected internal virtual void InvokeCommand(Cmdlet command)
		{
			var result = command.Invoke().GetEnumerator();

			result.MoveNext();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessRecord_IfIncludeContainNullValues_ShouldThrowAnArgumentException()
		{
			try
			{
				this.InvokeCommand(new GetFileSystemEntryPathMatchCommand {Include = new string[] {null}});
			}
			catch(ArgumentException argumentException)
			{
				if(string.Equals(argumentException.ParamName, "Include", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public void ProcessRecord_IfIncludeIsEmpty_ShouldNotThrowAnException()
		{
			this.InvokeCommand(new GetFileSystemEntryPathMatchCommand {Include = Array.Empty<string>()});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRecord_IfIncludeIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				this.InvokeCommand(new GetFileSystemEntryPathMatchCommand {Include = null});
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(string.Equals(argumentNullException.ParamName, "Include", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public void ProcessRecord_ShouldWriteTheResultFromTheFileSystemEntryMatcherToThePipeline()
		{
			IEnumerable<string> actualResult = null;
			IEnumerable<string> expectedResult = new[] {"A", "B", "C"};
			var include = new[] {"First", "Second", "Third"};

			var commandRuntimeMock = new Mock<ICommandRuntime>();
			commandRuntimeMock.Setup(commandRuntime => commandRuntime.WriteObject(It.IsAny<object>())).Callback((object sendToPipeline) => { actualResult = (IEnumerable<string>) sendToPipeline; });

			var fileSystemEntryMatcherMock = new Mock<IFileSystemEntryMatcher>();
			fileSystemEntryMatcherMock.Setup(fileSystemEntryMatcher => fileSystemEntryMatcher.GetPathMatches(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>())).Returns(() => expectedResult);

			var getFileSystemEntryPathMatchCommand = new GetFileSystemEntryPathMatchCommand(fileSystemEntryMatcherMock.Object)
			{
				CommandRuntime = commandRuntimeMock.Object,
				Include = include
			};

			this.InvokeCommand(getFileSystemEntryPathMatchCommand);

			Assert.IsTrue(expectedResult.SequenceEqual(actualResult, StringComparer.Ordinal));
		}

		#endregion
	}
}