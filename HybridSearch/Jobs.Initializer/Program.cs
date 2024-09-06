using AlbinRonnkvist.HybridSearch.Jobs.Initializer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProductsInitializer>();
builder.Services.ConfigureElasticsearch(builder.Configuration, builder.Environment);

var host = builder.Build();
host.Run();
