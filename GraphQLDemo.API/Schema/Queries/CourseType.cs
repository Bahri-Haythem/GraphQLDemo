using FirebaseAdmin.Auth;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.Schema.Queries
{
    public class CourseType : ISearchResultType
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Subject Subject { get; set; }
        [IsProjected(true)]
        public Guid InstructorId { get; set; }
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            InstructorDTO instructorDTO = await instructorDataLoader.LoadAsync(InstructorId);
            
            return new InstructorType()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };
        }
        public IEnumerable<StudentType>? Students { get; set; }
        [IsProjected(true)]
        public string? CreatorId { get; set; }
        public async Task<UserType?> Creator([Service] UserDataLoader userDataLoader)
        {
            if (string.IsNullOrWhiteSpace(CreatorId)) return null;

            return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);
        }
    }
}
