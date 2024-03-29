﻿using Microsoft.VisualStudio.VCProjectEngine;
using System.Collections.Generic;
using System.Linq;

namespace FileUtilities.Model
{
	abstract class ContainerWrapper : VCProjectItemWrapper
    {
        protected ContainerWrapper(VCProjectItem obj) : base(obj)
        {
        }

        protected abstract dynamic _Files { get; }
        protected abstract dynamic _Filters { get; }

        protected abstract VCFilter _AddFilter(string name);
        protected abstract VCFile _AddFile(string path);

        public IEnumerable<VCFileWrapper> Files
        {
            get
            {
                return (_Files as IVCCollection)
                    .Cast<VCFile>()
                    .Where(file => VCProjectItem == file.Parent)
                    .Select(file => new VCFileWrapper(file));
            }
        }

        public IEnumerable<VCFilterWrapper> Filters
        {
            get
            {
                return (_Filters as IVCCollection)
                    .Cast<VCFilter>()
                    .Where(filter => VCProjectItem == filter.Parent)
                    .Select(filter => new VCFilterWrapper(filter));
            }
        }

        public VCFilterWrapper AddFilter(string name)
        {
            return new VCFilterWrapper(_AddFilter(name));
        }

        public VCFileWrapper AddFile(string path)
        {
            return new VCFileWrapper(_AddFile(path));
        }

        public VCFilterWrapper GetFilter(string name, bool create = false)
        {
            foreach (VCFilterWrapper filter in Filters)
            {
                if (filter.Name.ToLower() == name.ToLower())
                    return filter;
            }

            if (create)
                return AddFilter(name);

            throw new KeyNotFoundException();
        }

        public VCFilterWrapper CreateFilterPath(string path)
        {
            return CreateFilterPath(path.Split('/', '\\'));
        }

        public VCFilterWrapper CreateFilterPath(string[] path)
        {
            string nextName = path[0];

            VCFilterWrapper nextFilter = GetFilter(nextName, true);
            string[] nextPath = path.Skip(1).ToArray();

            if (nextPath.Length == 0)
                return nextFilter;
            else
                return nextFilter.CreateFilterPath(nextPath);
        }

        public IEnumerable<VCFileWrapper> GetFilesRecursive()
        {
            var files = new List<VCFileWrapper>();
            GetFilesRecursive(files);
            return files;
        }

        private void GetFilesRecursive(List<VCFileWrapper> files)
        {
            files.AddRange(Files);

            foreach (ContainerWrapper child in Filters)
            {
                child.GetFilesRecursive(files);
            }
        }
    }
}