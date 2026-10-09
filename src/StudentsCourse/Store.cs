using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
namespace StudentsCourse {
 public class Store {
  public Database Data {get;private set;} public string FilePath {get;private set;}
  public Store(string path) {
   FilePath=Path.GetFullPath(path);
   Data=File.Exists(FilePath)?new JavaScriptSerializer().Deserialize<Database>(File.ReadAllText(FilePath,Encoding.UTF8)):new Database();
   Validate();
  }
  public void Validate(){
   if(Data==null||Data.Groups==null||Data.Students==null||Data.Grades==null)throw new InvalidDataException("Неверная структура данных");
   if(Data.Groups.Any(x=>x==null||x.Id<=0||String.IsNullOrWhiteSpace(x.Code))||Data.Groups.Select(x=>x.Id).Distinct().Count()!=Data.Groups.Count)throw new InvalidDataException("Неверные группы");
   if(Data.Groups.Select(x=>x.Code.Trim().ToUpperInvariant()).Distinct().Count()!=Data.Groups.Count)throw new InvalidDataException("Повтор обозначения группы");
   if(Data.Students.Any(x=>x==null||x.Id<=0||String.IsNullOrWhiteSpace(x.Name)||!Data.Groups.Any(g=>g.Id==x.GroupId))||Data.Students.Select(x=>x.Id).Distinct().Count()!=Data.Students.Count)throw new InvalidDataException("Неверные студенты");
   if(Data.Grades.Any(x=>x==null||x.Id<=0||x.Value<2||x.Value>5||String.IsNullOrWhiteSpace(x.Subject)||!Data.Students.Any(s=>s.Id==x.StudentId))||Data.Grades.Select(x=>x.Id).Distinct().Count()!=Data.Grades.Count)throw new InvalidDataException("Неверные оценки");
  }
  public void Save(){
   Validate(); Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
   string temp=FilePath+".tmp"; File.WriteAllText(temp,new JavaScriptSerializer().Serialize(Data),new UTF8Encoding(false));
   if(File.Exists(FilePath))File.Replace(temp,FilePath,FilePath+".bak");else File.Move(temp,FilePath);
  }
  public static string Required(string value,int max,string label){
   if(String.IsNullOrWhiteSpace(value)||value.Trim().Length>max)throw new ArgumentException(label+": пустое или слишком длинное значение");return value.Trim();
  }
 }
}
