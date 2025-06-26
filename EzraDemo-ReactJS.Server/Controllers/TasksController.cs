using EzraDemo.Application.DTOs;
using EzraDemo.Application.Factories;
using EzraDemo.Application.Interfaces;
using EzraDemo.Domain.Entities;
using EzraDemo.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzraDemo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;
        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves all tasks
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            //Currently the only task type is GeneralTask otherwise additional logic would be required to create all of the necessary DTO objects
            if(tasks.Count() == 0)
            {
                //It's fine to be empty
                var result = new List<TaskItem>();
                return Ok(result);
            }
            return Ok(tasks.Select(t => TaskItemDtoFactory.CreateTaskItemDto(t)));
        }
        /// <summary>
        /// The JSON that is sent from the frontend will contain only the task name which is all that is needed to create the new task
        /// </summary>
        /// <param name="newTask"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto newTask)
        {
            if (newTask == null || string.IsNullOrEmpty(newTask.TaskName))
            {
                _logger.LogDebug("Invalid Task or Task Name");
                return BadRequest(new { message = "Task Name is required" });
            }

            var taskItem = new TaskItem { Id = Guid.NewGuid(), TaskName = newTask.TaskName, TaskType = TaskEnums.General, IsCompleted = false };
            var createdTask = await _taskRepository.CreateTaskAsync(taskItem);

            if (createdTask == null)
            {
                return StatusCode(500, new { message = "Failed to create task due to internal error." });
            }
            return Ok(TaskItemDtoFactory.CreateTaskItemDto(createdTask));
        }
        /// <summary>
        /// Updates a task that matches the Guid
        /// </summary>
        /// <param name="id"> Is a Guid that should correspond to a task in the database</param>
        /// <returns>The task that was updated if successful or a not found result</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskItem>> ToggleTask(Guid id)
        {
            var updated = await _taskRepository.ToggleStatusAsync(id);
            if (updated != null)
            {
                return Ok(TaskItemDtoFactory.CreateTaskItemDto(updated));
            }
            else
            {
                _logger.LogDebug("Task not found: {id}", id);
                return NotFound();
            }
        }

        [HttpDelete("completed")]
        public async Task<IActionResult> DeleteCompletedTasks()
        {
            var deletedCount = await _taskRepository.DeleteCompletedTasksAsync();
            if (deletedCount > 0)
            {
                return Ok(new { deleted = deletedCount });
            }
            else
            {
                return NotFound("No completed tasks found to delete.");
            }
        }
    }
}
