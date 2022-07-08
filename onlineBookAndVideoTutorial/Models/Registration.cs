using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineBookAndVideoTutorial.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public string RegistrationNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Mobile { get; set; }
    }
}