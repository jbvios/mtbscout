using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;

namespace MTBScout
{
    public static class PathFunctions
    {
        const string workingDataUrl = "~\\Public\\WorkingData";
        static string workingDataPath = HttpContext.Current.Server.MapPath(workingDataUrl);
        public static string RootPath = HttpContext.Current.Server.MapPath("~").TrimEnd(Path.DirectorySeparatorChar);
        
        public static string GetProfileUrl(string routeFolder)
        {
            return string.Format("{0}\\Routes\\{1}\\profile.png", workingDataUrl, routeFolder);
        }

        public static string GetImagePathFromGpx(string gpxFile)
        {
            return Path.Combine(Path.GetDirectoryName(gpxFile), "Images");
        }
      
        public static string GetWorkingPath(string sourcePath)
		{
			string relPath = GetRelativePath(sourcePath);
			return workingDataPath + relPath;
		}

        public static string GetThumbsFolder(string imagesFolder)
        {
			string path = GetWorkingPath(imagesFolder);
            return Path.Combine(Path.GetDirectoryName(path), "Thumbs");
        }

		public static string GetThumbFile(string file)
        {
            string folder = Path.GetDirectoryName(file);
            folder = GetThumbsFolder(folder);
            return Path.Combine(folder, Path.GetFileName(file));
        }
        public static string GetReducedFolder(string imagesFolder)
        {
			string path = GetWorkingPath(imagesFolder);
			return Path.Combine(Path.GetDirectoryName(path), "Reduced");
        }
        public static string GetReducedFile(string file)
        {
            string folder = Path.GetDirectoryName(file);
            folder = GetReducedFolder(folder);
            return Path.Combine(folder, Path.GetFileName(file));
        }
        public static string GetRelativePath(string fullPath)
        {
            return fullPath.Substring(RootPath.Length);
        }

        public static string GetFullUrlFromPath(string path)
        {
            string relUrl = GetUrlFromPath(path, false);
            Uri u = HttpContext.Current.Request.Url;
            UriBuilder ub = new UriBuilder();
            ub.Scheme = u.Scheme;
            ub.Host = u.Host;
            ub.Port = u.Port;

            if (Helper.IsDevelopment())
                ub.Path = "/MTBScout/" + relUrl.TrimStart('/');
            else
                ub.Path = relUrl.TrimStart('/');
            return ub.ToString();
        }
        public static string GetUrlFromPath(string path, bool addRootToken)
        {
            return (addRootToken ? "~" : "") + 
                GetRelativePath(path).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        public static string GetRelativePath(this Page page, string file)
        {
            return GetRelativePath(page.MapPath(file));
        }

        public static string GetFullPath(HttpRequest request, string subPath)
        {
            Uri u = request.Url;
            UriBuilder ub = new UriBuilder();
            ub.Scheme = u.Scheme;
            ub.Host = u.Host;
            ub.Port = u.Port;

            if (Helper.IsDevelopment())
                ub.Path = "/MTBScout/" + subPath.TrimStart('/');
            else
                ub.Path = subPath.TrimStart('/');
            return ub.ToString();
        }

        public static string GetRoutePathFromName(string name)
        {
            return Path.Combine(RootPath, string.Format("Routes\\{0}", name));
        }

        internal static string GetMappingPath()
        {
            return Path.Combine(RootPath, "Mappings");
        }
    }
}