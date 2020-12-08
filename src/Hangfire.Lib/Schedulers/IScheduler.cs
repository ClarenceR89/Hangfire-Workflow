using System.Threading.Tasks;

namespace Hangfire.Schedulers
{
    /// <summary>
    /// <see cref="IScheduler"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduler<T>
    {
        /// <summary>
        /// <see cref="AddScheduledJob"/>
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<string> AddScheduledJob(T job);

        /// <summary>
        /// <see cref="UpdateScheduledJob"/>
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<bool> UpdateScheduledJob(T job);

        /// <summary>
        /// <see cref="DeleteScheduledJob"/>
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<bool> DeleteScheduledJob(string jobId);

        /// <summary>
        /// <see cref="InvokeScheduledJob"/>
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<bool> InvokeScheduledJob(string jobId);

        /// <summary>
        /// <see cref="ReSyncSchedules"/>
        /// </summary>
        /// <returns></returns>
        Task ReSyncSchedules();
    }
}
