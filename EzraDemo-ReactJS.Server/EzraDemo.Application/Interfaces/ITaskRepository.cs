using EzraDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzraDemo.Application.Interfaces
{
    public interface ITaskRepository : IDisposable
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> ToggleStatusAsync(Guid id);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task<int> DeleteCompletedTasksAsync();
    }
}
