using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class MajorCourse
{
    public int MajorCourseId { get; set; }

    public int MajorId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Major Major { get; set; } = null!;
}
