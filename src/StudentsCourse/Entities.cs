using System;
using System.Collections.Generic;
namespace StudentsCourse {
 public class Group { public int Id {get;set;} public string Code {get;set;} }
 public class Student { public int Id {get;set;} public string Name {get;set;} public int GroupId {get;set;} }
 public class Grade { public int Id {get;set;} public int StudentId {get;set;} public string Subject {get;set;} public int Value {get;set;} }
 public class Database {
  public List<Group> Groups {get;set;} public List<Student> Students {get;set;} public List<Grade> Grades {get;set;}
  public Database(){Groups=new List<Group>();Students=new List<Student>();Grades=new List<Grade>();}
 }
 public interface IModule { string Title {get;} int Order {get;} void Execute(Store store); }
}
