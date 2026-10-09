using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
namespace StudentsCourse {
 public class Program {
  public static int Move(int current,int delta,int count){return (current+delta+count)%count;}
  public static IModule[] Modules(){return Assembly.GetExecutingAssembly().GetTypes().Where(t=>typeof(IModule).IsAssignableFrom(t)&&!t.IsInterface).Select(t=>(IModule)Activator.CreateInstance(t)).OrderBy(t=>t.Order).ToArray();}
  static int Menu(string[] names){
   if(Console.IsInputRedirected){for(int i=0;i<names.Length;i++)Console.WriteLine((i+1)+". "+names[i]);int v;return Int32.TryParse(Console.ReadLine(),out v)&&v>0&&v<=names.Length?v-1:names.Length-1;}
   int selected=0;
   while(true){Console.Clear();Console.WriteLine("АС Студенты — выпадающее меню\n↑/↓ — выбор, Enter — открыть\n");for(int i=0;i<names.Length;i++)Console.WriteLine((i==selected?"> ":"  ")+names[i]);var key=Console.ReadKey(true).Key;
    if(key==ConsoleKey.UpArrow)selected=Move(selected,-1,names.Length);if(key==ConsoleKey.DownArrow)selected=Move(selected,1,names.Length);if(key==ConsoleKey.Enter)return selected;if(key==ConsoleKey.Escape)return names.Length-1;
   }
  }
  public static string Read(string title){Console.Write(title+": ");string v=Console.ReadLine();if(v==null)throw new OperationCanceledException("Ввод завершён");return v;}
  public static int ReadId(string title){int id;if(!Int32.TryParse(Read(title),out id))throw new ArgumentException("Нужен целый номер");return id;}
  public static int Main(string[] args){
   Console.OutputEncoding=Encoding.UTF8;Console.InputEncoding=Encoding.UTF8;
   try{
    if(args.Length>0&&args[0]=="--self-test"){var tests=Assembly.GetExecutingAssembly().GetType("StudentsCourse.SelfTests");return (int)tests.GetMethod("Run").Invoke(null,new object[]{args.Length>1?args[1]:"test-results"});}
    if(args.Length>0&&args[0]=="--demo"){var tests=Assembly.GetExecutingAssembly().GetType("StudentsCourse.SelfTests");tests.GetMethod("Demo").Invoke(null,new object[]{args.Length>1?args[1]:"demo.json"});return 0;}
    var store=new Store(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"data","students.json"));var modules=Modules();string[] menu=modules.Select(m=>m.Title).Concat(new[]{"Выход"}).ToArray();
    while(true){int chosen=Menu(menu);if(chosen==modules.Length)return 0;try{modules[chosen].Execute(store);}catch(ArgumentException e){Console.WriteLine("Отказ: "+e.Message);}catch(OperationCanceledException){return 0;}if(!Console.IsInputRedirected){Console.WriteLine("Нажмите любую клавишу");Console.ReadKey(true);}}
   }catch(Exception e){Console.Error.WriteLine("Ошибка: "+(e.InnerException??e).Message);return 1;}
  }
 }
}
