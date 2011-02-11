using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LiteralControl c = new LiteralControl("<META NAME=\"Description\" CONTENT=\"mountain bike MTB montoggio scout bicicletta bike biker bikers pentemina scrivia\">");
        Page.Header.Controls.Add(c);


        if (LoginState.User != null)
        {
            User.Visible = true;
            DisconnectButton.ToolTip = string.Format("Disconnetti l'utente {0}", LoginState.User.DisplayName);
            Disconnect.Alt = DisconnectButton.ToolTip;
        }
        else
        {
            User.Visible = false;
        }
    }
    protected void Disconnect_Click(object sender, EventArgs e)
    {
        LoginState.User = null;
        User.Visible = false;
    }

}
