using System;
using System.Collections.Generic;

namespace university.Models
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skills { get; set; }
        public int TotalStudents { get; set; }
        public decimal Salary { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
