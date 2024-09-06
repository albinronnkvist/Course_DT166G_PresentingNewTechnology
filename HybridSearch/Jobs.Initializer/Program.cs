using AlbinRonnkvist.HybridSearch.Embedding;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProductsInitializer>();
builder.Services.ConfigureJobsInitializerProject(builder.Configuration, builder.Environment);
builder.Services.ConfigureEmbeddingProject(builder.Configuration);

var host = builder.Build();
host.Run();
