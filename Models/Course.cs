using System;
using System.Collections.Generic;

namespace SchoolManagement.Web.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    // ชื่ออาจารย์รวมเป็นข้อความ จาก SP GetAllCourses (join course_teachers) — ใช้แสดงในหน้า Index เท่านั้น ไม่มีในตาราง courses
    public string? TeacherNames { get; set; }

    public byte Credit { get; set; }

    public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();

    public virtual ICollection<MajorCourse> MajorCourses { get; set; } = new List<MajorCourse>();
}
