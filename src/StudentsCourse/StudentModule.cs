using System;using System.Linq;using System.Collections.Generic;
namespace StudentsCourse {
 public class StudentModule:IModule {
  public string Title{get{return "Студенты";}} public int Order{get{return 2;}}
  public static Student Add(Store s,string name,int group){name=Store.Required(name,120,"ФИО");if(!s.Data.Groups.Any(g=>g.Id==group))throw new ArgumentException("Группа не найдена");var x=new Student{Id=s.Data.Students.Count==0?1:s.Data.Students.Max(t=>t.Id)+1,Name=name,GroupId=group};s.Data.Students.Add(x);return x;}
  public static IEnumerable<Student> Search(Store s,string term){term=term??"";return s.Data.Students.Where(x=>x.Name.IndexOf(term,StringComparison.OrdinalIgnoreCase)>=0);}
  public void Execute(Store s){Console.WriteLine("1 — добавить, 2 — поиск, 3 — список");string action=Program.Read("Действие");if(action=="1"){var x=Add(s,Program.Read("ФИО"),Program.ReadId("ID группы"));s.Save();Console.WriteLine("Добавлен студент "+x.Id+": "+x.Name);}else foreach(var x in Search(s,action=="2"?Program.Read("Поиск"):""))Console.WriteLine(x.Id+" | "+x.Name+" | группа "+x.GroupId);}
 }
}