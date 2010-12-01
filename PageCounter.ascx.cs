using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class PageCounter : System.Web.UI.UserControl
{
	public int Pages { get; set; }
	public int CurrentPage { get; set; }
	public delegate void OnClickDelegate (int page);
	public event OnClickDelegate OnClick;

	protected void Page_Load(object sender, EventArgs e)
    {
    }

	public void DrawPages ()
	{
		PagesRow.Controls.Clear();
		
		int start = 0;
		if (Pages <= 1)
			return;

		while (start < Pages)
		{
			TableCell cell = new TableCell();
			LinkButton link = new LinkButton();
			link.ID = "Link" + start.ToString();
			link.Text = (start + 1).ToString();
			link.Click += new EventHandler(link_Click);

			if (start == CurrentPage)
			{
				link.BorderStyle = BorderStyle.Solid;
				link.BorderColor = Color.Red;
				link.BorderWidth = Unit.Pixel(1);
			}
			cell.Controls.Add(link);

			PagesRow.Controls.Add(cell);
			start++;
		}
		
	}

	void link_Click (object sender, EventArgs e)
	{
		int page = 0;
		int.TryParse(((LinkButton)sender).Text, out page);
		if (OnClick != null)
			OnClick(page - 1);
	}

}
