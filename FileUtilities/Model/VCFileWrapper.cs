﻿using FileUtilities.Helpers;
using Microsoft.VisualStudio.VCProjectEngine;
using System;

namespace FileUtilities.Model
{
	internal class VCFileWrapper : VCProjectItemWrapper, IMovable
    {
        public VCFileWrapper(VCFile file) : base(file)
        {
        }


        protected VCFile VCFile
        {
            get
            {
                return (VCFile)VCProjectItem;
            }
        }

        public override string FullPath
        {
            get
            {
                return VCFile.FullPath;
            }
        }

        public string RelativePath
        {
            get
            {
                string projectRoot = ContainingProject.GetProjectRoot();

                if (projectRoot == null)
                    return null;

                return PathHelper.GetRelativePath(projectRoot, FullPath);
            }
        }

        public void Move(ContainerWrapper newParent)
        {
            if (!VCFile.CanMove(newParent.VCProjectItem))
                throw new InvalidOperationException();

            VCFile.Move(newParent.VCProjectItem);
        }

        public void Remove()
        {
            VCFile.Remove();
        }

        public void SetFullPath(string path)
        {
            // This actually specifies the associated file
            VCFile.AddFile(path);
        }
    }
}
