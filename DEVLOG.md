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

### 11-9-2026
Lessons Learned
* Atomic SQL operations do not need a transaction and the exception should bubble up to the worker
* `dotnet user-secrets set` - sets a user secret that is automatically merged with your appsettings configuration
* IOptions can either be used or first tied to a model class (.Value syntax vs using model class directly)

### 11-10-2026
* In real world scenarios, multiple workers run at once to process jobs. 
* This was simulated by running two instances of the worker on 20 seeded jobs, which correctly resulted in two processes claiming jobs simultaneously. 

### 11-12-2026
* You need to use command definition with dapper to pass in a cancellation token
* Job type and payload will determine what gets done with that job in the future 
* There will be a job handler in the future to handle those jobs
