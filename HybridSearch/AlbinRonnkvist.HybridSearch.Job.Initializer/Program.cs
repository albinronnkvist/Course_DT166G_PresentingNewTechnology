using AlbinRonnkvist.HybridSearch.Embedding;
using AlbinRonnkvist.HybridSearch.Job.Initializer;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.ConfigureEmbeddingProject(builder.Configuration);
builder.Services.ConfigureJobsInitializerProject(builder.Configuration, builder.Environment);

builder.Services.AddTransient<IInitializer, ProductsInitializer>();
builder.Services.AddTransient<IInitializerFactory, InitializerFactory>();
builder.Services.AddTransient<MainInitializer>();

var host = builder.Build();

// Run main initializer
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var mainComponent = services.GetRequiredService<MainInitializer>();

    using var cts = new CancellationTokenSource();
    await mainComponent.ExecuteAllInitializersAsync(cts.Token);
}