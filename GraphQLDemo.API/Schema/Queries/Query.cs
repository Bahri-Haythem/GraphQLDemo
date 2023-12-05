using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        public async Task<IEnumerable<ISearchResultType>> Search(string term, SchoolDbContext context)
        {
            var courses = await context.Courses
                .Where(c => c.Name.Contains(term))
                .Select(c => new CourseType
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId,
                    CreatorId = c.CreatorId
                })
                .ToListAsync();

            var instructors = await context.Instructors
                .Where(i => i.FirstName.Contains(term) || i.LastName.Contains(term))
                .Select(c => new InstructorType
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Salary = c.Salary
                })
                .ToListAsync();

            return new List<ISearchResultType>().Concat(courses).Concat(instructors);
        }
        //[GraphQLDeprecated("this is deprecated")]
        public string Test => "this is a test 2";
    }
}
