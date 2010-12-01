using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
	protected void Page_Load (object sender, EventArgs e)
	{

	}

	protected void CustomValidator1_ServerValidate (object source, ServerValidateEventArgs args)
	{
		DateTime result;
		args.IsValid = DateTime.TryParse(args.Value, out result);
	}
	protected void CustomValidator2_ServerValidate (object source, ServerValidateEventArgs args)
	{
		args.IsValid = args.Value == pwd1.Text;
	}
	protected void CustomValidator3_ServerValidate (object source, ServerValidateEventArgs args)
	{
		args.IsValid = args.Value == pwd2.Text;
	}
}
