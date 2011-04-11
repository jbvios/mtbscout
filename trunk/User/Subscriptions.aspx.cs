using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using System.Globalization;

public partial class User_Subscriptions : System.Web.UI.Page
{
	EventSubscriptor[] subscriptors;
	public const int SuperEnduroId = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!LoginState.TestLogin())
			return;

		LoadSubscriptors();
    }

	private void LoadSubscriptors()
	{
		subscriptors = DBHelper.GetSubscriptors(LoginState.User.Id, SuperEnduroId);

		GridViewSubscriptions.DataSource = subscriptors;
		GridViewSubscriptions.DataBind();
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
	protected void ButtonSave_Click(object sender, EventArgs e)
	{
		EventSubscriptor sbscr = new EventSubscriptor();
		DateTime dt;
		if (!ParseDate(TextBoxBirthDate.Text, out dt))
			return;

		sbscr.BirthDate = dt;
		int dummy;
		int.TryParse(SubscriptionId.Value, out dummy); 
		sbscr.Id = dummy;
		sbscr.EventId = SuperEnduroId;
		sbscr.UserId = LoginState.User.Id;
		sbscr.Name = TextBoxName.Text;
		sbscr.Surname = TextBoxSurname.Text;
		sbscr.EMail = TextBoxMail.Text;
            
		sbscr.GenderNumber = (short)RadioButtonListGender.SelectedIndex;
		
		DBHelper.SaveSubscriptor(sbscr);
		LoadSubscriptors();

		if (subscriptors.Length > 0)
			SubscriptionId.Value = subscriptors[subscriptors.Length - 1].Id.ToString();
		else
			SubscriptionId.Value = "";
	}
	protected void GridViewSubscriptions_RowEditing(object sender, GridViewEditEventArgs e)
	{

	}
	protected void GridViewSubscriptions_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		if (e.RowIndex == -1 || e.RowIndex > subscriptors.Length - 1)
			return;
		EventSubscriptor sbscr = subscriptors[e.RowIndex];
		DBHelper.DeleteSubscriptor(sbscr);
		
		LoadSubscriptors();
	}
}
