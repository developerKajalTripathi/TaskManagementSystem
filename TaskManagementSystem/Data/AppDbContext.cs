using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(): base("TaskManagementDb")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }





}