using DataSeeder;
using DataSeeder.DataSets;
using DataSeeder.DataSets.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyOrigin();
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = new AppSettings();
builder.Configuration.Bind(settings);

var mongoContext = new MongoContext(settings.MongoDb);
builder.Services.AddSingleton<MongoContext>(mongoContext);

var emptySet = new EmptySet();
var defaultUserSet = new UserOnlyDataSet();
var userWithCreatedThreadSet = new UserWithCreatedThreadSet();
var userWithCreatedThreadWithCreatedComment = new UserWithCreatedThreadWithCreatedComment();
var dataSets = new Dictionary<string, IDataSet>
{
    {emptySet.Id, emptySet},
    {defaultUserSet.Id, defaultUserSet},
    {userWithCreatedThreadSet.Id, userWithCreatedThreadSet},
    {userWithCreatedThreadWithCreatedComment.Id, userWithCreatedThreadWithCreatedComment}
};
builder.Services.AddSingleton(dataSets);

var dataClearer = new DataClearer(mongoContext);
builder.Services.AddSingleton(dataClearer);

var app = builder.Build();

app.UseCors(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();