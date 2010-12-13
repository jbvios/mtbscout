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
        
    }
    protected string GetUserString()
    {
        if (LoginState.User != null)
        {
			return LoginState.User.DisplayName +" - ";
        }
        else
        {
            return "";
        }
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
    
}
