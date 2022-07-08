using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using onlineBookAndVideoTutorial.Models;

namespace onlineBookAndVideoTutorial.Context
{
    public class tutorialContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}