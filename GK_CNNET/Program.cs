using System.Collections;
using GK_CNNET.Configurations;
using GK_CNNET.GraphQL;
using GK_CNNET.Models;
using GK_CNNET.Services;
using MongoDB.Driver;
using Grpc.AspNetCore.Web;
var builder = WebApplication.CreateBuilder(args);

//REST
builder.Services.AddControllers();

//GraphQL
builder.Services.AddGraphQLServer().AddQueryType<Query>().AddMutationType<Mutation>();

//gRPC
builder.Services.AddGrpc();
        

// Ánh xạ cấu hình
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddSingleton<IMongoClient>(sp => {
    var connectionString = builder.Configuration.GetSection("ConnectionStrings")["DBConnection"];
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(sp => {
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("BookDB");
    return database.GetCollection<Book>("Books");
});
// Đăng ký Collection Gamess - MongoDB
builder.Services.AddScoped(sp => {
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("store_game"); 
    return database.GetCollection<Game>("games");    // Bảng gáme 
});

// Đăng ký GameService
builder.Services.AddScoped<IGameService, GameService>();
///// 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyMethod()   
              .AllowAnyHeader()  
              .WithExposedHeaders("grpc-status", "grpc-message", "grpc-encoding", "grpc-accept-encoding");
    });
});

var app = builder.Build();

// Map các Endpoint
app.UseRouting();
app.UseCors("AllowAll");
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapControllers(); 
app.MapGraphQL();    
app.MapGrpcService<BookGrpcService>().EnableGrpcWeb();

app.Run();