using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_LoginUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Login1.Visible = LoginState.User == null && LoginState.NewUser == null;
        User1.Visible = !Login1.Visible;
    }
}
