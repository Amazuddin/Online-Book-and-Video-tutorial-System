using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineBookAndVideoTutorial.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public string Pdf { get; set; }
    }
}