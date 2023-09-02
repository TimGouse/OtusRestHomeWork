using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

            Console.WriteLine("Введите ID клиента:");
            string idInput = Console.ReadLine();
            long id = long.Parse(idInput);

            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:5000/customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Received JSON: {content}");
                Customer customer = JsonSerializer.Deserialize<Customer>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (customer != null)
                {
                    Console.WriteLine($"Найден клиент: {customer.Firstname} {customer.Lastname}");
                }
                else
                {
                    Console.WriteLine("Не удалось десериализовать клиента.");
                }
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }

            CustomerCreateRequest newCustomer = RandomCustomer();
            string json = JsonSerializer.Serialize(newCustomer);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage createResponse = await httpClient.PostAsync("http://localhost:5000/customers", data);
            if (createResponse.IsSuccessStatusCode)
            {
                string newId = await createResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Создан новый клиент с ID: {newId}");
            }
            else
            {
                Console.WriteLine("Ошибка при создании клиента.");
            }
            string idInputp = Console.ReadLine();
        }

        private static CustomerCreateRequest RandomCustomer()
        {
            string firstName = "John" + Guid.NewGuid().ToString().Substring(0, 4);
            string lastName = "Doe" + Guid.NewGuid().ToString().Substring(0, 4);
            return new CustomerCreateRequest(firstName, lastName);
        }
    }
}