using DataAcceessLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataAcceessLibrary.Data
{
    class JsonContext
    {//i main-- JsonService.WriteToFileCorrect(filepath, new Person("Nataliya", "Lisjo", 19, "Degefors"));
       
        

        public static class JsonService
        {
           
           private static string fileName = "db.json";    
           private static string filepath =Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);

            

            public static async Task CreateFileAsync()
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                filepath = $"Filename={Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName)}";
            }
            public static void WriteToFile(string filepath, Order order)
            {
                var json = JsonConvert.SerializeObject(order);// person object skicka til json 

                StreamWriter writer = new StreamWriter(filepath);//from system.IO
                writer.Write(json);

                //writer.Write(json); //skriver over info --! tilläga //samma som Writeline/ har ! append variant

            }



            public static void ReadFromFile(string filepath)
            {
                StreamReader reader = new StreamReader(filepath);

                var json = reader.ReadToEnd();//läsa hela text

            }


            public static void WriteToFileCorrect(string filepath, Order order)//list--  nån fel hear --skapade inte list i filen

            {

                //problematisk att updarera på den set - det inte vanlig som vi skriver in adta in json fil

                try //om det likas att läsa 
                {
                    StreamReader reader = new StreamReader(filepath);   // hämta info  som finns i filen
                    var json = reader.ReadToEnd();        //och spara i json//fil inte tomt, när den hämta /sträng -tomt , men ! 0
                    reader.Close();//stäng strim delläsa
                    if (json != string.Empty) //if fil finns då vill göra nån
                    {
                        var list = JsonConvert.DeserializeObject<List<Order>>(json); //  1. packa up lista med persons
                        list.Add(order);        //+ i list

                        var json2 = JsonConvert.SerializeObject(list);

                        StreamWriter writer = new StreamWriter(filepath);//kan skriva

                        writer.WriteLine(json2);//skriver over
                        writer.Close();//stänga
                    }
                }

                catch  //om  finns inga filen 
                {
                    StreamWriter writer = new StreamWriter(filepath);  // då skapa vi fil
                    var list = new List<Order>() { order };   //+ person direct i Lista
                                                              //list.Add(person);//flera person
                    var json = JsonConvert.SerializeObject(list);   //den list göra om till json
                                                                    
                    writer.Write(json);
                                        
                }
            }
        }
    }
}
