using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using RegionOrebroLan.IO;

namespace RegionOrebroLan.PowerShell.IO.Commands
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsCommon.Get, "FileSystemEntryPathMatch")]
	public class GetFileSystemEntryPathMatchCommand : Cmdlet
	{
		#region Fields

		private static readonly IFileSystemEntryMatcher _fileSystemEntryMatcher = new FileSystemEntryMatcher();

		#endregion

		#region Constructors

		public GetFileSystemEntryPathMatchCommand() : this(_fileSystemEntryMatcher) { }

		protected internal GetFileSystemEntryPathMatchCommand(IFileSystemEntryMatcher fileSystemEntryMatcher)
		{
			this.FileSystemEntryMatcher = fileSystemEntryMatcher ?? throw new ArgumentNullException(nameof(fileSystemEntryMatcher));
		}

		#endregion

		#region Properties

		[Parameter(Position = 2)]
		public virtual string DirectoryPath { get; set; }

		[Parameter(Position = 1)]
		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual string[] Exclude { get; set; }

		protected internal virtual IFileSystemEntryMatcher FileSystemEntryMatcher { get; }

		[Parameter(Mandatory = true, Position = 0)]
		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual string[] Include { get; set; }

		#endregion

		#region Methods

		[SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
		protected override void ProcessRecord()
		{
			try
			{
				if(this.Include == null)
					throw new ArgumentNullException(nameof(this.Include));

				var pathMatches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

				foreach(var includePattern in this.Include)
				{
					if(includePattern == null)
						throw new ArgumentException("Include can not contain null values.", nameof(this.Include));

					IEnumerable<string> paths;

					try
					{
						paths = this.FileSystemEntryMatcher.GetPathMatches(this.DirectoryPath, this.Exclude, includePattern).ToArray();
					}
					catch(Exception exception)
					{
						throw new InvalidOperationException("Could not get files from the file-matcher.", exception);
					}

					foreach(var pathMatch in paths)
					{
						pathMatches.Add(pathMatch);
					}
				}

				this.WriteObject(pathMatches.ToArray());
			}
			catch(MissingMethodException missingMethodException)
			{
				throw new InvalidOperationException("A method is missing. Make sure you have .NET Framework 4.6, or higher, installed.", missingMethodException);
			}
		}

		#endregion
	}
}