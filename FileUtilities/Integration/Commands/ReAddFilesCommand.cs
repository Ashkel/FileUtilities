using FileUtilities.Helpers;
using FileUtilities.Model;
using System.ComponentModel.Design;
using System.Linq;

namespace FileUtilities.Integration.Commands
{
	class ReAddFilesCommand : BaseCommand
    {
        public ReAddFilesCommand(FileUtilitiesPackage package)
            : base(package, new CommandID(GuidList.GuidCommandSet, (int)CmdIDList.CmdIDReAddFiles))
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            var selection = SolutionHelper.GetSelectedItems(Package).ToList();

            Visible = selection.Any() && selection.All(item => !(item is VCProjectWrapper));
            Enabled = SolutionHelper.GetSelectedFiles(Package).Any();
        }

        protected override void OnExecute()
        {
            var selectedFiles = SolutionHelper.GetSelectedFiles(Package).ToList();

            foreach (var file in selectedFiles)
            {
                var path = file.FullPath;
                var parent = file.Parent;

                file.Remove();
                parent.AddFile(path);
            }
        }
    }
}
