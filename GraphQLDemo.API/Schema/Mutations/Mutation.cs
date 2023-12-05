using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Middlewares.UseUser;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Validators;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly CourseRepository _courseRepository;
        private readonly CourseTypeInputValidator _rules;

        public Mutation(CourseRepository courseRepository
            , CourseTypeInputValidator rules)
        {
            _courseRepository = courseRepository;
            _rules = rules;
        }

        //[Authorize]
        [UseUser]
        public async Task<CourseResult> CreateCourse(
            CourseInput courseInput,
            [Service] ITopicEventSender topicEventSender,
            [GlobalState("User")] User user)
        {
            var validationResult = _rules.Validate(courseInput);
            if(!validationResult.IsValid)
            {
                throw new GraphQLException("Validation error");
            }

            var courseDto = new CourseDTO()
            {
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId,
                CreatorId = user.Id ?? ""
            };

            courseDto = await _courseRepository.Create(courseDto);

            CourseResult c = new CourseResult
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Subject = courseDto.Subject,
                InstructorId = courseDto.InstructorId
            };

            //await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), c);
            return c;
        }

        [Authorize]
        public async Task<CourseResult> UpdateCourseAsync(Guid id, string name, Subject subject, Guid instructorId
            , [Service] ITopicEventSender topicEventSender)
        {
            var courseDto = new CourseDTO()
            {
                Id = id,
                Name = name,
                Subject = subject,
                InstructorId = instructorId
            };

            courseDto = await _courseRepository.Update(courseDto);

            CourseResult c = new CourseResult
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Subject = courseDto.Subject,
                InstructorId = courseDto.InstructorId
            };

            var updateCourseTopic = $"{c.Id}";
            await topicEventSender.SendAsync(updateCourseTopic, c);

            return c;
        }
        //you can inject ClaimsPrinciple here
        [Authorize(Policy = "IsAdmin")]
        public async Task<bool> DeleteCourse(Guid id)
        {
            try
            {
                return await _courseRepository.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
