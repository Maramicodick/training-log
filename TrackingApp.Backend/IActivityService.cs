public interface IActivityService
{
    Task<List<Activity>> GetAllActivitiesAsync();
    Task<Activity> GetActivityAsync(Guid id);
    Task<Activity> CreateAsync(CreateActivityRequest request);
    Task UpdateAsync(Guid id, UpdateActivityRequest request);
    Task DeleteAsync(Guid id);
}
