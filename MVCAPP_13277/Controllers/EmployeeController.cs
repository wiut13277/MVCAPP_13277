using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCAPP_13277.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVCAPP_13277.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Uri baseAddress = new Uri("http://ec2-34-226-148-36.compute-1.amazonaws.com/");

        private readonly HttpClient _httpClient;

        public EmployeeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            var employees = new List<Employee>();
            var apiCall = await _httpClient.GetAsync("api/employees");

            if (apiCall.IsSuccessStatusCode)
            {
                var response = await apiCall.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            }

            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var employee = new Employee();
            var apiCall = await _httpClient.GetAsync($"api/employees/{id}");

            if (apiCall.IsSuccessStatusCode)
            {
                var response = await apiCall.Content.ReadAsStringAsync();
                employee = JsonConvert.DeserializeObject<Employee>(response);
            }

            return View(employee);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        { 
            return View(new Employee());
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employeeCall = JsonConvert.SerializeObject(employee);
                    var content = new StringContent(employeeCall, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("api/employees", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(employee);
            }
            catch
            {
                return View(employee);
            }
        }

        // GET: EmployeeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var employee = new Employee();
            var response = await _httpClient.GetAsync($"api/employees/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                employee = JsonConvert.DeserializeObject<Employee>(message);
            }

            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee employee)
        {
            try
            {
                var employeeInfo = JsonConvert.SerializeObject(employee);
                var content = new StringContent(employeeInfo, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/employees/{id}", content);
                if (response.IsSuccessStatusCode)
                { 
                    return RedirectToAction(nameof(Index));
                }

                return View(employee);
            }
            catch
            {
                return View(employee);
            }
        }

        // GET: EmployeeController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var employee = new Employee();
            var response = await _httpClient.GetAsync($"api/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                employee = JsonConvert.DeserializeObject<Employee>(message);
            }

            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/employees/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
