using System;using System.Linq;
namespace StudentsCourse {
 public class GroupModule:IModule {
  public string Title{get{return "Учебные группы";}} public int Order{get{return 1;}}
  public static Group Add(Store s,string code){code=Store.Required(code,40,"Группа");if(s.Data.Groups.Any(g=>g.Code.Equals(code,StringComparison.OrdinalIgnoreCase)))throw new ArgumentException("Группа уже существует");var x=new Group{Id=s.Data.Groups.Count==0?1:s.Data.Groups.Max(g=>g.Id)+1,Code=code};s.Data.Groups.Add(x);return x;}
  public void Execute(Store s){Console.WriteLine("1 — добавить, 2 — список");if(Program.Read("Действие")=="1"){var x=Add(s,Program.Read("Обозначение"));s.Save();Console.WriteLine("Добавлена группа "+x.Id+": "+x.Code);}else foreach(var g in s.Data.Groups)Console.WriteLine(g.Id+" | "+g.Code);}
 }
}