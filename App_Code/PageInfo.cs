using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
namespace MTBScout
{
    /// <summary>
    /// Summary description for PageInfo
    /// </summary>
    public class PageInfo
    {
        public PageInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    public class ImageCache
    {
        public const int maxPerPage = 33;
        public string[] files;
        public string[] thumbUrls;
        public string[] reducedUrls;
        public string[] captions;
        public string[] fileUrls;
        public Size[] sizes;
        public int pages;

        public ImageCache(string imagesPath)
        {
            files = Directory.GetFiles(imagesPath, "*.jpg");
            thumbUrls = new string[files.Length];
            reducedUrls = new string[files.Length];
            captions = new string[files.Length];
            fileUrls = new string[files.Length];
            sizes = new Size[files.Length];
            pages = (int)Math.Ceiling((float)files.Length / (float)maxPerPage);
            string thumbDir = PathFunctions.GetThumbsFolder(imagesPath);
            if (!Directory.Exists(thumbDir))
                Directory.CreateDirectory(thumbDir);
            string reducedDir = PathFunctions.GetReducedFolder(imagesPath);
            if (!Directory.Exists(reducedDir))
                Directory.CreateDirectory(reducedDir);

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                sizes[i] = Helper.CreateThumbnail(file, 200/*800*/);
                thumbUrls[i] = PathFunctions.GetUrlFromPath(PathFunctions.GetThumbFile(file), true);

                Helper.CreateReduced(file);
                reducedUrls[i] = PathFunctions.GetUrlFromPath(PathFunctions.GetReducedFile(file), true);

                captions[i] = Helper.GetImageCaption(i, file);
                fileUrls[i] = PathFunctions.GetUrlFromPath(file, false);

            }
        }

    }

	public class UploadedImage
	{
		public string FileName { get; set; }
		public string Description { get; set; }
		public Bitmap Image { get; set; }


		public void AddToSession(string routeName)
		{
			List<UploadedImage> list = FromSession(routeName);

			//foreach (UploadedImage ui in list)
			//{
			//    if (ui.FileName == FileName)
			//    {
			//        list.Remove(ui);
			//        break;
			//    }
			//}

			list.Add(this);
		}

		public static List<UploadedImage> FromSession(string routeName)
		{
			string key = GetKey(routeName);
			List<UploadedImage> list = HttpContext.Current.Session[key] as List<UploadedImage>;
			if (list == null)
			{
				list = new List<UploadedImage>();
				HttpContext.Current.Session[key] = list;
			}
			return list;
		}



		private static string GetKey(string routeName)
		{
			return routeName + "ImageList";
		}
	}
}