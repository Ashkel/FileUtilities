using FileUtilities.Helpers;
using FileUtilities.Logic;
using FileUtilities.Model;
using System.ComponentModel.Design;
using System.Linq;

namespace FileUtilities.Integration.Commands
{
	class RemoveEmptyFiltersCommand : BaseCommand
    {
        public RemoveEmptyFiltersCommand(FileUtilitiesPackage package)
            : base(package, new CommandID(GuidList.GuidCommandSet, (int)CmdIDList.CmdIDRemoveEmptyFilters))
        {

        }

        protected override void OnBeforeQueryStatus()
        {
            var selection = SolutionHelper.GetSelectedItems(Package).ToList();

            Visible = selection.Any() && selection.All(item => !(item is VCProjectWrapper));
            Enabled = selection.Any(item => item is ContainerWrapper);
        }

        protected override void OnExecute()
        {
            var selection = SolutionHelper.GetSelectedItems(Package)
                .Where(item => item is ContainerWrapper)
                .ToList();

            foreach (ContainerWrapper container in selection)
            {
                FileUtils.RemoveEmptyFilters(container);
            }
        }
    }
}
