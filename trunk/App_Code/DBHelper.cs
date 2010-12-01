using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Web.SessionState;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using MTBScout.Entities;
using MTBScout;
using NHibernate.Criterion;



/// <summary>
/// Summary description for DBHelper
/// </summary>
//================================================================================
public class DBHelper
{


    //--------------------------------------------------------------------------------
    public static string GetDBPath(HttpRequest req)
    {
        return Helper.IsDevelopment()
                ? HttpContext.Current.Server.MapPath("~/mdb-database/DebugMtbScout.mdb")
                : HttpContext.Current.Server.MapPath("~/mdb-database/MtbScout.mdb");
    }

    //--------------------------------------------------------------------------------
    public static OleDbConnection OpenConnection(HttpRequest req)
    {
        OleDbConnection conn = new OleDbConnection(
                    string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; User Id=admin; Password=", GetDBPath(req))
                    );
        conn.Open();
        return conn;
    }
    public const string VisitorSessionCount = "VisitorSessionCount";
    public const string SessionCount = "SessionCount";
    public const string HostCount = "HostCount";
    //--------------------------------------------------------------------------------
    public static void CountVisitor(HttpRequest request, HttpSessionState session, string mdbPath)
    {
        string host = request["REMOTE_HOST"];
        long visitorSessionCount;
        using (ISession iSession = NHSessionManager.GetSession())
        {
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                Visitor visitor = iSession.Get<Visitor>(host);
                if (visitor == null)
                    visitor = new Visitor(host);
                visitor.Visits++;
                iSession.SaveOrUpdate(visitor);
                iSession.Flush();
                visitorSessionCount = visitor.Visits;
                transaction.Commit();
                session[VisitorSessionCount] = visitorSessionCount;
                
            }
            var criteria = iSession.CreateCriteria<Visitor>()
                    .SetProjection(Projections.Sum("Visits"), Projections.Count("Visits"));
            object[] result = criteria.List<object[]>()[0];

            session[SessionCount] = Convert.ToInt64(result[0]);
            session[HostCount] = Convert.ToInt64(result[1]);
        }
    }
}


class NHSessionManager
{

    private static ISessionFactory factory;

    static NHSessionManager()
    {
        try
        {
            Configuration cfg = new Configuration();
            string configFile = Helper.IsDevelopment()
                ? @"hibernate.debug.cfg.xml"
                : @"hibernate.cfg.xml";
            string mappingPath = PathFunctions.GetMappingPath();
            cfg.Configure(Path.Combine(mappingPath, configFile));
            foreach (string file in Directory.GetFiles(mappingPath, "*.hbm.xml"))
                cfg.AddXmlFile(file);
            factory = cfg.BuildSessionFactory();
        }
        catch
        {

        }
    }

    public static ISession GetSession()
    {
        return factory.OpenSession();
    }
}
