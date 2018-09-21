using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.PowerShell.IO.Commands;

namespace RegionOrebroLan.PowerShell.IO.IntegrationTests.Commands
{
	[TestClass]
	public class GetFileSystemEntryPathMatchCommandTest : BasicTest
	{
		#region Fields

		private const string _projectRelativeTestResourceDirectoryPath = @"Commands\Test-resources";

		#endregion

		#region Properties

		protected internal override string ProjectRelativeTestResourceDirectoryPath => _projectRelativeTestResourceDirectoryPath;

		#endregion

		#region Methods

		protected internal virtual IEnumerable<string> InvokeAndGetResult(IEnumerable<string> include)
		{
			var getFileSystemEntryPathMatchCommand = new GetFileSystemEntryPathMatchCommand
			{
				Include = include.ToArray()
			};

			this.InvokeCommand(getFileSystemEntryPathMatchCommand);

			// ReSharper disable PossibleNullReferenceException
			var output = (IEnumerable<object>) getFileSystemEntryPathMatchCommand.CommandRuntime.GetType().GetField("output", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(getFileSystemEntryPathMatchCommand.CommandRuntime);
			// ReSharper restore PossibleNullReferenceException

			return (IEnumerable<string>) output.First();
		}

		[TestMethod]
		public void ProcessRecord_ShouldWriteTheResultFromTheFileMatcherToThePipeline()
		{
			var include = new[] {this.GetTestResourcePath(@"**\*.log")};
			var actual = this.InvokeAndGetResult(include);
			var expected = new List<string>
			{
				this.GetTestResourcePath(@"Folder\First.log"),
				this.GetTestResourcePath(@"Folder\Second.log"),
				this.GetTestResourcePath("First.log"),
				this.GetTestResourcePath("Second.log")
			};
			Assert.IsTrue(expected.SequenceEqual(actual, StringComparer.Ordinal));

			include = new[] {this.GetTestResourcePath(@"**\*.txt")};
			actual = this.InvokeAndGetResult(include);
			expected = new List<string>
			{
				this.GetTestResourcePath(@"Folder\First.txt"),
				this.GetTestResourcePath(@"Folder\Second.txt"),
				this.GetTestResourcePath(@"Folder\Third.txt"),
				this.GetTestResourcePath("First.txt"),
				this.GetTestResourcePath("Second.txt"),
				this.GetTestResourcePath("Third.txt")
			};
			Assert.IsTrue(expected.SequenceEqual(actual, StringComparer.Ordinal));

			include = new[] {this.GetTestResourcePath(@"**\*.*")};
			actual = this.InvokeAndGetResult(include);
			expected = new List<string>
			{
				this.GetTestResourcePath(@"Folder\First.log"),
				this.GetTestResourcePath(@"Folder\First.txt"),
				this.GetTestResourcePath(@"Folder\Second.log"),
				this.GetTestResourcePath(@"Folder\Second.txt"),
				this.GetTestResourcePath(@"Folder\Third.txt"),
				this.GetTestResourcePath("First.log"),
				this.GetTestResourcePath("First.txt"),
				this.GetTestResourcePath("Second.log"),
				this.GetTestResourcePath("Second.txt"),
				this.GetTestResourcePath("Third.txt")
			};
			Assert.IsTrue(expected.SequenceEqual(actual, StringComparer.Ordinal));

			include = new[] {this.GetTestResourcePath(@"**\*")};
			actual = this.InvokeAndGetResult(include);
			Assert.IsTrue(expected.SequenceEqual(actual, StringComparer.Ordinal));
		}

		#endregion
	}
}