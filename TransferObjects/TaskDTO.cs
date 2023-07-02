using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.TransferObjects
{
    internal class TaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? Due { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }

        public TaskDTO() { }

    }
}
