### 11-4-2026
Lessons learned
* ROWLOCK - locks rows at row level rather than page level 
* READPAST - allows locked rows to be skipped 
* UPDLOCK - gives rows an update lock so other processes cannot update them 
* Mismatching index priority and ORDER BY priority can result in too many rows being read and given an update lock. 

### 11-5-2026 
Lessons learned 
* Generic host is an app's runtime shell that includes DI container, logging, config, and hosted services. 
* BackgroundService is an abstract class that implements IHostedService 
* .NET Worker template - adds a worker as a hosted service. 
* IOptions can bind to any c# object 
