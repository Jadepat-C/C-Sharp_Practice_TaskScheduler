using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskScheduler.DataSource;
using TaskScheduler.TransferObjects;

namespace TaskScheduler.BusinessLogic
{
    internal class TaskLogic
    {
        private static int MaxLength = 50;
        private ITaskDao taskDao = null;

        public TaskLogic()
        {
            taskDao = new TaskDaoImpl();
        }

        public List<TaskDTO>? GetAllTasks()
        {
            return taskDao.GetAllTasks();
        }

        public TaskDTO GetTaskById(int id)
        {
            return taskDao.GetTaskById(id);
        }

        public void CreateTask(TaskDTO task)
        {
            taskDao.CreateTask(task);
        }

        public void DeleteTask(TaskDTO task)
        {
            taskDao.DeleteTask(task);
        }

        public void UpdateTask(TaskDTO task)
        {
            taskDao.UpdateTask(task);
        }

        public void ValidateName(string name)
        {
            TrimData(name);
            ValidateString(name, "Task name", MaxLength, false);
        }

        public void ValidateType(string type)
        {
            TrimData(type);
            ValidateString(type, "Task type", MaxLength, false);
        }

        public void ValidateStatus(string? status)
        {
            TrimData(status);
            ValidateString(status, "Status", MaxLength, true);
        }

        public void ValidateNotes(string? notes)
        {
            TrimData(notes);
            ValidateString(notes, "Notes", MaxLength, true);
        }

        public string TrimData(string str)
        {
            str = str.Trim();
            return str;
        }

        public void ValidateString(string? str, string fieldName, int maxLength, bool isNullAllowed)
        {
            if (string.IsNullOrEmpty(str) && isNullAllowed == true)
            {
                return;
            }
            if (string.IsNullOrEmpty(str) && isNullAllowed == false)
            {
                throw new ValidationException($"{fieldName} cannot be empty or whitespace");
            }
            if (str.Length > maxLength && isNullAllowed == false)
            {
                throw new ValidationException($"{fieldName} cannot be longer then {maxLength} characters");
            }
        }

        public void ValidateDateTime(string? str, bool isNullAllowed)
        {
            if (str == null && isNullAllowed == true)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(str) && isNullAllowed == false)
            {
                throw new ValidationException("Date cannot be null or empty");
            }

            if (!DateTime.TryParse(str, out DateTime parsedDateTime) && isNullAllowed == false)
            {
                throw new ValidationException("Invalid date format");
            }

            if ((parsedDateTime < SqlDateTime.MinValue.Value || parsedDateTime > SqlDateTime.MaxValue.Value) && isNullAllowed == false)
            {
                throw new ValidationException("Date must be between " + SqlDateTime.MinValue.Value + " and " + SqlDateTime.MaxValue.Value);
            }
        }



    }
}
