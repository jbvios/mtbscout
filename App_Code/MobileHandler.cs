using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using MTBScout.Entities;
using System.Text;
using System.Globalization;
using System.Net;

namespace MTBScout
{
    [Serializable]
    struct T
    {
        public String name;
        public int lat;
        public int lon;
    }
    [Serializable]
    class R
    {
        public R(Route r)
        {
            if (r == null)
                return;
            title = r.Title;
            cycling = r.Cycling;
            difficulty = r.Difficulty;
            length = (float)Math.Round(r.Parser.Distance3D / 1000, 1);
            maxHeight = Convert.ToInt32(r.Parser.MaxElevation);
            minHeight = Convert.ToInt32(r.Parser.MinElevation);
            rank = DBHelper.GetMediumRank(r, out votes);
        }
        public string title = "";
        public int cycling = 0;
        public string difficulty = "";
        float length;
        int maxHeight;
        int minHeight;
        double rank;
        int votes;

    }
   
    public class MobileHandler : IHttpHandler
    {
        private static void SerializeJSON(HttpContext context, object o)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, o);
            context.Response.Write(Encoding.UTF8.GetString(ms.ToArray()));
        }
        public bool IsReusable
        {
            get { return true; }
        }
        [Serializable]
        class Test
        {
            public String Val = "OK";
        }
        public void ProcessRequest(HttpContext context)
        {

            try
            {
                string url = context.Request.QueryString["Url"];
                if (!string.IsNullOrEmpty(url))
                {
                    using (WebClient client = new WebClient())
                    {
                        using (StreamReader s = new StreamReader(client.OpenRead(url)))
                        {
                            context.Response.Write(s.ReadToEnd());
                        }
                    }
                    return;
                }
                string action = context.Request.QueryString["Action"];
                switch (action)
                {
                    case "test":
                        context.Response.Write(context.Request.QueryString["callback"] + "(");
                        SerializeJSON(context, new Test());//version
                        context.Response.Write(");");
                        context.Response.Write(0);
                        break;
                    case "getVersion":
                        context.Response.Write('1');//version
                        context.Response.Write('1');//OK
                        break;
                    case "getTracks":
                        {

                            double minlat = double.Parse(context.Request.QueryString["minlat"]) / 1000000.0;
                            double maxlat = double.Parse(context.Request.QueryString["maxlat"]) / 1000000.0;
                            double minlon = double.Parse(context.Request.QueryString["minlon"]) / 1000000.0;
                            double maxlon = double.Parse(context.Request.QueryString["maxlon"]) / 1000000.0;
                            List<T> rr = new List<T>();
                            foreach (Route r in DBHelper.Routes)
                                if (r.Parser.MediumPoint.lat < maxlat &&
                                    r.Parser.MediumPoint.lat > minlat &&
                                    r.Parser.MediumPoint.lon < maxlon &&
                                    r.Parser.MediumPoint.lon > minlon)
                                {
                                    T t = new T();
                                    t.name = r.Name;
                                    t.lat = Convert.ToInt32(r.Parser.MediumPoint.lat * 1e6);
                                    t.lon = Convert.ToInt32(r.Parser.MediumPoint.lon * 1e6);
                                    rr.Add(t);
                                }
                            SerializeJSON(context, rr);
                            context.Response.Write('1');//OK
                            break;
                        }
                    case "getTrackDetail":
                        {

                            string name = context.Request.QueryString["name"];
                            Route r = DBHelper.GetRoute(name);
                            SerializeJSON(context, new R(r));
                            context.Response.Write('1');//OK
                            break;
                        }
                    case "getTrackPoints":
                        {
                            StringBuilder sb = new StringBuilder();
                            string name = context.Request.QueryString["name"];
                            Route r = DBHelper.GetRoute(name);
                            foreach (GenericPoint gp in r.Parser.Tracks[0].Segments[0].ReducedPoints)
                            {
                                if (sb.Length > 0)
                                    sb.Append('-');
                                sb.Append(Convert.ToInt32(gp.lat * 1e6));
                                sb.Append('-');
                                sb.Append(Convert.ToInt32(gp.lon * 1e6));
                                sb.Append('-');
                                sb.Append(gp.ele.ToString("0.00", CultureInfo.InvariantCulture));
                            }
                            context.Response.Write(sb.ToString());
                            context.Response.Write('1');//OK
                            break;
                        }
                }

            }
            catch (Exception e)
            {
                Log.Add(e.ToString());
                SerializeJSON(context, e.Message);
                context.Response.Write('0');//ERROR
            }

        }

    }

}