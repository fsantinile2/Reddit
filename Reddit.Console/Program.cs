using System.Net;

List<int> listQuantity = new List<int>();
var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

Console.WriteLine("Hello user!, this will show how many threads/news were created in the last minute");

while (await timer.WaitForNextTickAsync())
{
    HttpClient request = new HttpClient();

    var response = await request.GetAsync("https://localhost:7262/api/reddit/news");
    string content = await response.Content.ReadAsStringAsync();

    listQuantity.Add(int.Parse(content));

    Console.WriteLine($"{content} news were created in the last minute at {DateTime.Now.Hour}:{DateTime.Now.Minute}");
    Console.WriteLine($"aravge per minute: {listQuantity.Average()}");

}

Console.ReadKey();





