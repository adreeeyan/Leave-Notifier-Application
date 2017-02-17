# **Leave Notifier App**
> If you get confused on what to do while implementing the leave notifier app, just check this repo.  This already have the initial framework for the api, including the database.

###**Setup**
**Prerequisites:**

 1. .NET Core 1.1 SDK (https://www.microsoft.com/net/download/core#/current)
 2. Database
	 3. For windows, SQL Server (2012 above) should be installed
	 4. For linux users, sqlite3 should be installed

**Lets do this!**

 1. Clone repo or download files in zip (then extract)
 2. Open command line
 3. Navigate to the folder cloned or downloaded
 4. Navigate to `src`
 4. Type in `dotnet restore` to download all the dependencies (this will take for a while)
 5. Next is to create the database, navigate to `LeaveNotifierApplication.Data`
 6. Type in `dotnet ef database update` (this will create the database)
 7. Now we need to run the project, navigate to parent folder then navigate to `LeaveNotifierApplication.Api`
 8. Type in `dotnet run` (wait until it says that application has started)
 9. Open browser to test api, visit this site: http://HOSTNAME:5000/swagger (e.g. http://kddppc369:5000)
  
  
**Shortcut (EZ way):**

 1. Clone repo or download files in zip (then extract)
 2. Execute build.cmd (build.sh for linux)
 3. Open browser to test api, visit this site: http://HOSTNAME:5000/swagger (e.g. http://kddppc369:5000)

**THATS IT!**