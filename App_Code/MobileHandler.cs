﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using MTBScout.Entities;
using System.Text;

namespace MTBScout
{
    [SerializableAttribute]
    struct T
    {
        public String name;
        public int lat;
        public int lon;
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
                            SerializeJSON(context, r);
                            break;
                        }
                }

            }
            catch (Exception e)
            {
                context.Response.Write(e.Message);
            }

        }

    }

}