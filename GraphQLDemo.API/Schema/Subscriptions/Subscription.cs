using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.API.Schema.Subscriptions
{
    public class Subscription
    {
        /*[Subscribe]
        public CourseType CourseCreated([EventMessage] CourseType course) => course;

        public ValueTask<ISourceStream<CourseType>> SubscribeToCourseUpdated(
            Guid Id,
            ITopicEventReceiver topicEventReceiver)
        {
            return topicEventReceiver.SubscribeAsync<CourseType>($"{Id}_{nameof(CourseUpdated)}");
        }

        //courseUpdated(id: "1db5d8b0-fbb9-4270-ae93-2ea827ade422")
        //{
        //    name
        //    subject
        //}

        [Subscribe(With = nameof(SubscribeToCourseUpdated))]
        //id needs an attribute
        public CourseType CourseUpdated(Guid Id, [EventMessage] CourseType courseResult)
            => courseResult;
        */
    }
}
