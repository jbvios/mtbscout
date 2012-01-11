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
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId;
using MTBScout.Entities;
using NHibernate;
using NHibernate.Criterion;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Drawing.Imaging;
using System.Text;
using System.Reflection;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    private static int sessions = 0;
    private static Dictionary<string, int> countryCodes = null;
    private const int ImageTitleId = 0x010E;
    private const int DigitizedId = 0x9004;
    private const int GpsLatitudeRefId = 0x1;
    private const int GpsLatitudeId = 0x2;
    private const int GpsLongitudeRefId = 0x3;
    private const int GpsLongitudeId = 0x4;
    public static Dictionary<string, string> DifficultyMap = new Dictionary<string, string>();
    public static Dictionary<string, Color> DifficultyMapColor = new Dictionary<string, Color>();

    static Helper()
    {
        DifficultyMap["TC"] = "(turistico) percorso su strade sterrate dal fondo compatto e scorrevole, di tipo carrozzabile";
        DifficultyMap["MC"] = "(per cicloescursionisti di media capacità tecnica) percorso su sterrate con fondo poco sconnesso o poco irregolare (tratturi, carrarecce…) o su sentieri con fondo compatto e scorrevole";
        DifficultyMap["BC"] = "(per cicloescursionisti di buone capacità tecniche) percorso su sterrate molto sconnesse o su mulattiere e sentieri dal fondo piuttosto sconnesso ma abbastanza scorrevole oppure compatto ma irregolare, con qualche ostacolo naturale (per es. gradini di roccia o radici)";
        DifficultyMap["OC"] = "(per cicloescursionisti di ottime capacità tecniche) percorso su mulattiere o sentieri dal fondo molto sconnesso e/o molto irregolare, con presenza significativa di ostacoli";
        DifficultyMap["EC"] = "(massimo livello per il cicloescursionista... estremo! ma possibilmente da evitare in gite sociali) percorso su sentieri molto irregolari, caratterizzati da gradoni e ostacoli in continua successione, che richiedono tecniche di tipo trialistico";

        DifficultyMapColor["TC"] = Color.Yellow;
        DifficultyMapColor["MC"] = Color.PaleGreen;
        DifficultyMapColor["BC"] = Color.Orange;
        DifficultyMapColor["OC"] = Color.OrangeRed;
        DifficultyMapColor["EC"] = Color.Violet;

        string file = Path.Combine(PathFunctions.RootPath, "resources\\ilmeteo_codici_comuni.csv");

        countryCodes = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        using (StreamReader sr = new StreamReader(file))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                string[] tokens = s.Split(';');
                string code = tokens[0];
                string name = tokens[1];
                string province = tokens[2];
                countryCodes.Add(Mengle(name, province), int.Parse(code));
            }
        }

    }
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

    public static Bitmap CreateThumbnail(string file, int size, bool save)
    {
        string thumbFile = PathFunctions.GetThumbFile(file);
        if (File.Exists(thumbFile))
        {
            return new Bitmap(thumbFile);
        }
        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
        {
            Bitmap img = CreateThumbnail(bmp, size);
            if (save)
                img.Save(thumbFile);
            return img;
        }
    }
    public static Bitmap CreateThumbnail(Bitmap original, int size)
    {
        int w;
        int h;
        GetNewSize(size, original, out w, out h);
        return new System.Drawing.Bitmap(original, w, h);
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

    public static GpxParser GetGpxParser(string gpxPath)
    {
        gpxPath = gpxPath.ToLower().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        GpxParser parser = HttpContext.Current.Cache[gpxPath] as GpxParser;
        if (parser == null)
        {
           if (!File.Exists(gpxPath))
                return null;
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

    public static void ClearImageCache(string imagesPath)
    {
        ImageCache.ClearCache(imagesPath);
        HttpContext.Current.Cache[imagesPath] = null; 
    }
    public static ImageCache GetImageCache(string imagesPath)
    {
        ImageCache cache = HttpContext.Current.Cache[imagesPath] as ImageCache;
        if (cache == null)
        {
            cache = ImageCache.Create(imagesPath);
            if (Directory.Exists(imagesPath))
            {
                HttpContext.Current.Cache.Add(
                                imagesPath,
                                cache,
                                new CacheDependency(new string[] { imagesPath }),
                                Cache.NoAbsoluteExpiration,
                                Cache.NoSlidingExpiration,
                                CacheItemPriority.Normal,
                                null);
            }
        }
        return cache;
    }

    public static string GenerateProfileFile(string gpxPath)
    {
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



    public static void GetImageInfos(
        string file, 
        out DateTime creationTime, 
        out double latitudeRef,
        out double latitude,
        out double longitudeRef,
        out double longitude)
    {
        Bitmap i = null;
		try
		{
			i = Image.FromFile(file) as Bitmap;

			PropertyItem piDate = i.GetPropertyItem(DigitizedId);//PropertyTagExifDTDigitized
			string s = Encoding.ASCII.GetString(piDate.Value);

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

				creationTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));
				latitudeRef = 0.0;
				latitude = 0.0;
				longitudeRef = 0.0;
				longitude = 0.0;
				return;
			}
		}
		catch
		{ 
		}
        finally
        {
            if (i != null)
                i.Dispose();
        }


        FileInfo fi = new FileInfo(file);
        creationTime = fi.LastWriteTime;
        latitudeRef = 0.0;
        latitude = 0.0;
        longitudeRef = 0.0;
        longitude = 0.0;
    }

	public static void SetCreationTime(string file, DateTime creationTime)
	{

		byte[] buff;
		using (FileStream fs = File.OpenRead(file))
		{
			buff = new byte[(int)fs.Length];
			fs.Read(buff, 0, (int)fs.Length);

		}
		using (MemoryStream ms = new MemoryStream(buff))
		{
			using (Bitmap bmp = (Bitmap)Bitmap.FromStream(ms))
			{
				SetCreationTime(bmp, creationTime);
				bmp.Save(file);
			}
		}
	}
	public static void SetCreationTime(
	   Bitmap i,
	   DateTime creationTime)
	{
		try
		{
			PropertyItem piDate = i.GetPropertyItem(DigitizedId);//PropertyTagExifDTDigitized
			//2010:12:11 14:15:19
			string s = string.Format("{0}:{1}:{2} {3}:{4}:{5}",
				creationTime.Year.ToString("0000"),
				creationTime.Month.ToString("00"),
				creationTime.Day.ToString("00"),
				creationTime.Hour.ToString("00"),
				creationTime.Minute.ToString("00"),
				creationTime.Second.ToString("00"));

			byte[] buff = Encoding.UTF8.GetBytes(s);
			piDate.Value = new byte[buff.Length + 1];
			buff.CopyTo(piDate.Value, 0);
			piDate.Value[buff.Length] = 0;
			piDate.Len = piDate.Value.Length;
			i.SetPropertyItem(piDate);
		}
		catch
		{
		}
	}
    public static string GetImageTitle(string file)
    {
        using (Bitmap bmp = new Bitmap(file))
            return GetImageTitle(bmp);
    }
    public static string GetImageTitle(Image img)
    {
        try
        {
            PropertyItem piDesc = img.GetPropertyItem(ImageTitleId);
            return Encoding.UTF8.GetString(piDesc.Value, 0, piDesc.Value.Length - 1);
        }
        catch
        {
            return "";
        }
    }


    public static void SetImageTitle(string file, string title)
    {

        byte[] buff;
        using (FileStream fs = File.OpenRead(file))
        {
            buff = new byte[(int)fs.Length];
            fs.Read(buff, 0, (int)fs.Length);

        }
        using (MemoryStream ms = new MemoryStream(buff))
        {
            using (Bitmap bmp = (Bitmap)Bitmap.FromStream(ms))
            {
                SetImageTitle(bmp, title);
                bmp.Save(file);
            }
        }
    }
    public static void SetImageTitle(Image img, string title)
    {
        try
        {

            PropertyItem piDate = img.PropertyItems[0];
            piDate.Id = ImageTitleId;
            piDate.Type = 2;
            byte[] buff = Encoding.UTF8.GetBytes(title);
            piDate.Value = new byte[buff.Length + 1];
            buff.CopyTo(piDate.Value, 0);
            piDate.Value[buff.Length] = 0;
            piDate.Len = piDate.Value.Length;

            img.SetPropertyItem(piDate);
        }
        catch (Exception)
        {
        }

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
        return countryCodes;
    }

    private static string Mengle(string country, string province)
    {
        return country + ", " + province;
    }

    internal static bool IsDevelopment()
    {
        return string.Compare(PathFunctions.RootPath, "c:\\mtbscout", StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public static void SendMail(string to, string cc, string bcc, string subject, string body, bool html)
    {
        string[] ccAr = cc == null ? new string[0] : new string[] { cc };
        string[] bccAr = bcc == null ? new string[0] : new string[] { bcc };
        SendMail(new string[] { to }, ccAr, bccAr, subject, body, html);
    }
    public static void SendMail(string[] to, string[] cc, string[] bcc, string subject, string body, bool html)
    {
        if (IsDevelopment())
            return;

        SmtpClient client = new SmtpClient("smtp.aruba.it");

        MailMessage msg = new MailMessage();
        msg.Body = body;
        msg.Subject = subject;
        msg.Sender = new MailAddress("info@mtbscout.it");
        msg.From = new MailAddress("info@mtbscout.it");
		msg.IsBodyHtml = html;
        foreach (string s in to)
            msg.To.Add(new MailAddress(s));
        foreach (string s in cc)
            msg.CC.Add(new MailAddress(s));
        foreach (string s in bcc)
            msg.Bcc.Add(new MailAddress(s));

        client.Send(msg);
    }

    public static void DisableAppDomainRestartOnDelete()
    {
        PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        object o = p.GetValue(null, null);
        FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
        object monitor = f.GetValue(o);
        MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
        m.Invoke(monitor, new object[] { }); 
    }
   
    public static void GenerateTrackCode(GpxParser parser, HttpResponse response, bool addScriptTags)
    {
        if (addScriptTags)
            response.Write("<script type=\"text/javascript\">\r\n");
        response.Write("function addTracks(){\r\n");

        if (parser != null)
        {
            int trackIndex = 0;
            foreach (Track trk in parser.Tracks)
            {
                response.Write(string.Format(@"
                t = {0}; 
                track = [];
                trk_info[t] = track;
                track['name'] = '{1}';
                track['desc'] = '{2}'; 
                track['clickable'] = true;
                track['width'] = 3; 
                track['opacity'] = 0.9;
                track['outline_color'] = '#000000';
                track['outline_width'] = 0;
                track['fill_color'] = '#E60000'; 
                track['fill_opacity'] = 0;
                
                trkSeg = [];
                trk_segments[t] = trkSeg;",
                   ++trackIndex,
                   trk.Name,
                   trk.Description));

                foreach (TrackSegment seg in trk.Segments)
                {
                    TrackPoint[] reducedPoints = seg.ReducedPoints;
                    for (int i = 0; i < reducedPoints.Length - 1; i++)
                    {
                        TrackPoint p1 = reducedPoints[i];
                        TrackPoint p2 = reducedPoints[i + 1];

                        string color = ColorProvider.GetColorString(p1.ele, parser.MinElevation, parser.MaxElevation);
                        //devo usare InvariantCulture per avere il punto come separatore dei decimali
                        response.Write(string.Format(
                            "trkSeg.push({{ color:'{0}', points:[ [{1},{2}], [{3},{4}] ] }});\r\n",
                            color,
                            p1.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            p1.lon.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            p2.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            p2.lon.ToString(System.Globalization.CultureInfo.InvariantCulture)
                            ));

                    }
                }

                response.Write("GV_Draw_Track(t);");
            }
        }
        response.Write("}\r\n");
        if (addScriptTags)
            response.Write("</script>\r\n");
    }


    public static string FormatDate(DateTime date)
    {
        return date.ToString("dddd dd MMMM yyyy");
    }
    public static string FormatDateTime(DateTime date)
    {
        return date.ToString("dddd dd MMMM yyyy alle ore HH:mm");
    }
}
internal class AutoLock : IDisposable
{
    private static readonly TimeSpan timeout = TimeSpan.FromMinutes(1);
    ReaderWriterLock l;
    bool forWrite;
    public AutoLock(ReaderWriterLock l, bool forWrite)
    {
        this.l = l;

        if (this.forWrite = forWrite)
            l.AcquireWriterLock(timeout);
        else
            l.AcquireReaderLock(timeout);

    }
    public void Dispose()
    {
        if (this.forWrite)
            l.ReleaseWriterLock();
        else
            l.ReleaseReaderLock();
    }

}

/// <summary>
/// Strong-typed bag of session state.
/// </summary>
public class LoginState
{
    public static bool TestLogin()
    {
        if (User == null)
        {
            FormsAuthentication.RedirectToLoginPage();
            return false;
        }

        return true;
    }

	public static bool IsAdmin()
    {
        if (User == null)
			return false;
		return User.OpenId == "https://www.google.com/accounts/o8/id?id=AItOawnJeTkR-nPcd4YIwRRGQhCKWlMar4Xjyq8";
	}
    private const string MTBUserIdentifier = "MTBUser";
    public static MTBUser User
    {
        get
        {
            return HttpContext.Current.Session[MTBUserIdentifier] as MTBUser;
        }
        set
        {
            HttpContext.Current.Session[MTBUserIdentifier] = value;
        }
    }
    private const string MTBNewUserIdentifier = "NewMTBUser";
    public static MTBUser NewUser
    {
        get
        {
            return HttpContext.Current.Session[MTBNewUserIdentifier] as MTBUser;
        }
        set
        {
            HttpContext.Current.Session[MTBNewUserIdentifier] = value;
        }
    }
}