using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.Security;

public partial class Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LiteralControl c = new LiteralControl("<META NAME=\"Description\" CONTENT=\"mountain bike MTB liguria montoggio casella valle GPS track scout bicicletta bike biker bikers pentemina scrivia\">");
        Page.Header.Controls.Add(c);


        SetLoginButton();
        /*if (Page.Request.Url.AbsolutePath.EndsWith("/whoweare.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiWhoWeAre);
        else*/
        if (Page.Request.Url.AbsolutePath.EndsWith("/events.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiEvents);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/Routes.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiRoutes);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/Links.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiLinks);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/User.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiUser);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/blog.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiBlog);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/Appointments.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiAppointments);
        else if (Page.Request.Url.AbsolutePath.EndsWith("/School.aspx", StringComparison.InvariantCultureIgnoreCase))
            HighLight(LiSchool);
    }

    private void SetLoginButton()
    {
        if (LoginState.User != null)
        {
            DisconnectButton.ToolTip = string.Format("Disconnetti l'utente {0}", LoginState.User.DisplayName);
            Disconnect.Src = "/Images/LogOff.png";
            Disconnect.Alt = DisconnectButton.ToolTip;
        }
        else
        {
            DisconnectButton.ToolTip = "Accedi";
            Disconnect.Src = "/Images/LogOn.png";
            Disconnect.Alt = DisconnectButton.ToolTip;
        }
    }

    private void HighLight(HtmlGenericControl c)
    {
        c.Attributes["class"] = "SelectedMenu";
        //c.Controls.AddAt(0, AddPanel());
        //c.Controls.Add(AddPanel());
    }

    private static Panel AddPanel()
    {
        Panel p = new Panel();
        p.Width = Unit.Pixel(20);
        p.Height = Unit.Pixel(40);
        p.Style[HtmlTextWriterStyle.Display] = "inline";
        p.Style[HtmlTextWriterStyle.PaddingBottom] = "20px";
        p.BackColor = Color.Red;
        return p;
    }
    protected void Disconnect_Click(object sender, EventArgs e)
    {
        if (LoginState.User != null)
        {
            LoginState.User = null;
            SetLoginButton();
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage();
        }
    }

}
