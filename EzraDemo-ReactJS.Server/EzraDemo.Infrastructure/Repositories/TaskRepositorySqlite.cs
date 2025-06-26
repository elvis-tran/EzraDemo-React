using EzraDemo.Application.Interfaces;
using EzraDemo.Domain.Entities;
using EzraDemo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzraDemo.Infrastructure.Repositories
{
    public class TaskRepositorySqlite : ITaskRepository
    {
        //EF Db Context
        private ApplicationDbContext _tasksDbContext;
        private readonly ILogger<TaskRepositorySqlite> _logger;
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,5);

        private bool disposed = false;

        public TaskRepositorySqlite(ApplicationDbContext tasksDbContext, ILogger<TaskRepositorySqlite> logger)
        {
            _tasksDbContext = tasksDbContext;
            if (_tasksDbContext.TaskItems.Count() == 0)
            {
                LoadTestData();
            }
            _logger = logger;
        }
        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if(taskItem.TaskType == Domain.Enums.TaskEnums.General)
                {
                    _tasksDbContext.TaskItems.Add(taskItem);
                    await _tasksDbContext.SaveChangesAsync();
                    return taskItem;
                }
                //Add future task types

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Failed to create task: {ex.Message}");
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var taskList = await _tasksDbContext.TaskItems.ToListAsync();
                return taskList;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Failed to retrieve tasks: {ex.Message}");
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        /// <summary>
        /// Deletes all tasks that are marked as completed
        /// </summary>
        /// <returns>The number of deleted tasks are returned as a value</returns>
        public async Task<int> DeleteCompletedTasksAsync()
        {
            
            await _semaphoreSlim.WaitAsync();
            try
            {
                var completedTasks = _tasksDbContext.TaskItems.Where(t => t.IsCompleted);
                int count = completedTasks.Count();

                if (count == 0) return 0;

                _tasksDbContext.TaskItems.RemoveRange(completedTasks);
                await _tasksDbContext.SaveChangesAsync();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Failed to all completed tasks: {ex.Message}");
                return 1;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        public async Task<TaskItem> ToggleStatusAsync(Guid id)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var task = await _tasksDbContext.TaskItems.FindAsync(id);
                if (task != null)
                {
                    task.IsCompleted = !task.IsCompleted;
                    await _tasksDbContext.SaveChangesAsync();
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Failed to create toggle task status: {ex.Message}");
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        public async Task<TaskItem> UpdateTaskAsync(Guid id)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var task = await _tasksDbContext.TaskItems.FindAsync(id);
                if (task != null)
                {
                    task.IsCompleted = !task.IsCompleted;
                    await _tasksDbContext.SaveChangesAsync();
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Failed to create toggle task status: {ex.Message}");
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void LoadTestData()
        {

        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _tasksDbContext.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
