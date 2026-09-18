using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class Models
    {
    }


    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Manager, User

        public DateTime CreatedAt { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ManagerId { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class TeamMember
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int UserId { get; set; }
    }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public int? AssignedToUserId { get; set; }

        public int? TeamId { get; set; }

        public string Status { get; set; } // To Do, In Progress, Done

        public string Priority { get; set; } // Low, Medium, High

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }


}