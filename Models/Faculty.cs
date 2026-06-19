using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Web.Models;

public partial class Faculty
{
    public int FacultyId { get; set; }

    [Display(Name = "ชื่อคณะ")]
    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();
}
