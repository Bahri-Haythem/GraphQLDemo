using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services;

namespace GraphQLDemo.API.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class CourseQuery
    {
        private readonly CourseRepository _courseRepository;

        public CourseQuery(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        //public async Task<IEnumerable<CourseType>> GetCourses()
        //{
        //    return (await _courseRepository.GetAll()).Select(c => new CourseType()
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Subject = c.Subject,
        //        InstructorId = c.InstructorId,
        //    });
        //}

        //[UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        //public async Task<IEnumerable<CourseType>> GetOffsetCourses()
        //{
        //    return (await _courseRepository.GetAll()).Select(c => new CourseType()
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Subject = c.Subject,
        //        InstructorId = c.InstructorId,
        //    });
        //}

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CourseType> GetCourses(SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId
            });
        }

        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            CourseDTO? c = await _courseRepository.GetById(id);
            return c is not null ? new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId
            } : new CourseType();
        }
    }
}
