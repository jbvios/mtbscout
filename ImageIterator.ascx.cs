using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MTBScout;
using System.Web.Caching;


//================================================================================
public partial class ImageIterator : System.Web.UI.UserControl
{
	string title = "Galleria fotografica";
	private const int maxPages = 10;

	ImageCache cache;
	//--------------------------------------------------------------------------------
	public string Title
	{
		get { return title; }
		set { title = value; }
	}

	string imagesRelPath = "Images";

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		cache = Helper.GetImageCache(Page.MapPath(imagesRelPath));
		PageCounterDown.OnClick += new PageCounter.OnClickDelegate(PageCounter_OnClick);
		PageCounterUp.OnClick += new PageCounter.OnClickDelegate(PageCounter_OnClick);

		int start = 0;
		int.TryParse(Start.Value, out start);
		int currentPage = (int)Math.Ceiling((float)start / (float)ImageCache.maxPerPage);
		PageCounterUp.Pages = PageCounterDown.Pages = cache.pages;
		PageCounterUp.CurrentPage = PageCounterDown.CurrentPage = currentPage;
		PageCounterUp.DrawPages();
		PageCounterDown.DrawPages();

		ImagesTitle.InnerText = Title;
		ImagesTable.Style[HtmlTextWriterStyle.MarginLeft] = "auto";
		ImagesTable.Style[HtmlTextWriterStyle.MarginRight] = "auto";
		ImagesTable.Width = Unit.Percentage(95);
	}
	//--------------------------------------------------------------------------------
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
			DrawTable();
	}

	//--------------------------------------------------------------------------------
	void PageCounter_OnClick(int page)
	{
		Start.Value = (page * ImageCache.maxPerPage).ToString();
		DrawTable();
	}

	//--------------------------------------------------------------------------------
	private void DrawTable()
	{
		int start = 0;
		int.TryParse(Start.Value, out start);
		int currentPage = (int)Math.Ceiling((float)start / (float)ImageCache.maxPerPage);
		PageCounterUp.Pages = PageCounterDown.Pages = cache.pages;
		PageCounterUp.CurrentPage = PageCounterDown.CurrentPage = currentPage;
		PageCounterUp.DrawPages();
		PageCounterDown.DrawPages();

		int prog = 0;
		int end = Math.Min(cache.files.Length, start + ImageCache.maxPerPage);

		Previous.Visible = start != 0;
		Next.Visible = end != cache.files.Length;

		int col = 0;

		TableRow row = null;
		for (prog = start; prog < end; prog++)
		{
			string file = cache.files[prog];

			if (col == 0)
			{
				row = new TableRow();
				ImagesTable.Rows.Add(row);
			}

			TableCell cell = new TableCell();
			row.Cells.Add(cell);
			cell.CssClass = "ImageCell";
			cell.Width = Unit.Percentage(33.33333);
			string caption = cache.captions[prog];

			HyperLink a = new HyperLink();
			Panel p = new Panel();
			System.Drawing.Size sz = cache.sizes[prog];
			p.Width = Unit.Pixel(sz.Width);
			p.Height = Unit.Pixel(sz.Height);
			p.Style[HtmlTextWriterStyle.MarginTop] = "20px";
			p.Style[HtmlTextWriterStyle.MarginBottom] = "10px";
			cell.Controls.Add(p);

			Image downLoadImg = new Image();
			downLoadImg.ImageUrl = "~/Images/download.png";
			downLoadImg.AlternateText = "Clicca per scaricare la versione originale";
			downLoadImg.Attributes["title"] = "Clicca per scaricare la versione originale";
			downLoadImg.Style[HtmlTextWriterStyle.BackgroundColor] = "transparent";
			downLoadImg.Height = Unit.Pixel(30);
			downLoadImg.Width = Unit.Pixel(30);
			downLoadImg.Style[HtmlTextWriterStyle.Position] = "absolute";
			downLoadImg.Style[HtmlTextWriterStyle.Left] = "0px";
			downLoadImg.Style[HtmlTextWriterStyle.Top] = "0px";
			downLoadImg.Style[HtmlTextWriterStyle.Display] = "inline";
			downLoadImg.Style[HtmlTextWriterStyle.Cursor] = "pointer";

			downLoadImg.Attributes["onmouseout"] = "normalImage(this);";
			downLoadImg.Attributes["onmouseover"] = "hoverImage(this);";
			string originalPhotoUrl = PathFunctions.GetFullPath(Request, cache.fileUrls[prog]);
			downLoadImg.Attributes["onclick"] = string.Format("window.open('{0}')", originalPhotoUrl);

			a.NavigateUrl = cache.reducedUrls[prog];
			a.Attributes["rel"] = "lightbox[roadtrip]";
			a.Attributes["title"] = caption;
			p.Controls.Add(a);


			Image img = new Image();
			a.Controls.Add(img);
			p.Controls.Add(downLoadImg);
			img.ImageUrl = cache.thumbUrls[prog];
			img.AlternateText = "Clicca per ingrandire";
			img.Attributes["title"] = "Clicca per ingrandire";
			img.Attributes["imageDescription"] = System.IO.Path.GetFileNameWithoutExtension(file);
			img.Style[HtmlTextWriterStyle.Position] = "absolute";
			img.BorderWidth = Unit.Pixel(1);
			img.Style[HtmlTextWriterStyle.BackgroundColor] = "white";
			img.Style[HtmlTextWriterStyle.Display] = "inline";
			img.Style[HtmlTextWriterStyle.Left] = "0px";
			img.Style[HtmlTextWriterStyle.Top] = "0px";

			Label l = new Label();
			l.Style[HtmlTextWriterStyle.Display] = "block";
			l.Text = caption;

			cell.Controls.Add(l);
			
			if (++col == 3)
				col = 0;
		}
		ImagesPanel.Update();


	}

	//--------------------------------------------------------------------------------
	protected void Next_Click(object sender, EventArgs e)
	{
		int start = 0;
		int.TryParse(Start.Value, out start);
		Start.Value = (start + ImageCache.maxPerPage).ToString();
		DrawTable();

	}
	//--------------------------------------------------------------------------------
	protected void Previous_Click(object sender, EventArgs e)
	{
		int start = 0;
		int.TryParse(Start.Value, out start);
		Start.Value = Math.Max(0, start - ImageCache.maxPerPage).ToString();
		DrawTable();

	}
}
