using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using System.Globalization;
using NHibernate;
using System.Web.Security;

public partial class User_User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MTBUser user = GetUser();
        //neahce di nuova creazione: vai alla login!
        if (user == null)
        {
            FormsAuthentication.RedirectToLoginPage();
            return;
        }
        if (LoginState.User == null)
        {
			MyRoutesPanel.Visible = false;
        }
        else
        {
			MyRoutesPanel.Visible = true;
            MyRoutes.Attributes["src"] = "/Routes/RouteContent.aspx?EditMode=true&UserId=" + LoginState.User.Id;
        }
        if (!IsPostBack)
        {
            TextBoxName.Text = user.Name;
            TextBoxSurname.Text = user.Surname;
            TextBoxNickname.Text = user.Nickname;
            TextBoxMail.Text = user.EMail;
            if (user.BirthDate != DateTime.MinValue)
                TextBoxBirthDate.Text = user.BirthDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
            else
                TextBoxBirthDate.Text = "gg/mm/aaaa";
            TextBoxZip.Text = user.Zip;
            RadioButtonListGender.SelectedIndex = user.GenderNumber;
            TextBoxBike1.Text = user.Bike1;
            TextBoxBike2.Text = user.Bike2;
            TextBoxBike3.Text = user.Bike3;
            CheckBoxMailList.Checked = user.SendMail;
        }
    }

    private static MTBUser GetUser()
    {
        MTBUser user = LoginState.User;
        if (user == null) //non ho trovato un utente da db, alora è di nuova creazione?
            user = LoginState.NewUser;
        return user;
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        string message = "Informazioni salvate correttamente.";
        try
        {
            MTBUser user = GetUser();
            user.Name = TextBoxName.Text;
            user.Surname = TextBoxSurname.Text;
            user.Nickname = TextBoxNickname.Text;
            user.EMail = TextBoxMail.Text;
            DateTime d;
            if (ParseDate(TextBoxBirthDate.Text, out d))
                user.BirthDate = d;
            user.Zip = TextBoxZip.Text;
            user.GenderNumber = (short)RadioButtonListGender.SelectedIndex;
            user.Bike1 = TextBoxBike1.Text;
            user.Bike2 = TextBoxBike2.Text;
            user.Bike3 = TextBoxBike3.Text;
            user.SendMail = CheckBoxMailList.Checked;
            DBHelper.SaveUser(user);
            //l'ho salvato: adesso è un utente buono
            if (LoginState.User != user)
            {
                LoginState.User = user;
                //mi mando una mail per conoscenza, e una la mando a chi si è registrato
                string msg = string.Format("Grazie per esserti registrato a MTBScout, questo è il riepilogo dei tuoi dati:\r\n{0}", user);
                Helper.SendMail(user.EMail, null, "info@mtbscout.it", "Conferma avvenuta registrazione", msg);
                
                //non mi serve più quello temporaneo
                LoginState.NewUser = null;
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "MessageOK", "alert('Informazioni salvate correttamente.');", true);
        }
        catch (Exception ex)
        {
            message = String.Format("Errore salvando i dati utente: {0}", ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "MessageError", string.Format("alert('{0}');", message), true);
        }
        
    }
    protected void CustomValidatorBirth_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DateTime dummy;
        args.IsValid = ParseDate(args.Value, out dummy);
    }

    private static bool ParseDate(string dateString, out DateTime date)
    {
        return DateTime.TryParse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
    }
	
}
