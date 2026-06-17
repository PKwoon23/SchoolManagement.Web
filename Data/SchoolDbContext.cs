using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Models;


namespace SchoolManagement.Web.Data;

public partial class SchoolDbContext : DbContext
{
    public SchoolDbContext()
    {
    }

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseTeacher> CourseTeachers { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<MajorCourse> MajorCourses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__courses__8F1EF7AEC1F139BA");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .HasColumnName("course_name");
            entity.Property(e => e.Credit).HasColumnName("credit");


            // ชื่ออาจารย์รวมเป็นข้อความ จาก SP GetAllCourses (join course_teachers) — ใช้แสดงในหน้า Index เท่านั้น ไม่มีในตาราง courses
            entity.Property(e => e.TeacherNames).HasColumnName("teacher_names");

        });

        modelBuilder.Entity<CourseTeacher>(entity =>
        {
            entity.HasKey(e => e.CourseTeacherId).HasName("PK__course_t__6C7356DF34819A43");

            entity.ToTable("course_teachers");

            entity.Property(e => e.CourseTeacherId).HasColumnName("course_teacher_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseTeachers)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_course_teachers_courses");

            entity.HasOne(d => d.Teacher).WithMany(p => p.CourseTeachers)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_course_teachers_teachers");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__enrollme__6D24AA7A80BEB9F2");

            entity.ToTable("enrollments");

            entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");
            entity.Property(e => e.AcademicYear).HasColumnName("academic_year");
            entity.Property(e => e.CourseTeacherId).HasColumnName("course_teacher_id");
            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("enrolled_at");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Semester)
                .HasMaxLength(10)
                .HasColumnName("semester");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.CourseTeacher).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseTeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_enrollments_course_teachers");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_enrollments_students");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__facultie__7B00413C290BB321");

            entity.ToTable("faculties");

            entity.HasIndex(e => e.FacultyName, "UQ_faculties_faculty_name").IsUnique();

            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.FacultyName)
                .HasMaxLength(255)
                .HasColumnName("faculty_name");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("PK__majors__DC7AC3C451742D62");

            entity.ToTable("majors");

            entity.Property(e => e.MajorId).HasColumnName("major_id");
            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.MajorName)
                .HasMaxLength(255)
                .HasColumnName("major_name");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Majors)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_majors_faculties");
        });

        modelBuilder.Entity<MajorCourse>(entity =>
        {
            entity.HasKey(e => e.MajorCourseId).HasName("PK__major_co__A339B148F5C52BF5");

            entity.ToTable("major_courses");

            entity.Property(e => e.MajorCourseId).HasColumnName("major_course_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.MajorId).HasColumnName("major_id");

            entity.HasOne(d => d.Course).WithMany(p => p.MajorCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_major_courses_courses");

            entity.HasOne(d => d.Major).WithMany(p => p.MajorCourses)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_major_courses_majors");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__students__2A33069A5DFF2626");

            entity.ToTable("students");

            entity.HasIndex(e => e.StudentCode, "UQ__students__6DF33C4556C87B68").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__students__AB6E6164082721D7").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EntranceYear).HasColumnName("entrance_year");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MajorId).HasColumnName("major_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.StudentCode).HasColumnName("student_code");

            entity.HasOne(d => d.Major).WithMany(p => p.Students)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_students_majors");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__teachers__03AE777ECCA19B01");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.MajorId).HasColumnName("major_id");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(100)
                .HasColumnName("teacher_name");

            entity.HasOne(d => d.Major).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.MajorId)
                .HasConstraintName("fk_teachers_majors");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
