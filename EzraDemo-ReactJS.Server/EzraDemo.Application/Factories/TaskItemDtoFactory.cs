using EzraDemo.Application.DTOs;
using EzraDemo.Domain.Entities;
using EzraDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzraDemo.Application.Factories
{
    public class TaskItemDtoFactory
    {
        //Can use an inheritable class in the future for creating new TaskItemDTOs
        public static TaskItemDto CreateTaskItemDto(TaskItem taskItem)
        {
            return new TaskItemDto
            {
                Id = taskItem.Id,
                TaskName = taskItem.TaskName,
                IsCompleted = taskItem.IsCompleted,
                TaskType = taskItem.TaskType
            };
        }
    }
}
