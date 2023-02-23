using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Models
{
    public class StudentVM
    {
        public StudentVM()
        {
            this.CourseList = new List<int>();
        }
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }
        public int Age { get; set; }

        [Column(TypeName = "bit")]
        public bool IsRegular { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }
        public string PicturePath { get; set; }
        public IFormFile  PictureFile { get; set; }
        public List<int> CourseList { get; set; }
    }
}
