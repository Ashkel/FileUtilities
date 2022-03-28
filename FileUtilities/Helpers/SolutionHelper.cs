using EnvDTE;
using FileUtilities.Model;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileUtilities.Helpers
{
	static class SolutionHelper
    {
        public static IEnumerable<VCProjectItemWrapper> GetSelectedItems(FileUtilitiesPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var item in GetSelectedUIHierarchyItems(package))
            {
                VCProjectItem vcProjectItem = null;

                if (item.Object is ProjectItem)
                    vcProjectItem = (item.Object as ProjectItem).Object as VCProjectItem;

                if (item.Object is Project)
                    vcProjectItem = (item.Object as Project).Object as VCProjectItem;

                if (vcProjectItem != null)
                    yield return WrapperFactory.FromVCProjectItem(vcProjectItem);
            }
        }

        public static IEnumerable<VCProjectWrapper> GetSelectedProjects(FileUtilitiesPackage package)
        {
            return GetSelectedItems(package)
                .Where(item => item is VCProjectWrapper)
                .Cast<VCProjectWrapper>();
        }

        public static IEnumerable<VCFileWrapper> GetSelectedFiles(FileUtilitiesPackage package)
        {
            foreach (var item in GetSelectedItems(package))
            {
                if (item is VCFileWrapper)
                    yield return (item as VCFileWrapper);

                if (item is ContainerWrapper)
                    foreach (var child in (item as ContainerWrapper).GetFilesRecursive())
                        yield return child;
            }
        }

        public static VCProjectWrapper GetProjectOfSelection(FileUtilitiesPackage package)
        {
            var projects = GetSelectedItems(package)
                .Select(item => item.ContainingProject)
                .ToList();

            if (projects.Count() == 0)
                return null;

            if (projects.Any(project => !project.Equals(projects[0])))
                return null;

            return projects[0];
        }

        public static string GetSelectedDirectory(FileUtilitiesPackage package)
        {
            var selection = GetSelectedItems(package)
                .ToList();

            if (selection.Count() != 1)
                return null;

            if (selection[0] is VCProjectWrapper)
                return (selection[0] as VCProjectWrapper).GetProjectRoot();

            if (selection[0] is VCFilterWrapper)
                return selection[0].FullPath;

            return null;
        }

        private static UIHierarchy GetSolutionExplorer(FileUtilitiesPackage package)
        {
            try
            {
                return package.IDE.ToolWindows.SolutionExplorer;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private static IEnumerable<UIHierarchyItem> GetSelectedUIHierarchyItems(FileUtilitiesPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            UIHierarchy solutionExplorer = GetSolutionExplorer(package);

            if (solutionExplorer == null)
                return new UIHierarchyItem[0];

            return ((object[])solutionExplorer.SelectedItems)
                .Cast<UIHierarchyItem>();
        }
    }
}
