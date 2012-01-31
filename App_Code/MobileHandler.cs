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
   /* [Serializable]
    struct P
    {
        public P(GenericPoint gp)
        {
            lat = Convert.ToInt32(gp.lat * 1e6);
            lon = Convert.ToInt32(gp.lon * 1e6);
            ele = gp.ele;
        }
        public int lat;
        public int lon;
        public double ele;
    }*/

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

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                string action = context.Request.QueryString["Action"];
                switch (action)
                {
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
                            break;
                        }
                    case "getTrackDetail":
                        {

                            string name = context.Request.QueryString["name"];
                            Route r = DBHelper.GetRoute(name);
                            SerializeJSON(context, new R(r));
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
                            break;
                        }
                }

            }
            catch (Exception e)
            {
                Log.Add(e.ToString());
                SerializeJSON(context, e.Message);
            }

        }

    }

}