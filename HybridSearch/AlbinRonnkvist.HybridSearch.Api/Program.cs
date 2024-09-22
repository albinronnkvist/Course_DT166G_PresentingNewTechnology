using AlbinRonnkvist.HybridSearch.Api;
using AlbinRonnkvist.HybridSearch.Embedding;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureEmbeddingProject(builder.Configuration);
builder.Services.ConfigureApiProject(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    app.UseHttpsRedirection();    
}

app.MapControllers();
app.UseCors(AlbinRonnkvist.HybridSearch.Api.ServiceCollectionExtensions.CorsPolicy);

app.Run();
