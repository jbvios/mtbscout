using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Footer : System.Web.UI.UserControl
{
    private Color backColor = Color.FromArgb(0x9933);
    public Color BackColor { get { return backColor; } set { backColor = value; } }

    private Color color = Color.White;
    public Color Color { get { return color; } set { color = value; } }

    private Unit top;

    public Unit Top
    {
        get { return top; }
        set { top = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MainFooterPanel.BackColor = BackColor;
        MainFooterPanel.ForeColor = Color;
        MainFooterPanel.Style[HtmlTextWriterStyle.Top] = Top.ToString();
        if (LoginState.User != null)
        {
            Disconnect.Visible = true;
            Disconnect.ToolTip = string.Format("Disconnetti l'utente {0}", GetUser());
        }
        else
        {
            Disconnect.Visible = false;
        }
    }
    protected string GetUserString()
    {
        if (LoginState.User != null)
        {
            return GetUser() + " - ";
        }
        else
        {
            return "";
        }
    }

    private static string GetUser()
    {
        return string.IsNullOrEmpty(LoginState.User.Nickname)
            ? LoginState.User.Name + " " + LoginState.User.Surname
            : LoginState.User.Nickname;
    }
    protected long GetVisitorNumber()
    {
        return (long)Session[DBHelper.HostCount];
    }

    protected long GetSessionNumber()
    {
        return (long)Session[DBHelper.SessionCount];
    }

    protected long GetVisitorSessionNumber()
    {
        return (long)Session[DBHelper.VisitorSessionCount];
    }
    protected void Disconnect_Click(object sender, EventArgs e)
    {
        LoginState.User = null;
        Disconnect.Visible = false;
    }
}
