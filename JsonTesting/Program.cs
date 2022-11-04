using System.IO;
var directory = Directory.GetCurrentDirectory();
var path = Path.Combine(directory, "jsondata.json");
Console.WriteLine(path);