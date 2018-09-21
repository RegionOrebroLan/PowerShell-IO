using System;
using System.IO;
using System.Management.Automation;

namespace RegionOrebroLan.PowerShell.IO.IntegrationTests
{
	public abstract class BasicTest
	{
		#region Fields

		private static DirectoryInfo _projectDirectory;
		private DirectoryInfo _testResourceDirectory;

		#endregion

		#region Properties

		// ReSharper disable PossibleNullReferenceException
		protected internal virtual DirectoryInfo ProjectDirectory => _projectDirectory ?? (_projectDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent);
		// ReSharper restore PossibleNullReferenceException

		protected internal abstract string ProjectRelativeTestResourceDirectoryPath { get; }
		protected internal virtual DirectoryInfo TestResourceDirectory => this._testResourceDirectory ?? (this._testResourceDirectory = new DirectoryInfo(Path.Combine(this.ProjectDirectory.FullName, this.ProjectRelativeTestResourceDirectoryPath)));

		#endregion

		#region Methods

		protected internal virtual string GetTestResourcePath(string fileSystemEntryName)
		{
			return Path.Combine(this.TestResourceDirectory.FullName, fileSystemEntryName);
		}

		[CLSCompliant(false)]
		protected internal virtual void InvokeCommand(Cmdlet command)
		{
			var result = command.Invoke().GetEnumerator();

			result.MoveNext();
		}

		#endregion
	}
}