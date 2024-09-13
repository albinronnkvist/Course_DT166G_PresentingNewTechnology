using AlbinRonnkvist.HybridSearch.Api;
using AlbinRonnkvist.HybridSearch.Embedding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureApiProject(builder.Configuration, builder.Environment);
builder.Services.ConfigureEmbeddingProject(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
