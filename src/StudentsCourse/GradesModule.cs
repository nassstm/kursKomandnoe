using System;using System.Linq;
namespace StudentsCourse {
 public class GradeModule:IModule {
  public string Title{get{return "Успеваемость";}} public int Order{get{return 3;}}
  public static Grade Add(Store s,int student,string subject,int value){subject=Store.Required(subject,100,"Дисциплина");if(!s.Data.Students.Any(st=>st.Id==student))throw new ArgumentException("Студент не найден");if(value<2||value>5)throw new ArgumentException("Оценка должна быть от 2 до 5");var x=new Grade{Id=s.Data.Grades.Count==0?1:s.Data.Grades.Max(t=>t.Id)+1,StudentId=student,Subject=subject,Value=value};s.Data.Grades.Add(x);return x;}
  public static double? Average(Store s,int student){var values=s.Data.Grades.Where(x=>x.StudentId==student).Select(x=>x.Value).ToArray();return values.Length==0?(double?)null:values.Average();}
  public void Execute(Store s){Console.WriteLine("1 — ввод оценки, 2 — оценки и средний балл");string action=Program.Read("Действие");int student=Program.ReadId("ID студента");if(action=="1"){var x=Add(s,student,Program.Read("Дисциплина"),Program.ReadId("Оценка"));s.Save();Console.WriteLine("Сохранена оценка "+x.Value);}else{if(!s.Data.Students.Any(st=>st.Id==student))throw new ArgumentException("Студент не найден");foreach(var x in s.Data.Grades.Where(x=>x.StudentId==student))Console.WriteLine(x.Subject+" | "+x.Value);var avg=Average(s,student);Console.WriteLine(avg.HasValue?"Средний балл: "+avg.Value.ToString("F2"):"Оценок нет");}}
 }
}