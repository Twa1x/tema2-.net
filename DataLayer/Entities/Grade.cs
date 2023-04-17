using DataLayer.Enums;
using System.Data;

namespace DataLayer.Entities;

public class Grade : BaseEntity
{
    public double Value { get; set; }
    public CourseType Course { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
    public DateTime DataCreated { get; set; }
    
}
