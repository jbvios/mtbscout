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
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
		DBHelper.BackupDB(dbPath);

		Helper.DecreaseSessions();
    }
       
</script>
