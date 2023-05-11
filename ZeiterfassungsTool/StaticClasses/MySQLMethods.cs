using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MySQLModels;

namespace ZeiterfassungsTool.StaticClasses
{
    static class MySQLMethods
    {
        static HttpClient client;
        static JsonSerializerOptions _serializerOptions;
        static string baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        static string urlEmployee = $"{baseAddress}/api/Employee";
        static string urlTimetracking = $"{baseAddress}/api/Timetracking";

        static MySQLMethods()
        {
            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        #region EmployeeMethods

        public static async Task<List<Employee>> GetAllAccounts()
        {
            //var response = await client.GetStringAsync(urlEmployee);
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(urlEmployee);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", $"Es konnte keine Verbindung zum Server hergestellt werden.", "OK");
                Debug.WriteLine(ex);
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                var employees = await client.GetFromJsonAsync<List<Employee>>(urlEmployee);

                //Employee p = (Employee)serializer.Deserialize(new JTokenReader(a), typeof(Employee));
                return employees;
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
                return null;
            }
        }

        public static async Task<List<Employee>> GetSingleAccount(int id)
        {
            var response = await client.GetAsync(string.Format($"{urlEmployee}/{id}"));

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStreamAsync();
                List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(data);
                return employees;
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
                return null;
            }
        }

        public static async Task<List<Employee>> GetSingleAccount(string username)
        {
            var response = await client.GetAsync(string.Format($"{urlEmployee}/{username}"));

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStreamAsync();
                var data2 = await response.Content.ReadAsStringAsync();
                List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(data);
                List<Employee> employees2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(data2);
                return employees;
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
                return null;
            }
        }

        public static async Task SaveAccount(Employee employee)
        {
            string json = JsonSerializer.Serialize(employee, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlEmployee, content);
            //var response = await client.PostAsJsonAsync(testUrl, json);

            Debug.WriteLine("Bla: " + response.StatusCode);
        }

        public static async Task DeleteAccount(int employeeId)
        {
            string json = JsonSerializer.Serialize(employeeId, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(string.Format($"{urlEmployee}/{employeeId}")); 
            //var response = await client.PostAsJsonAsync(testUrl, json);

            Debug.WriteLine("Bla: " + response.StatusCode);
        }

        public static async Task DeleteAllAccounts()
        {
            string json = JsonSerializer.Serialize(_serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(string.Format($"{urlEmployee}"));
            //var response = await client.PostAsJsonAsync(testUrl, json);

            Debug.WriteLine("Bla: " + response.StatusCode);
        }

        public static async Task UpdateAccount(Employee e)
        {
            string json = JsonSerializer.Serialize(e, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(string.Format($"{urlEmployee}/{e.Id}"), content);

            Debug.WriteLine("Bla: " + response.StatusCode);
        }
        #endregion


        #region Timetracking Methods
        //Timetracking Methods
        public static async Task<List<Timetracking>> GetTimetracking(int employeeId)
        {
            //Die SQL Abfrage von der API lautet: "select * from zeiterfassung.timetracking WHERE employeeid = @id";   

            var response = await client.GetAsync(string.Format($"{urlTimetracking}/{employeeId}"));

            if (response.IsSuccessStatusCode)
            {
                //var data = await response.Content.ReadAsStreamAsync();
                var data2 = await response.Content.ReadAsStringAsync();
                //List<Timetracking> timetracking = JsonSerializer.Deserialize<List<Timetracking>>(data);                       //konvertiert die Daten nicht korrekt, da der Wert von typeId immer auf 0 gesetzt wird.
                List<Timetracking> timetracking2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Timetracking>>(data2);
                return timetracking2;
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
                return null;
            }
        }

        public static async Task<List<Timetracking>> GetAllTimetrackingData()
        {
            var response = await client.GetAsync(string.Format($"{urlTimetracking}"));

            if (response.IsSuccessStatusCode)
            {
                //var data = await response.Content.ReadAsStreamAsync();
                var data2 = await response.Content.ReadAsStringAsync();
                //List<Timetracking> timetracking = JsonSerializer.Deserialize<List<Timetracking>>(data);                       //konvertiert die Daten nicht korrekt, da der Wert von typeId immer auf 0 gesetzt wird.
                List<Timetracking> timetracking2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Timetracking>>(data2);
                return timetracking2;
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
                return null;
            }
        }


        public static async Task UpdateTimetracking(Timetracking t)
        {
            //Die SQL Abfrage von der API lautet: 
            //string query = @"
            //            update zeiterfassung.timetracking set 
            //            startTime = @Starttime, 
            //            endTime = @EndTime,
            //            subject = @Subject,
            //            typeId = @TypeId
            //            where id = @Id;
            //";

            string json = JsonSerializer.Serialize(t, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(urlTimetracking, content);   

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStreamAsync();  
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
            }
        }

        public static async Task AddTime(Timetracking t)
        {
            string json = JsonSerializer.Serialize(t, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlTimetracking, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("ok: " + response.StatusCode);
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
            }
        }
        public static async Task DeleteAllTimes()
        {
            var response = await client.DeleteAsync(urlTimetracking);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("ok: " + response.StatusCode);
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
            }
        }

        public static async Task DeleteTime(int id)
        {
            var response = await client.DeleteAsync(string.Format($"{urlTimetracking}/{id}"));

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("ok: " + response.StatusCode);
            }
            else
            {
                Debug.WriteLine("fehler: " + response.StatusCode);
            }
        }

        #endregion
    }
}
