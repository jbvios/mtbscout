using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using System.Web.UI.HtmlControls;

public partial class AppointmentsPage : System.Web.UI.Page
{
    Appointment currentAppointment;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DBHelper.DeleteOldAppointments();
        }
        ButtonCreate.OnClientClick = string.Format("onSendPost('{0}');", Name.ClientID);
        LoadAppointments();
        FBLike.Attributes["src"] = string.Format(
           "http://www.facebook.com/widgets/like.php?href={0}",
           HttpUtility.UrlEncode(Page.Request.Url.ToString()));
    }

    private void LoadAppointments()
    {
        IList<Appointment> apps = DBHelper.GetAppointments();

        Appointments.DataSource = apps;
        Appointments.DataBind();
    }


    protected void ButtonSend_Click(object sender, EventArgs e)
    {
        try
        {
            RepeaterItem item = (RepeaterItem)((Button)sender).Parent.Parent;
            TextBox message = ((TextBox)item.FindControl("Message"));
            TextBox name = ((TextBox)item.FindControl("Name"));

            if (String.IsNullOrEmpty(message.Text) || string.IsNullOrEmpty(name.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "alert('Mancano alcuni campi obbligatori');", true);
                return;
            }
            Appointment p = DBHelper.GetAppointment(int.Parse(((Button)sender).CommandArgument));

            Post post = new Post();
            post.Name = name.Text;
            post.PostingDate = DateTime.Now;
            post.Message = message.Text;
            post.Ip = Request["REMOTE_HOST"];
            p.AppointmentPosts.Add(post);

            DBHelper.SaveAppointment(p);
            /*message.Text = "";
            //mando una mail agli utenti registrati
            string msg = string.Format("Ciao biker!<br/>L'utente {0} ha commentato l'appuntamento creato da {1}:<br/>{2}<br/><a target=\"appointment\" href=\"http://www.mtbscout.it/Appointments.aspx\">Visualizza pagina degli appuntamenti</a>",
                post.Name,
                p.Name,
                post.Message
                );
            foreach (MTBUser u in DBHelper.Users)
                if (u.SendMail)
                    Helper.SendMail(u.EMail, null, null, "Commenti ad appuntamento", msg, true);*/
            ClientScript.RegisterStartupScript(GetType(), "message", "alert('Messaggio salvato correttamente');", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(GetType(), "message", string.Format("alert('{0}');", ex.Message), true);
        }
        LoadAppointments();
    }
    protected void Appointments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        currentAppointment = (Appointment)e.Item.DataItem;
        if (currentAppointment == null)
            return;

        ImageButton btnDel = (ImageButton)e.Item.FindControl("ButtonDelete");
        btnDel.CommandArgument = currentAppointment.Id.ToString();
        btnDel.Visible = LoginState.IsAdmin();

        Repeater inner = (Repeater)e.Item.FindControl("Posts");
        List<Post> posts = new List<Post>();
        foreach (Post p in currentAppointment.AppointmentPosts)
            posts.Add(p);
        posts.Sort((a, b) => a.PostingDate.CompareTo(b.PostingDate));
        inner.DataSource = posts;
        inner.ItemDataBound += new RepeaterItemEventHandler(inner_ItemDataBound);
        inner.DataBind();
        TextBox txt = (TextBox)e.Item.FindControl("Name");
        Button btn = (Button)e.Item.FindControl("ButtonSend");
        btn.CommandArgument = currentAppointment.Id.ToString();
        btn.OnClientClick = string.Format("onSendPost('{0}');", txt.ClientID);
        btn = (Button)e.Item.FindControl("ButtonToggle");
        Panel comments = (Panel)e.Item.FindControl("CommentsPanel");
        btn.OnClientClick = string.Format("onToggle(this, '{0}');return false;", comments.ClientID);


    }

    void inner_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Post p = (Post)e.Item.DataItem;
        if (p == null)
            return;
        ImageButton btn = (ImageButton)e.Item.FindControl("ButtonDelete");
        btn.CommandArgument = currentAppointment.Id.ToString() + '.' + p.Id.ToString();
        btn.Visible = LoginState.IsAdmin();

    }
    protected void ButtonCreate_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(Message.Text) || string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(Date.Text))
        {
            ClientScript.RegisterStartupScript(GetType(), "message", "alert('Mancano alcuni campi obbligatori');", true);
            return;
        }
        try
        {
            Appointment p = new Appointment();
            p.AppointmentDate = DateTime.Parse(Date.Text);
            p.PostingDate = DateTime.Now;
            p.Name = Name.Text;
            p.Message = Message.Text;
            p.Ip = Request["REMOTE_HOST"];
            DBHelper.SaveAppointment(p);
            Message.Text = "";
            Date.Text = "";

            //mando una mail agli utenti registrati
            string msg = string.Format("Ciao biker!<br/>L'utente {0} ha creato un nuovo appuntamento:<br/>{1}<br/><a target=\"appointment\" href=\"http://www.mtbscout.it/Appointments.aspx\">Visualizza pagina degli appuntamenti</a>",
                p.Name,
                p.Message
                );
            foreach (MTBUser u in DBHelper.Users)
                if (u.SendMail)
                    Helper.SendMail(u.EMail, null, null, "Nuovo appuntamento", msg, true);

            ClientScript.RegisterStartupScript(GetType(), "message", "alert('Appuntamento creato correttamente');", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(GetType(), "message", string.Format("alert('{0}');", ex.Message), true);
        }
        LoadAppointments();
    }
    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string[] tokens = ((ImageButton)sender).CommandArgument.Split('.');
            DBHelper.DeletePost(int.Parse(tokens[0]), int.Parse(tokens[1]));
        }
        catch
        {
        }
        LoadAppointments();
    }
    protected void ButtonDeleteAppointment_Click(object sender, EventArgs e)
    {
        try
        {
            DBHelper.DeleteAppointment(int.Parse(((ImageButton)sender).CommandArgument));
        }
        catch
        {
        }
        LoadAppointments();
    }
}
