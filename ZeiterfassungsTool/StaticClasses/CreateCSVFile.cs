using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZeiterfassungsTool
{
    public static class CreateCSVFile
    {

        static CreateCSVFile()
        {
            //using (var mem = new MemoryStream())
            //using (var writer = new StreamWriter(mem))
            //using (var csvWriter = new CsvWriter(writer))   

                    
        }
            
    }


    //Beispiel Code von https://www.delftstack.com/de/howto/csharp/how-to-write-data-into-a-csv-file-in-csharp/

    //public class Project
    //{
    //    public string PersonName { get; set; }
    //    public string Title { get; set; }
    //}
    //public class Program
    //{

    //    static void Main(string[] args)
    //    {
    //        var data = new[]
    //        {
    //            new Project { CustomerName = "Olivia", Title = "Mother"},
    //            new Project { CustomerName = "Lili", Title = "Elder Sister"}
    //        };

    //        using (var mem = new MemoryStream())
    //        using (var writer = new StreamWriter(mem))
    //        using (var csvWriter = new CsvWriter(writer))
    //        {
    //            csvWriter.Configuration.Delimiter = ";";
    //            csvWriter.Configuration.HasHeaderRecord = true;
    //            csvWriter.Configuration.AutoMap<Project>();

    //            csvWriter.WriteHeader<Project>();
    //            csvWriter.WriteRecords(data);

    //            writer.Flush();
    //            var result = Encoding.UTF8.GetString(mem.ToArray());
    //            Console.WriteLine(result);
    //        }
    //    }
    //}
}
