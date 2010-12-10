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

		public string Name { get; set; }
		public string Title { get; set; }
		public string Page { get; set; }
		public string Image { get; set; }
		public int Cycling { get; set; }
		public string Difficulty { get; set; }
		public string Description { get; set; }
		public int OwnerId { get; set; }

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
}