using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly InstructorRepository _repository;
        public InstructorDataLoader(
            InstructorRepository repository,
            IBatchScheduler batchScheduler) 
            : base(batchScheduler, new DataLoaderOptions()
            {
                MaxBatchSize = 100
            })
        {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var res = await _repository.GetManyByIds(keys);
            return res.ToDictionary(i => i.Id);
        }
    }
}
