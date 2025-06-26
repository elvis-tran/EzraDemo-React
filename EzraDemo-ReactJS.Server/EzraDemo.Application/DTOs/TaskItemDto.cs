using EzraDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EzraDemo.Application.DTOs
{
    public class TaskItemDto
    {
        //Consider possibly doing [JsonPropertyName("id")] for this one instead of normalizing on frontend and on global policy changes....
        public Guid Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public TaskEnums TaskType { get; set; }
    }
}
