using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.TransferObjects;

namespace TaskScheduler.DataSource
{
    internal interface ITaskDao
    {
        List<TaskDTO>? GetAllTasks();
        TaskDTO GetTaskById(int id);
        void CreateTask(TaskDTO task);
        void UpdateTask(TaskDTO task);
        void DeleteTask(TaskDTO task);

    }
}
