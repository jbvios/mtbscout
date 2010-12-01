<%@ Application Language="C#" %>

<script runat="server">
	static string dbPath;
	
    void Application_Start(object sender, EventArgs e) 
    {
        this.EndRequest += new EventHandler(global_asax_EndRequest);
    }

    void global_asax_EndRequest(object sender, EventArgs e)
    {
        
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
		if (dbPath == null)
			dbPath = DBHelper.GetDBPath(Request);
		
		DBHelper.CountVisitor(Request, Session, dbPath);

		Helper.IncreaseSessions();
    }	

    void Session_End(object sender, EventArgs e) 
    {
		Helper.DecreaseSessions();
    }
       
</script>
