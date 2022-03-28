using FileUtilities.Helpers;
using FileUtilities.Logic;
using FileUtilities.Model;
using System.ComponentModel.Design;
using System.Linq;

namespace FileUtilities.Integration.Commands
{
	class OrganizeOnDiskCommand : BaseCommand
    {
        public OrganizeOnDiskCommand(FileUtilitiesPackage package)
            : base(package, new CommandID(GuidList.GuidCommandSet, (int)CmdIDList.CmdIDOrganizeOnDisk))
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            var selection = SolutionHelper.GetSelectedItems(Package).ToList();

            Visible = selection.Any() && selection.All(item => !(item is VCProjectWrapper));
            Enabled = selection.All(item => item.ContainingProject.GetProjectRoot() != null);
        }

        protected override void OnExecute()
        {
            var selectedFiles = SolutionHelper.GetSelectedFiles(Package).ToList();

            foreach (var file in selectedFiles)
            {
                FileUtils.OrganizeFileOnDisk(file);
            }
        }
    }
}
