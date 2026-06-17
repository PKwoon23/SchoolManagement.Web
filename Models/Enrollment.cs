using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }

    public int CourseTeacherId { get; set; }

    public string Semester { get; set; } = null!;

    public int? AcademicYear { get; set; }

    public int? Score { get; set; }

    public int? Grade { get; set; }

    public DateTime EnrolledAt { get; set; }

    public virtual CourseTeacher CourseTeacher { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
