using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
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
            files = Directory.Exists(imagesPath)
             ? Directory.GetFiles(imagesPath, "*.jpg")
             : new string[0];
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
				Size sz;
				using (Bitmap bmp = Helper.CreateThumbnail(file, 200/*800*/, true))
					sz = bmp.Size;
				sizes[i] = sz;
                thumbUrls[i] = PathFunctions.GetUrlFromPath(PathFunctions.GetThumbFile(file), true);

                Helper.CreateReduced(file);
                reducedUrls[i] = PathFunctions.GetUrlFromPath(PathFunctions.GetReducedFile(file), true);
				string title = Helper.GetImageTitle(file);
				if (string.IsNullOrEmpty(title))
					title = Helper.GetImageCaption(i, file);
				captions[i] = title;
                fileUrls[i] = PathFunctions.GetUrlFromPath(file, false);

            }
        }

    }

	public class UploadedImage 
	{
		private string description;
		private string file;

		public UploadedImage(string file, Stream s)
		{
			this.file = file;
			using (Bitmap image = (Bitmap)Bitmap.FromStream(s))
			{
				Description = Helper.GetImageTitle(image);
				image.Save(file);
			}
		}
		public UploadedImage(string file, string description)
		{
			this.file = file;
			this.description = description;
		}

		~UploadedImage()
		{
			if (file.StartsWith(PathFunctions.GetTempPath()))
				System.IO.File.Delete(file);
		}

		public string File
		{
			get { return file; }
		}

		public string Description
		{
			get { return description; }
			set { if (description != value) { description = value; IsModified = true; } }
		}
		
        public bool IsModified { get; set; }
        public bool IsDeleted { get; set; }

		public static UploadedImages FromSession(string routeName)
		{
			string key = GetKey(routeName);
			UploadedImages list = HttpContext.Current.Session[key] as UploadedImages;
			if (list == null)
			{
				list = new UploadedImages();
				HttpContext.Current.Session[key] = list;
				//carico i file già presenti su file system
                ImageCache cache = Helper.GetImageCache(PathFunctions.GetImagePathFromRouteName(routeName));
				for (int i = 0; i < cache.files.Length; i++)
				{
					UploadedImage img = new UploadedImage(
						cache.files[i],
						cache.captions[i]
						);
					list.Add(img);
				}
			}
			return list;
		}


		public static UploadedImage FromSession(string routeName, string file)
        {
			UploadedImages list = FromSession(routeName);
            foreach (UploadedImage img in list)
				if (img.file == file)
                    return img;
            return null;
        }

		private static string GetKey(string routeName)
		{
			return routeName + "IL";
		}


		public void SaveTo(string imageFolder)
		{
			if (IsDeleted)
            {
                string newFile = Path.Combine(imageFolder, Path.GetFileName(file));
                if (System.IO.File.Exists(newFile))
                    System.IO.File.Delete(newFile);
            }
            else if (IsModified)
			{
				string newFile = Path.Combine(imageFolder, Path.GetFileName(file));
				if (!System.IO.File.Exists(newFile))
				{
					System.IO.File.Move(file, newFile);
					file = newFile;
				}
				Helper.SetImageTitle(file, Description);
			}
		}

		internal void SaveTo(HttpResponse httpResponse)
		{
			using (Bitmap bmp = Helper.CreateThumbnail(file, 200, false))
			{
				httpResponse.ContentType = "image/jpeg";
				bmp.Save(httpResponse.OutputStream, ImageFormat.Jpeg);
			}
			
		}
	}

	public class UploadedImages : IEnumerable<UploadedImage>
	{
		List<UploadedImage> list = new List<UploadedImage>();

		public int Count { get { return list.Count;  } }

		public void Add(UploadedImage img)
		{
			list.Add(img);
		}

		public IEnumerator<UploadedImage> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}


		public UploadedImage GetAt(int i)
		{
			return list[i];
		}
	}
}