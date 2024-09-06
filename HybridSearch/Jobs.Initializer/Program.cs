using AlbinRonnkvist.HybridSearch.Jobs.Initializer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProductsInitializer>();
builder.Services.ConfigureElasticsearch(builder.Configuration, builder.Environment);
builder.Services.ConfigureCustomServices();
builder.Services.ConfigureIndices(builder.Configuration);

var host = builder.Build();
host.Run();
