using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exercise2.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;

        public string Photo { get; set; } 
    }
}
