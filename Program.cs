using conveyor;

var builder = Host.CreateApplicationBuilder(args);

// add services
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IJobQueue, SqlJobQueue>(); 

// configure QueueOptions
builder.Services.Configure<QueueOptions>(builder.Configuration.GetSection("Queue"));

var host = builder.Build();

host.Run();
