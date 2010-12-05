using System;
using System.Linq;
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
    public const string VisitorSessionCount = "VisitorSessionCount";
    public const string SessionCount = "SessionCount";
    public const string HostCount = "HostCount";

    private static IList<Route> routes;

    static DBHelper()
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            ICriteria criteria = iSession.CreateCriteria<Route>();
            routes = criteria.List<Route>();
        }
    }
    //--------------------------------------------------------------------------------
    public static void CountVisitor(HttpRequest request, HttpSessionState session)
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

    public static IEnumerable<Route> Routes { get { return routes; } }

    public static Route GetRoute(string routeName)
    {
        var routes =
            from route in Routes
            where string.Compare(route.Name, routeName, StringComparison.InvariantCultureIgnoreCase) == 0
            select route;

        return routes.First<Route>();
    }
}


public class NHSessionManager
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
