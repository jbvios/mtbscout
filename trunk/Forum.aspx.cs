﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;

public partial class Forum : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         if (!Page.IsPostBack)
         {
             DBHelper.DeleteOldAppointments();
         }
        LoadAppointments();
    }

    private void LoadAppointments()
    {
        IList<Appointment> apps = DBHelper.GetAppointments();

        Appointments.DataSource = apps;
        Appointments.DataBind();
    }


    protected void ButtonSend_Click(object sender, EventArgs e)
    {
        RepeaterItem item = (RepeaterItem)((Button)sender).Parent;
        TextBox message = ((TextBox)item.FindControl("Message"));
        TextBox name = ((TextBox)item.FindControl("Name"));

        if (String.IsNullOrEmpty(message.Text) || string.IsNullOrEmpty(name.Text))
            return;
        Appointment p = DBHelper.GetAppointment(int.Parse(((Button)sender).CommandArgument));
        
        Post post = new Post();
        post.Name = name.Text;
        post.PostingDate = DateTime.Now;
        post.Message = message.Text;
        post.Ip = Request["REMOTE_HOST"];
        p.AppointmentPosts.Add(post);

        DBHelper.SaveAppointment(p);
        message.Text = "";
        LoadAppointments();
    }
    protected void Appointments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Appointment app = (Appointment)e.Item.DataItem;
        if (app == null)
            return;
        Repeater inner = (Repeater)e.Item.FindControl("Posts");
        inner.DataSource = app.AppointmentPosts;
        inner.DataBind();
        TextBox txt = (TextBox)e.Item.FindControl("Name");
        Button btn = (Button)e.Item.FindControl("ButtonSend");
        btn.CommandArgument = app.Id.ToString();
        btn.OnClientClick = string.Format("onSendPost('{0}');", txt.ClientID);
        btn = (Button)e.Item.FindControl("ButtonToggle");
        Panel comments = (Panel)e.Item.FindControl("CommentsPanel");
        btn.OnClientClick = string.Format("onToggle(this, '{0}');return false;", comments.ClientID);
    }
}