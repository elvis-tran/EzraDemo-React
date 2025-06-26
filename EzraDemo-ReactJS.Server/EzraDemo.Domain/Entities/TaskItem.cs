using EzraDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzraDemo.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public TaskEnums TaskType { get; set; }
    }
}
