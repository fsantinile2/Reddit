using System.Net;

Console.WriteLine("Hello user!, this will show how many threads/news were created in the last minute");
List<int> listQuantity = new List<int>();
var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

while (await timer.WaitForNextTickAsync())
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7262/api/reddit/news");
    request.Method = "GET";

    var response = (HttpWebResponse)request.GetResponse();

    Stream dataStream = response.GetResponseStream();
    StreamReader reader = new StreamReader(dataStream);
    string content = reader.ReadToEnd();

    listQuantity.Add(int.Parse(content));

    Console.WriteLine($"{content} news were created in the last minute at {DateTime.Now.Hour}:{DateTime.Now.Minute}");
    Console.WriteLine($"aravge per minute: {listQuantity.Average()}");

}

Console.ReadKey();





