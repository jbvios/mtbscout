using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Data.OleDb;
using System.Threading;
using MTBScout;
using System.Web.Caching;
using System.Drawing;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    private static int sessions = 0;

    public static int GetActiveSessionCount()
    {
        return sessions;
    }
    public static void IncreaseSessions()
    {
        Interlocked.Increment(ref sessions);
    }
    public static void DecreaseSessions()
    {
        Interlocked.Decrement(ref sessions);
    }

    public static Size CreateThumbnail(string file, int size)
    {
        string thumbFile = PathFunctions.GetThumbFile(file);
        if (File.Exists(thumbFile))
        {
            using (Bitmap bmp = new Bitmap(thumbFile))
                return bmp.Size;
        }
        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
        {
            int w;
            int h;
            GetNewSize(size, bmp, out w, out h);
            using (System.Drawing.Image img = new System.Drawing.Bitmap(bmp, w, h))
            {
                img.Save(thumbFile);
                return img.Size;
            }
        }
    }
    public static void Resize(string file, int size)
    {
        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
        {
            int w;
            int h;
            GetNewSize(size, bmp, out w, out h);
            if (w >= bmp.Width || h > bmp.Height)
                return;
            using (System.Drawing.Image img = new System.Drawing.Bitmap(bmp, w, h))
            {
                bmp.Dispose();
                img.Save(file);
            }
        }
    }

    public static void CreateReduced(string file)
    {
        string reduced = PathFunctions.GetReducedFile(file);
        if (File.Exists(reduced))
            return;

        File.Copy(file, reduced, false);
        Helper.Resize(reduced, 800);
    }
    private static void GetNewSize(int size, System.Drawing.Bitmap bmp, out int w, out int h)
    {

        if (bmp.Height < bmp.Width)
        {
            w = size;
            h = w * bmp.Height / bmp.Width;
        }
        else
        {
            h = size;
            w = h * bmp.Width / bmp.Height;
        }
    }



    public static void AddUserInfoToSession(OleDbDataReader reader)
    {
        HttpContext.Current.Session["UserId"] = Convert.ToInt32(reader["ID"]);
        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Params["Return"]);
    }

    public static void TestLogin()
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect(string.Format("~/Login.aspx?Return={0}", HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString())));
        }
    }

    public static GpxParser GetGpxParser(string gpxPath)
    {
        gpxPath = gpxPath.ToLower().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        GpxParser parser = HttpContext.Current.Cache[gpxPath] as GpxParser;
        if (parser == null)
        {
            parser = new GpxParser();
            parser.Parse(gpxPath);
            HttpContext.Current.Cache.Add(
                gpxPath,
                parser,
                new CacheDependency(new string[] { gpxPath, HttpContext.Current.Server.MapPath("Map.aspx") }),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);
        }
        return parser;
    }
    public static ImageCache GetImageCache(string imagesPath)
    {
        ImageCache cache = HttpContext.Current.Cache[imagesPath] as ImageCache;
        if (cache == null)
        {
            cache = new ImageCache(imagesPath);
            HttpContext.Current.Cache.Add(
                imagesPath,
                cache,
                new CacheDependency(new string[] { imagesPath }),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);
        }
        return cache;
    }

    public static string GenerateProfileFile(string filesRelPath)
    {
        string gpxPath = PathFunctions.RootPath + filesRelPath;
        //calcolo la cartella che contiene le tracce
        string folder = Path.GetFileName(Path.GetDirectoryName(gpxPath));

        string profileFileRelPath = PathFunctions.GetProfileUrl(folder);
        string profileFileFullPath = HttpContext.Current.Server.MapPath(profileFileRelPath);
        if (!File.Exists(profileFileFullPath))
        {
            ProfileGenerator gen = new ProfileGenerator();
            gen.GenerateProfile(Helper.GetGpxParser(gpxPath), profileFileFullPath);
        }
        return profileFileRelPath;

    }

    public static string GetImageCaption(int prog, string file)
    {
        string caption = System.IO.Path.GetFileNameWithoutExtension(file);
        if (caption.IndexOf("-") != 3)
            caption = prog.ToString("000");
        else if (!char.IsDigit(caption[0]) || !char.IsDigit(caption[1]) || !char.IsDigit(caption[1]))
            caption = prog.ToString("000");
        else
            caption = caption.Substring(4);
        return caption;
    }
    public static DateTime GetCreationDate(string file)
    {
        Bitmap i = null;
        try
        {
            i = Image.FromFile(file) as Bitmap;

            EXIFextractor ext = new EXIFextractor(ref i, Environment.NewLine);
            string s = ext["DTDigitized"] as string;

            if (s != null)
            {
                int idxStart = 0, idxEnd = 0;
                idxEnd = s.IndexOf(":", idxStart);
                string year = s.Substring(idxStart, idxEnd - idxStart);

                idxStart = idxEnd + 1;
                idxEnd = s.IndexOf(":", idxStart);
                string month = s.Substring(idxStart, idxEnd - idxStart);

                idxStart = idxEnd + 1;
                idxEnd = s.IndexOf(" ", idxStart);
                string day = s.Substring(idxStart, idxEnd - idxStart);

                idxStart = idxEnd + 1;
                idxEnd = s.IndexOf(":", idxStart);
                string hour = s.Substring(idxStart, idxEnd - idxStart);

                idxStart = idxEnd + 1;
                idxEnd = s.IndexOf(":", idxStart);
                string minute = s.Substring(idxStart, idxEnd - idxStart);

                idxStart = idxEnd + 1;
                idxEnd = s.IndexOf("\0", idxStart);
                string second = s.Substring(idxStart, idxEnd - idxStart);

                return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));

            }
        }
        finally
        {
            if (i != null)
                i.Dispose();
        }


        FileInfo fi = new FileInfo(file);
        return fi.LastWriteTime;
    }

    public static int GetCountryCode(double lat, double lon)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false",
                    lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    lon.ToString(System.Globalization.CultureInfo.InvariantCulture));
                using (Stream s = client.OpenRead(url))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(s);
                    XmlNode localityNode = doc.SelectSingleNode("GeocodeResponse/result[type/text()='locality']");
                    if (localityNode != null)
                    {
                        XmlNode countryNode = localityNode.SelectSingleNode("address_component[type/text()='locality']/long_name/text()");
                        XmlNode provinceNode = localityNode.SelectSingleNode("address_component[type/text()='administrative_area_level_2']/short_name/text()");

                        if (provinceNode != null && countryNode != null)
                        {
                            return GetCountryCode(countryNode.Value, provinceNode.Value);
                        }
                    }
                }
                return 0;
            }
        }
        catch
        {
            return 0;
        }
    }

    static int GetCountryCode(string country, string province)
    {
        Dictionary<string, int> codes = GetCountryCodes();
        int code = 0;
        codes.TryGetValue(Mengle(country, province), out code);
        return code;
    }

    static Dictionary<string, int> GetCountryCodes()
    {
        string file = Path.Combine(PathFunctions.RootPath, "resources\\ilmeteo_codici_comuni.csv");

        Dictionary<string, int> codes = HttpContext.Current.Cache[file] as Dictionary<string, int>;
        if (codes == null)
        {
            codes = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            using (StreamReader sr = new StreamReader(file))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] tokens = s.Split(';');
                    string code = tokens[0];
                    string name = tokens[1];
                    string province = tokens[2];
                    codes.Add(Mengle(name, province), int.Parse(code));
                }
            }

            HttpContext.Current.Cache.Add(
                file,
                codes,
                new CacheDependency(new string[] { file }),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);

        }
        return codes;
    }

    private static string Mengle(string country, string province)
    {
        return country + ", " + province;
    }


    internal static bool IsDevelopment()
    {
        return string.Compare(PathFunctions.RootPath, "c:\\mtbscout", StringComparison.InvariantCultureIgnoreCase) == 0;
    }
}

