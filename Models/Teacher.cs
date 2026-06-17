using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = null!;

    public int MajorId { get; set; }

    public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();

    public virtual Major? Major { get; set; }

}
