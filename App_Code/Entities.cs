using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;
using DotNetOpenAuth.OpenId;
namespace MTBScout.Entities
{
    public class Visitor
    {
        private string host;
        public string Host { get { return host; } set { host = value; } }
        private long visits;
        public long Visits { get { return visits; } set { visits = value; } }

        public Visitor()
        {

        }
        public Visitor(string host)
        {
            this.host = host;
            this.visits = 0;
        }

    }


    public class Route
    {
        private GpxParser parser = null;
        public GpxParser Parser
        {
            get
            {
                lock (this)
                {
                    if (parser == null)
                    { 
                        string path = PathFunctions.GetGpxPathFromRouteName(Name);
                        parser = Helper.GetGpxParser(path);
                    }
                    return parser;
                }
            }
        }
        private Int32 id;
        public Int32 Id
        {
            get { return id; }
        }

        public Route()
        {
            id = 0;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Page { get; set; }
        public string Image { get; set; }
        public int Cycling { get; set; }
        public string Difficulty { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                MethodInfo mi = pi.GetGetMethod();
                if (mi == null)
                    continue;
                sb.AppendFormat("{0}: {1}\r\n", pi.Name, mi.Invoke(this, null));
            }
            return sb.ToString();
        }
    }

    public class MTBUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
       
        
    }
}