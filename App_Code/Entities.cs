using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
}