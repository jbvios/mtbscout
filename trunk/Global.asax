<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        Helper.DisableAppDomainRestartOnDelete();
    }


    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // Filter the text to be rendered as all uppercase.
        //Response.Filter = new WebLocalizer.UpperCaseFilterStream(Response.Filter);

    }
    
	void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		
	}

	void Application_End(object sender, EventArgs e)
	{
	}
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
		DBHelper.CountVisitor(Request, Session);

		Helper.IncreaseSessions();
    }	

    void Session_End(object sender, EventArgs e) 
    {
		Helper.DecreaseSessions();
    }
       
</script>
