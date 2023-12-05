using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation.AspNetCore;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using GraphQLDemo.API.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddTransient<CourseTypeInputValidator>();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    //.AddSubscriptionType<Subscription>()
    .AddType<CourseType>()
    .AddType<InstructorType>()
    .AddTypeExtension<CourseQuery>()
    .AddInMemorySubscriptions()
    .RegisterDbContext<SchoolDbContext>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(o => o.AddPolicy("IsAdmin", p => p.RequireClaim(
    FirebaseUserClaimType.EMAIL, "haythem.bahri8@hotmail.com")));

var cs = builder.Configuration.GetConnectionString("default");

builder.Services
    .AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(cs));

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<UserDataLoader>();
builder.Services.AddDbContext<SchoolDbContext>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    var rs = scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<SchoolDbContext>>();
    using (var context = rs.CreateDbContext())
    {
        context.Database.Migrate();
    }
}

app.UseAuthentication();
app.UseWebSockets();
app.MapGraphQL();

app.Run();
