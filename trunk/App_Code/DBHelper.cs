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
using System.Linq.Expressions;
using System.Threading;



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
    private static IList<MTBUser> users;
    //--------------------------------------------------------------------------------
    static DBHelper()
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            LoadRoutes(iSession);
            LoadUsers(iSession);
        }
    }

    private static void LoadUsers(ISession iSession)
    {
        ICriteria criteria = iSession.CreateCriteria<MTBUser>();
        users = criteria.List<MTBUser>();
    }

    private static void LoadRoutes(ISession iSession)
    {
        ICriteria criteria = iSession.CreateCriteria<Route>();
        routes = criteria.List<Route>();
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

            Expression<Func<Visitor, object>> expr = v => v.Visits;
            var criteria = iSession.CreateCriteria<Visitor>()
                    .SetProjection(Projections.Sum(expr), Projections.Count(expr));
            object[] result = criteria.UniqueResult<object[]>();

            session[SessionCount] = Convert.ToInt64(result[0]);
            session[HostCount] = Convert.ToInt64(result[1]);
        }
    }

    //--------------------------------------------------------------------------------
    public static IEnumerable<Route> Routes { get { return routes; } }
    //--------------------------------------------------------------------------------
    public static IEnumerable<MTBUser> Users { get { return users; } }

    //--------------------------------------------------------------------------------
    public static IEnumerable<Route> GetRoutes(int ownerId)
    {
        var rr =
            from route in Routes
            where route.OwnerId == ownerId
            select route;

        return rr;
    }
    //--------------------------------------------------------------------------------
    public static Route GetRoute(string routeName)
    {
        var rr =
            from route in Routes
            where string.Compare(route.Name, routeName, StringComparison.InvariantCultureIgnoreCase) == 0
            select route;

        if (rr.Count() == 0)
            return null;
        return rr.First<Route>();
    }

    //--------------------------------------------------------------------------------
    public static void SaveUser(MTBUser user)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.SaveOrUpdate(user);
                iSession.Flush();
                transaction.Commit();
            }
            DBHelper.LoadUsers(iSession);
        }
    }
    //--------------------------------------------------------------------------------
    public static void SaveRank(int userId, int routeId, byte rank)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Rank r = GetRank(userId, routeId, iSession);
            if (r == null)
            {
                r = new Rank();
                r.RouteId = routeId;
                r.UserId = userId;
            }
            if (r.RankNumber != rank)
            {
                r.RankNumber = rank;
                using (ITransaction transaction = iSession.BeginTransaction())
                {
                    iSession.SaveOrUpdate(r);
                    iSession.Flush();
                    transaction.Commit();
                }
            }
        }
    }
    public static Rank GetRank(int userId, int routeId)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            return GetRank(userId, routeId, iSession);
        }
    }
    public static IList<Rank> GetRanks(int routeId)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Rank, object>> expr = rt => rt.RouteId;
            var criteria = iSession.CreateCriteria<Rank>();
            criteria.Add(Restrictions.Eq(Projections.Property(expr), routeId));

            return criteria.List<Rank>();
        }
    }
    private static Rank GetRank(int userId, int routeId, ISession iSession)
    {
        Expression<Func<Rank, object>> expr = rt => rt.RouteId;
        var criteria = iSession.CreateCriteria<Rank>();
        criteria.Add(Restrictions.Eq(Projections.Property(expr), routeId));
        expr = rt => rt.UserId;
        criteria.Add(Restrictions.Eq(Projections.Property(expr), userId));

        return criteria.UniqueResult<Rank>();
    }

    public static double GetMediumRank(Route r, out int voteNumber)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Rank, object>> expr = rt => rt.RouteId;
            var criteria = iSession.CreateCriteria<Rank>();
            criteria.Add(Restrictions.Eq(Projections.Property(expr), r.Id));

            expr = rt => rt.RankNumber;

            criteria.SetProjection(Projections.Sum(expr), Projections.Count(expr));
            object[] result = criteria.UniqueResult<object[]>();
            voteNumber = (int)result[1];

            return voteNumber == 0
                ? 0.0
                : Convert.ToDouble(result[0]) / (double)voteNumber;
        }
    }
    //--------------------------------------------------------------------------------
    /// <summary>
    /// recupera lo user a partire dal suo openid
    /// </summary>
    /// <param name="openId"></param>
    /// <returns></returns>
    public static MTBUser LoadUser(string openId)
    {
        var uu =
           from user in Users
           where user.OpenId == openId
           select user;
        return uu.Count() == 0 ? null : uu.First<MTBUser>();
    }


    //--------------------------------------------------------------------------------
    /// <summary>
    /// recupera lo user a partire dal suo id interno
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static MTBUser LoadUser(int id)
    {

        var uu =
           from user in Users
           where user.Id == id
           select user;

        return uu.Count() == 0 ? null : uu.First<MTBUser>();
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
