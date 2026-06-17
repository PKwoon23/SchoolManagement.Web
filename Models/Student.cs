using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int StudentCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int EntranceYear { get; set; }

    public int MajorId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Major Major { get; set; } = null!;
}
