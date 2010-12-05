using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;

public partial class User_User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LoginState.TestLogin() && !IsPostBack)
        {
            MTBUser user = LoginState.User;
            TextBoxName.Text = user.Name;
            TextBoxSurname.Text = user.Surname;
            TextBoxMail.Text = user.EMail;
        }
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        MTBUser user = LoginState.User;
        user.Name = TextBoxName.Text;
        user.Surname = TextBoxSurname.Text;
        user.EMail = TextBoxMail.Text;
        LoginState.SaveUser(user);
    }
}
