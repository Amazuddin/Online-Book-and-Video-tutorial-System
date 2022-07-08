using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineBookAndVideoTutorial.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string VideoName { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public string Link { get; set; }
    }
}