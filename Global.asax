<%@ Application Language="C#" %>

<script runat="server">
    static System.Threading.Timer t1, t2;
    void Application_Start(object sender, EventArgs e) 
    {
        Helper.DisableAppDomainRestartOnDelete();
        t1 = new System.Threading.Timer((System.Threading.TimerCallback)delegate { DBHelper.BackupDB(1); }, null, TimeSpan.FromDays(1), TimeSpan.FromDays(1));
        t2 = new System.Threading.Timer((System.Threading.TimerCallback)delegate { DBHelper.BackupDB(2); }, null, TimeSpan.FromDays(7), TimeSpan.FromDays(7));
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
        t1.Dispose();
        t2.Dispose();
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
