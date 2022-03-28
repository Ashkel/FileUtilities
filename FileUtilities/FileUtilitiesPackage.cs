using EnvDTE80;
using FileUtilities.Integration;
using FileUtilities.Integration.Commands;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace FileUtilities
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideMenuResource(1000, 1)]
	[Guid(GuidList.GuidPackageString)]
	public sealed class FileUtilitiesPackage : AsyncPackage
	{
		#region Fields

		private DTE2 _ide;
		private OleMenuCommandService _menuCommandService;

		#endregion


		#region Properties

		public DTE2 IDE
		{
			get
			{
				return _ide ?? (_ide = GetService(typeof(SApplicationObject)) as DTE2);
			}
		}

		public OleMenuCommandService MenuCommandService
		{
			get
			{
				return _menuCommandService ?? (_menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService);
			}
		}

		#endregion


		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
		/// <param name="progress">A provider for progress updates.</param>
		/// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			//base.Initialize();

			RegisterCommands();
		}

		#endregion


		#region Helper methods

		private void RegisterCommands()
		{
			var menuCommandService = MenuCommandService;

			if (menuCommandService != null)
			{
				menuCommandService.AddCommand(new SetProjectRootCommand(this));
				menuCommandService.AddCommand(new RemoveEmptyFiltersCommand(this));
				menuCommandService.AddCommand(new ReAddFilesCommand(this));
				menuCommandService.AddCommand(new OrganizeOnDiskCommand(this));
				menuCommandService.AddCommand(new OrganizeInProjectCommand(this));
			}
		}

		#endregion
	}
}
