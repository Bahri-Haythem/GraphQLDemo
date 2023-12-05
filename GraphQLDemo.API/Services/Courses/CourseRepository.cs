using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Courses
{
    public class CourseRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _contextFactory;
        public CourseRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<CourseDTO>> GetAll()
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Courses
                    .ToListAsync();
            }
        }

        public async Task<CourseDTO?> GetById(Guid courseId)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Courses
                    .FirstOrDefaultAsync(c => c.Id == courseId);
            }
        }

        public async Task<CourseDTO> Create(CourseDTO course)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<CourseDTO> Update(CourseDTO course)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                CourseDTO course = new CourseDTO()
                {
                    Id = id
                };
                context.Courses.Remove(course);

                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
