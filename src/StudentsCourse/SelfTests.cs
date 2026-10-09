using System;using System.IO;using System.Linq;using System.Text;using System.Collections.Generic;using System.Web.Script.Serialization;
namespace StudentsCourse {
 public static class SelfTests {
  static List<string> checks=new List<string>();
  static void Check(string title,bool good){if(!good)throw new Exception("FAIL: "+title);checks.Add(title);Console.WriteLine("PASS | "+title);}
  static bool Reject(Action action){try{action();return false;}catch(ArgumentException){return true;}}
  public static int Run(string dir){Directory.CreateDirectory(dir);string path=Path.Combine(dir,"test-"+Guid.NewGuid().ToString("N")+".json");var s=new Store(path);
   Check("Empty store",s.Data.Students.Count==0);var g=GroupModule.Add(s," ПИ-123 ");Check("Group trim",g.Code=="ПИ-123");Check("Duplicate group",Reject(()=>GroupModule.Add(s,"пи-123")));Check("Blank group",Reject(()=>GroupModule.Add(s," ")));Check("Long group",Reject(()=>GroupModule.Add(s,new String('x',41))));
   var a=StudentModule.Add(s,"Тестовый студент",g.Id);Check("Student reference",a.GroupId==g.Id);Check("Unknown group",Reject(()=>StudentModule.Add(s,"Другой",999)));Check("Blank name",Reject(()=>StudentModule.Add(s,"",g.Id)));Check("Search case insensitive",StudentModule.Search(s,"ТЕСТОВЫЙ").Count()==1);Check("Search no match",StudentModule.Search(s,"НетТакого").Count()==0);
   var second=StudentModule.Add(s,"Без оценок",g.Id);Check("Increasing IDs",second.Id==a.Id+1);Check("Empty average",!GradeModule.Average(s,second.Id).HasValue);
   GradeModule.Add(s,a.Id,"Математика",2);GradeModule.Add(s,a.Id,"Информатика",5);Check("Average",Math.Abs(GradeModule.Average(s,a.Id).Value-3.5)<.000001);Check("Grade below limit",Reject(()=>GradeModule.Add(s,a.Id,"Тест",1)));Check("Grade above limit",Reject(()=>GradeModule.Add(s,a.Id,"Тест",6)));Check("Unknown student",Reject(()=>GradeModule.Add(s,999,"Тест",4)));Check("Empty subject",Reject(()=>GradeModule.Add(s,a.Id,"",4)));
   s.Save();var restored=new Store(path);Check("Reload student count",restored.Data.Students.Count==2);Check("Reload grade count",restored.Data.Grades.Count==2);Check("Cyrillic persistence",restored.Data.Students[0].Name=="Тестовый студент");s.Save();Check("Backup created",File.Exists(path+".bak"));Check("Three modules",Program.Modules().Length==3);Check("Menu up wraps",Program.Move(0,-1,4)==3);Check("Menu down wraps",Program.Move(3,1,4)==0);
   string bad=Path.Combine(dir,"invalid.json");File.WriteAllText(bad,"{\"Groups\":[],\"Students\":[{\"Id\":1,\"Name\":\"Test\",\"GroupId\":99}],\"Grades\":[]}");bool invalid=false;try{new Store(bad);}catch(InvalidDataException){invalid=true;}Check("Invalid references rejected",invalid);
   File.WriteAllText(Path.Combine(dir,"results.json"),new JavaScriptSerializer().Serialize(new{Passed=checks.Count,Failed=0,Checks=checks}),Encoding.UTF8);Console.WriteLine("TOTAL: "+checks.Count+" passed, 0 failed");return 0;
  }
  public static void Demo(string path){
   if(File.Exists(path))throw new ArgumentException("Для демо нужен новый файл");var s=new Store(path);var g=GroupModule.Add(s,"ПИ-123");var st=StudentModule.Add(s,"Учебный Студент",g.Id);GradeModule.Add(s,st.Id,"Математика",4);GradeModule.Add(s,st.Id,"Информатика",5);s.Save();Console.WriteLine("АС Студенты — демонстрация трёх функций");Console.WriteLine("Группа: "+g.Code);Console.WriteLine("Студент: "+st.Name);Console.WriteLine("Оценки: 4, 5");Console.WriteLine("Средний балл: "+GradeModule.Average(s,st.Id).Value.ToString("F2"));var reload=new Store(path);Console.WriteLine("После повторного открытия: "+reload.Data.Students.Count+" студент, "+reload.Data.Grades.Count+" оценки");Console.WriteLine("Файл: "+Path.GetFileName(path));
  }
 }
}