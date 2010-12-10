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
	private static ReaderWriterLock routesLock = new ReaderWriterLock();
	private static IList<MTBUser> users;
	private static ReaderWriterLock userLock = new ReaderWriterLock();
	//--------------------------------------------------------------------------------
	static DBHelper()
	{
		using (ISession iSession = NHSessionManager.GetSession())
		{
			ICriteria criteria = iSession.CreateCriteria<Route>();
			routes = criteria.List<Route>();
			criteria = iSession.CreateCriteria<MTBUser>();
			users = criteria.List<MTBUser>();
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

			Expression<Func<Visitor, object>> expr = v => v.Visits;
			var criteria = iSession.CreateCriteria<Visitor>()
					.SetProjection(Projections.Sum(expr), Projections.Count(expr));
			object[] result = criteria.List<object[]>()[0];

			session[SessionCount] = Convert.ToInt64(result[0]);
			session[HostCount] = Convert.ToInt64(result[1]);
		}
	}

	//--------------------------------------------------------------------------------
	public static IEnumerable<Route> Routes { get { return routes; } }
	//--------------------------------------------------------------------------------
	public static IEnumerable<MTBUser> Users { get { return users; } }

	//--------------------------------------------------------------------------------
	public static Route GetRoute(string routeName)
	{
		using (AutoLock l = new AutoLock(routesLock, false))
		{
			var routes =
				from route in Routes
				where string.Compare(route.Name, routeName, StringComparison.InvariantCultureIgnoreCase) == 0
				select route;

			return routes.First<Route>();
		}
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
			using (AutoLock l = new AutoLock(userLock, true))
			{
				//se non è presente nella mia cache, lo aggiungo
				if (user != LoadUser(user.Id))
					users.Add(user);
			}
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
		using (AutoLock l = new AutoLock(userLock, false))
		{
			var users =
			   from user in Users
			   where user.OpenId == openId
			   select user;

			return users.First<MTBUser>();
		}
	}


	//--------------------------------------------------------------------------------
	/// <summary>
	/// recupera lo user a partire dal suo id interno
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static MTBUser LoadUser(int id)
	{
		using (AutoLock l = new AutoLock(userLock, false))
		{
			var users =
			   from user in Users
			   where user.Id == id
			   select user;

			return users.First<MTBUser>();
		}
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
