using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class CourseTeacher
{
    public int CourseTeacherId { get; set; }

    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Teacher Teacher { get; set; } = null!;
}
