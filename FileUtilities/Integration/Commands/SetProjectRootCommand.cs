using FileUtilities.Helpers;
using FileUtilities.Model;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;

namespace FileUtilities.Integration.Commands
{
	class SetProjectRootCommand : BaseCommand
	{
        public SetProjectRootCommand(FileUtilitiesPackage package)
            : base(package, new CommandID(GuidList.GuidCommandSet, (int)CmdIDList.CmdIDSetProjectRoot))
        {

        }

        protected override void OnBeforeQueryStatus()
        {
            Visible = SolutionHelper.GetProjectOfSelection(Package) != null;
            Enabled = true;
        }

        protected override void OnExecute()
        {
            VCProjectWrapper project = SolutionHelper.GetProjectOfSelection(Package);

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = project.GetProjectDirectory();
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExtensionSettings settings = SettingsManager.GetSettings(project);
                string projectDir = project.GetProjectDirectory();
                settings.RelativeProjectRoot = PathHelper.GetRelativePath(projectDir, dlg.SelectedPath + Path.DirectorySeparatorChar);
                SettingsManager.SaveSettings(project);
            }
        }
    }
}
