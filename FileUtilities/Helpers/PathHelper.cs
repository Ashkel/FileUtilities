using System;
using System.IO;


namespace FileUtilities.Helpers
{
	static class PathHelper
	{
		public static string GetRelativePath(string root, string absolutePath)
		{
			if (!root.EndsWith("\\") && !root.EndsWith("/"))
				root += Path.DirectorySeparatorChar;

			Uri uriRoot = new Uri(root);
			Uri uriPath = new Uri(absolutePath);
			Uri uriRelative = uriRoot.MakeRelativeUri(uriPath);

			return Uri.UnescapeDataString(uriRelative.ToString());
		}

		public static string GetAbsolutePath(string root, string relativePath)
		{
			if (!root.EndsWith("\\") && !root.EndsWith("/"))
				root += Path.DirectorySeparatorChar;

			UriBuilder builder = new UriBuilder(root);

			builder.Path += relativePath;

			string absolutePath = Uri.UnescapeDataString(builder.Uri.AbsolutePath);

			return absolutePath.Replace('/', Path.DirectorySeparatorChar);
		}
	}
}
