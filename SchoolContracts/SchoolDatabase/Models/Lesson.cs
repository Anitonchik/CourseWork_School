using AutoMapper;
using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Models;
[AutoMap(typeof(LessonDataModel), ReverseMap = true)]
public class Lesson
{
    [Key]
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkerId { get; set; }
    public required string LessonName { get; set; }
    public DateTime LessonDate { get; set; }
    public string Description { get; set; }
    public Worker? Worker { get; set; }

    [ForeignKey("LessonId")]
    public List<LessonInterest>? LessonInterests { get; set; }
    
    [ForeignKey("LessonId")]
    public List<InterestMaterial>? InterestMaterials { get; set; }
}
