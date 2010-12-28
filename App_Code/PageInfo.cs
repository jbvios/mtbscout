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
		private string description;
		private Bitmap image;
		private string id;
		
		public UploadedImage(string id)
		{
			this.id = id;
		}
		public UploadedImage(string id, string description, Bitmap image)
		{
			this.id = id;
			this.description = description;
			this.image = image;
		}
		public string Id
		{
			get { return id; }
		}

		public string Description
		{
			get { return description; }
			set { if (description != value) { description = value; IsModified = true; } }
		}
		public Bitmap Image
		{
			get { return image; }
			set { if (image != value) { image = value; IsModified = true; } }
		}
        public bool IsModified { get; set; }

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
				//carico i file già presenti su file system
                ImageCache cache = Helper.GetImageCache(PathFunctions.GetImagePathFromRouteName(routeName));
                foreach (string file in cache.files)
                {
                    UploadedImage img = new UploadedImage(
						Path.GetFileName(file), 
						Helper.GetImageCaption(0, file),
						Bitmap.FromFile(file) as Bitmap);
                    list.Add(img);
                }
			}
			return list;
		}


        internal static UploadedImage FromSession(string routeName, string imageName)
        {
            List<UploadedImage> list = FromSession(routeName);
            foreach (UploadedImage img in list)
                if (img.Id == imageName)
                    return img;
            return null;
        }

		private static string GetKey(string routeName)
		{
			return routeName + "ImageList";
		}

    }
}