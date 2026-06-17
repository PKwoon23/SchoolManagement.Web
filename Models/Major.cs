using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Major
{
    public int MajorId { get; set; }

    public string MajorName { get; set; } = null!;

    public int FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<MajorCourse> MajorCourses { get; set; } = new List<MajorCourse>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
