using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Student
    {
        public Student()
        {
            this.StudentEntries = new List<StudentEntry>();
        }
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }
        public int Age { get; set; }

        [Column(TypeName = "bit")]
        public bool IsRegular { get; set; }

        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }
        public string PicturePath { get; set; }
        public virtual ICollection<StudentEntry> StudentEntries { get; set; }
    }
    public class Course
    {
        public Course()
        {
            this.StudentEntries = new List<StudentEntry>();
        }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual ICollection<StudentEntry> StudentEntries { get; set; }

    }
    public class StudentEntry
    {
        public int StudentEntryId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
    //public class StudentDbContext:DbContext
    //{
    //    public StudentDbContext(DbContextOptions<StudentDbContext>options):base(options)
    //    {

    //    }
    //    public DbSet<Student> Students { get; set; }
    //    public DbSet<Course> Courses { get; set; }
    //    public DbSet<StudentEntry> StudentEntries { get; set; }
    //}
}
