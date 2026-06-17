using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();
}
