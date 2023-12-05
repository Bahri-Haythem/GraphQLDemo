using GraphQLDemo.Client;
using Microsoft.Extensions.DependencyInjection;
using StrawberryShake;

await Task.Delay(3000);


var serviceCollection = new ServiceCollection();

serviceCollection
    .AddGraphQLDemoClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:7268/graphql"));

IServiceProvider services = serviceCollection.BuildServiceProvider();

IGraphQLDemoClient client = services.GetRequiredService<IGraphQLDemoClient>();

var res = await client.Test.ExecuteAsync();
var res2 = await client.GetCourses.ExecuteAsync();
var res3 = await client.GetCourseById.ExecuteAsync(new Guid("98bd48ec-740b-4310-8df2-d565ce4d8628"));


if (res.IsErrorResult())
{
    Console.WriteLine("there is an error here");
}
else
{
    /*Console.WriteLine(res.Data?.Test);
    res2.Data?.Courses?.Edges?.ToList().ForEach(e =>
    {
        Console.WriteLine($"Name : {e.Node.Name},Id : {e.Node.Id},CreatorId : {e.Node.Creator?.Id}");
    });*/
    var Course = res3.Data?.CourseById;
    Console.WriteLine($"Name : {Course?.Name},Id : {Course?.Id},CreatorId : {Course?.Creator?.Id}");
}

Console.ReadLine();