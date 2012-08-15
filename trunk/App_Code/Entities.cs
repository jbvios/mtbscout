using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;
using DotNetOpenAuth.OpenId;
using NHibernate;
using System.Linq.Expressions;
using NHibernate.Criterion;
using System.IO;
using System.Collections;
using Iesi.Collections;
using System.Xml;
using System.Xml.Serialization;
namespace MTBScout.Entities
{
    internal class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
    public class Entity
    {
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                MethodInfo mi = pi.GetGetMethod();
                if (mi == null)
                    continue;
                object[] o = pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                string propDesc = (o.Length == 1)
                    ? ((DescriptionAttribute)o[0]).Description
                    : pi.Name;
                sb.AppendFormat("{0}: {1}\r\n", propDesc, mi.Invoke(this, null));
            }
            return sb.ToString();
        }
    }
    public class Visitor : Entity
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

    public class Route : Entity
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

        private string name;
        public string Name { get { return name; } set { name = value; } }
        
        private string title;
        public string Title { get { return title; } set { title = value; } }

        private string page;
        public string Page { get { return page; } set { page = value; } }
        
        private string image;
        public string Image { get { return image; } set { image = value; } }

        private int cycling;
        public int Cycling { get { return cycling; } set { cycling = value; } }

        private string difficulty;
        public string Difficulty { get { return difficulty; } set { difficulty = value; } }

        private string description;
        public string Description { get { return description; } set { description = value; } }

        private int ownerId;
        public int OwnerId { get { return ownerId; } set { ownerId = value; } }

        public string GetRouteUrl(bool editMode)
        {
            string routeFolderPath = PathFunctions.GetRoutePathFromName(Name);
            string url =
                editMode
                ? PathFunctions.GetUrlFromPath(PathFunctions.EditRoutePage, false).Replace("'", "\\'") + "?Route=" + Name
                : (string.IsNullOrEmpty(Page)
                    ? PathFunctions.GetUrlFromPath(PathFunctions.RoutesPage, false).Replace("'", "\\'") + "?Route=" + Name
                    : PathFunctions.GetUrlFromPath(Path.Combine(routeFolderPath, Page), false).Replace("'", "\\'"));
            return url;
        }
    }

    public class MTBUser : Entity
    {
        public enum GenderType { Male = 0, Female = 1, Unspecified = 2 }
        public MTBUser()
        {
            SendMail = true;
            Gender = GenderType.Unspecified;
            BirthDate = DateTime.MinValue;
        }
        [Description("Codice identificativo")]
        public int Id { get; set; }
        [Description("Identificativo OpenId")]
        public string OpenId { get; set; }
        [Description("Nome")]
        public string Name { get; set; }
        [Description("Cognome")]
        public string Surname { get; set; }
        [Description("Nickname")]
        public string Nickname { get; set; }
        [Description("Indirizzo di posta elettronica")]
        public string EMail { get; set; }
        [Description("Data di nascita")]
        public DateTime BirthDate { get; set; }
        [Description("Genere (numero)")]
        public Int16 GenderNumber { get; set; }
        [Description("Codice postale")]
        public string Zip { get; set; }
        [Description("Prima bici")]
        public string Bike1 { get; set; }
        [Description("Seconda bici")]
        public string Bike2 { get; set; }
        [Description("Terza bici")]
        public string Bike3 { get; set; }
        [Description("Mandatemi mail")]
        public bool SendMail { get; set; }
        [Description("Genere (tipo)")]
        public GenderType Gender { get { return (GenderType)GenderNumber; } set { GenderNumber = (short)value; } }
        [Description("Nome visualizzato")]
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(Nickname)
                    ? Name + " " + Surname
                    : Nickname;
            }
        }


    }

    public class EventSubscriptor : Entity
    {
        public EventSubscriptor()
        {
            Gender = MTBScout.Entities.MTBUser.GenderType.Unspecified;
            BirthDate = DateTime.MinValue;
        }
        [Description("Codice identificativo")]
        public int Id { get; set; }
        [Description("Codice identificativo evento")]
        public int EventId { get; set; }
        [Description("Codice identificativo utente")]
        public int UserId { get; set; }
        [Description("Nome")]
        public string Name { get; set; }
        [Description("Cognome")]
        public string Surname { get; set; }
        [Description("Indirizzo di posta elettronica")]
        public string EMail { get; set; }
        [Description("Data di nascita")]
        public DateTime BirthDate { get; set; }
        [Description("Genere (numero)")]
        public Int16 GenderNumber { get; set; }
        [Description("Genere (tipo)")]
        public MTBScout.Entities.MTBUser.GenderType Gender { get { return (MTBScout.Entities.MTBUser.GenderType)GenderNumber; } set { GenderNumber = (short)value; } }
        [Description("Genere (descrizione)")]
        public string GenderDescription
        {
            get
            {
                switch (Gender)
                {
                    case MTBUser.GenderType.Female:
                        return "Femmina";
                    case MTBUser.GenderType.Male:
                        return "Maschio";
                    default:
                        return "Non specificato";
                }
            }
        }

        [Description("Nome visualizzato")]
        public string DisplayName { get { return Name + " " + Surname; } }
    }

    public class Rank : Entity
    {
        private Int32 id;
        public Int32 Id
        {
            get { return id; }
        }

        public Rank()
        {
            id = 0;
        }

        public Int32 RouteId { get; set; }
        public Int32 UserId { get; set; }
        public Byte RankNumber { get; set; }
    }

    public class Post : Entity
    {
        private Int32 id;
        public Int32 Id
        {
            get { return id; }
        }

        public Post()
        {
            id = 0;
        }
        public String Name { get; set; }
        public String Message { get; set; }
        public String FormattedMessage { get { return Message.Replace("\n", "<br/>"); } }
        public String Ip { get; set; }
        public String UserId { get; set; }
        public DateTime PostingDate { get; set; }
    }
    public class Appointment : Entity
    {
        private Int32 id;
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        public Appointment()
        {
            id = 0;
        }
        public String Name { get; set; }
        public String Message { get; set; }
        public String FormattedMessage { get { return Message.Replace("\n", "<br/>"); } }
        public String Ip { get; set; }
        public String UserId { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public ISet AppointmentPosts { get; set; }
    }
}