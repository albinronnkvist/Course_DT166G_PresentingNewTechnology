using AlbinRonnkvist.HybridSearch.Jobs.Initializer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProductsInitializer>();
builder.Services.ConfigureJobsInitializerProject(builder.Configuration, builder.Environment);

var host = builder.Build();
host.Run();
