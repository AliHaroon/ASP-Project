﻿using ASP_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.ViewModels
{
    public class StudentCourseViewModel
    {
		public IEnumerable<string> Enrollment { get; set; }
		public string CourseID { get; set; }

    }
}
