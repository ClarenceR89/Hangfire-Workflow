namespace Hangfire.Enqueuers
{
    public interface IEnqueuedJob<T>
    {
        string EnqueueJob(T jobParams);
    }
}
