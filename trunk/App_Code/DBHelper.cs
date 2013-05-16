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
using System.Collections;
using System.Reflection.Emit;



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
    public static IList ExecQuery(string queryString)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            ISQLQuery query = iSession.CreateSQLQuery(queryString);
            IList genericList = query.List();
            if (genericList.Count == 0)
                return new ArrayList();
            Type t = CreateType(genericList[0] as object[]);
            if (t == null)
                return new ArrayList();

            for (int i = 0; i < genericList.Count; i++)
                genericList[i] = CreateTypeInstance(t, genericList[i] as object[]);
            return genericList;
        }
    }
    //--------------------------------------------------------------------------------
    private static Type CreateType(object[] fields)
    {
        if (fields == null)
            return null;

        // create a dynamic assembly and module 
        AssemblyName assemblyName = new AssemblyName();
        assemblyName.Name = "tmpAssembly";
        AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder module = assemblyBuilder.DefineDynamicModule("tmpModule");

        // create a new type builder
        TypeBuilder typeBuilder = module.DefineType("BindableRowCellCollection", TypeAttributes.Public | TypeAttributes.Class);
        int idx = 0;
        // Loop over the attributes that will be used as the properties names in out new type
        foreach (object obj in fields)
        {
            string propertyName = "Field_" + (++idx).ToString("00");

            // Generate a private field
            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, obj.GetType(), FieldAttributes.Private);
            // Generate a public property
            PropertyBuilder property =
                typeBuilder.DefineProperty(propertyName,
                                 PropertyAttributes.None,
                                 obj.GetType(),
                                 new Type[] { typeof(string) });

            // The property set and property get methods require a special set of attributes:

            MethodAttributes GetSetAttr =
                MethodAttributes.Public |
                MethodAttributes.HideBySig;

            // Define the "get" accessor method for current private field.
            MethodBuilder currGetPropMthdBldr =
                typeBuilder.DefineMethod("get_value",
                                           GetSetAttr,
                                           obj.GetType(),
                                           Type.EmptyTypes);

            // Intermediate Language stuff...
            ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
            currGetIL.Emit(OpCodes.Ldarg_0);
            currGetIL.Emit(OpCodes.Ldfld, field);
            currGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for current private field.
            MethodBuilder currSetPropMthdBldr =
                typeBuilder.DefineMethod("set_value",
                                           GetSetAttr,
                                           null,
                                           new Type[] { obj.GetType() });

            // Again some Intermediate Language stuff...
            ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
            currSetIL.Emit(OpCodes.Ldarg_0);
            currSetIL.Emit(OpCodes.Ldarg_1);
            currSetIL.Emit(OpCodes.Stfld, field);
            currSetIL.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            property.SetGetMethod(currGetPropMthdBldr);
            property.SetSetMethod(currSetPropMthdBldr);
        }

        // Generate our type
        return typeBuilder.CreateType();
    }
    private static object CreateTypeInstance(Type generetedType, object[] fields)
    {
        // Now we have our type. Let's create an instance from it:
        object generetedObject = Activator.CreateInstance(generetedType);

        // Loop over all the generated properties, and assign the values from our XML:
        PropertyInfo[] properties = generetedType.GetProperties();

        int propertiesCounter = 0;

        // Loop over the values that we will assign to the properties
        foreach (object obj in fields)
        {
            properties[propertiesCounter].SetValue(generetedObject, obj, null);
            propertiesCounter++;
        }

        return generetedObject;
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
    public static void DeleteRoute(Route route)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.Delete(route);
                iSession.Flush();
                transaction.Commit();
            }

        }
        new Thread(() =>
            {
                using (ISession ss = NHSessionManager.GetSession())
                {
                    DBHelper.LoadRoutes(ss);
                }
            }).Start();
    }
    //--------------------------------------------------------------------------------
    public static void SaveRoute(Route route)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.SaveOrUpdate(route);
                iSession.Flush();
                transaction.Commit();
            }

        }
        new Thread(() =>
                {
                    using (ISession ss = NHSessionManager.GetSession())
                    {
                        DBHelper.LoadRoutes(ss);
                    }
                }).Start();
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
            new Thread(() =>
            {
                using (ISession ss = NHSessionManager.GetSession())
                {
                    DBHelper.LoadUsers(ss);
                }
            }).Start();
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

    public static EventSubscriptor[] GetSubscriptors(int userId, int eventId)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<EventSubscriptor, object>> expr = rt => rt.EventId;
            var criteria = iSession.CreateCriteria<EventSubscriptor>();
            criteria.Add(Restrictions.Eq(Projections.Property(expr), eventId));
            expr = rt => rt.UserId;
            criteria.Add(Restrictions.Eq(Projections.Property(expr), userId));

            return criteria.List<EventSubscriptor>().ToArray();
        }
    }
    //--------------------------------------------------------------------------------
    public static void DeleteSubscriptor(EventSubscriptor subscriptor)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.Delete(subscriptor);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }
    //--------------------------------------------------------------------------------
    public static void SaveSubscriptor(EventSubscriptor subscriptor)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.SaveOrUpdate(subscriptor);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }
    //--------------------------------------------------------------------------------
    public static EventSubscriptor LoadSubscriptor(string email)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<EventSubscriptor, object>> expr = rt => rt.EMail;
            var criteria = iSession.CreateCriteria<EventSubscriptor>();
            criteria.Add(Restrictions.Eq(Projections.Property(expr), email));
            return criteria.UniqueResult<EventSubscriptor>();
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


    public static IList<Appointment> GetAppointments()
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Appointment, object>> expr = rt => rt.AppointmentDate;
            var criteria = iSession.CreateCriteria<Appointment>();
            criteria.AddOrder(Order.Asc(Projections.Property(expr)));
            IList<Appointment> apps = criteria.List<Appointment>();
            
            return apps;
        }
    }

    public static void SaveAppointment(Appointment appointment)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.SaveOrUpdate(appointment);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }

    public static void DeleteOldAppointments()
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Appointment, object>> expr = rt => rt.AppointmentDate;
            var criteria = iSession.CreateCriteria<Appointment>();
            criteria.Add(Restrictions.Lt(Projections.Property(expr), DateTime.Now - TimeSpan.FromDays(3)));
            //aggiungo l'utente al database, oppure lo aggiorno
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                foreach (Appointment app in criteria.List<Appointment>())
                    iSession.Delete(app);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }

    public static Appointment GetAppointment(int id)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Appointment, object>> expr = rt => rt.Id;
            var criteria = iSession.CreateCriteria<Appointment>();
            criteria.Add(Restrictions.Eq("id", id));
            return criteria.UniqueResult<Appointment>();
        }
    }
    public static void DeletePost(int appointmentId, int postId)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Appointment, object>> expr = rt => rt.Id;
            var criteria = iSession.CreateCriteria<Appointment>();
            criteria.Add(Restrictions.Eq("id", appointmentId));
            Appointment app = criteria.UniqueResult<Appointment>();
            foreach (Post p in app.AppointmentPosts)
            {
                if (p.Id == postId)
                {
                    app.AppointmentPosts.Remove(p);
                    break;
                }
            }
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.SaveOrUpdate(app);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }
    public static void DeleteAppointment(int appointmentId)
    {
        using (ISession iSession = NHSessionManager.GetSession())
        {
            Expression<Func<Appointment, object>> expr = rt => rt.Id;
            var criteria = iSession.CreateCriteria<Appointment>();
            criteria.Add(Restrictions.Eq("id", appointmentId));
            Appointment app = criteria.UniqueResult<Appointment>();
            using (ITransaction transaction = iSession.BeginTransaction())
            {
                iSession.Delete(app);
                iSession.Flush();
                transaction.Commit();
            }
        }
    }

    public static void BackupDB(int i)
    {
        try
        {
            string path = PathFunctions.DBPath;
            if (File.Exists(path))
                File.Copy(path, path + ".bkp" + i, true);
        }
        catch (Exception ex)
        {
            Log.Add(ex.ToString());
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
