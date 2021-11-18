using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) new Socket
            // 2) Bind(EP) -> EP = IP port
            // 3) Listen
            // 4) Accept
            // 5) work with client
            // 6) Send responce
            // 7) Close

            // 1)
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = entry.AddressList[1];
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //комбінація протоколів для стрімових Socket

            // 2)
            const int PORT = 2020;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT);
            try
            {
                server.Bind(ep);

                // 3)
                const int BACKLOG = 10;
                server.Listen(BACKLOG); // переходимо в режим прослуховування

                while (true)
                {
                    // 4)
                    Socket client = server.Accept(); // очікуємо клієнта

                    // 5)
                    const int SIZE = 255;
                    int count = 0;
                    var buffer = new byte[SIZE];
                    string data = "";
                    do
                    {
                        int tempCount = client.Receive(buffer); // отримуємо дані з клієнта
                        data += Encoding.UTF8.GetString(buffer, 0, tempCount);
                        count += tempCount;
                    } 
                    while (client.Available > 0); //вичитуємо до тих пір, поки є символи в буфері клієнта

                    Console.WriteLine("Receive: " + data); //виводимо повідомлення, яке отримаємо
                    Console.WriteLine("Receive from: " + client.RemoteEndPoint); //виводимо від кого отримали повідомлення

                    // 6)
                    //перевіряємо чи запит від клієнта співпадає з назвою файла, якщо так, то відкриваємо даний файл, вичитуємо з нього дані
                    //та відправляємо їх для клієнта
                    //var path = Path.Combine("f:\\YURKO\\SHAG\\Project C#\\28_07112021_Lab_System Prog(Register)_Network Prog (OSI, Socket)" +
                    //    "\\Homework_NP_Socket_Server_Client1\\SocketServer\\Cities");
                    var path = Path.Combine("F:\\..\\..\\..\\..\\..\\..\\Cities");
                    //var path = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent
                    //    (Environment.CurrentDirectory).FullName).FullName).FullName, "Сities"); //доступаємось до папки
                    var dir = new DirectoryInfo(path); // папка с файлами 

                    foreach (FileInfo file in dir.GetFiles()) //перебираємо папку по файлам
                    {
                        if (file.ToString() == data) //порівнюємо назву файла з запитом, який прийшов від клієнта
                        {
                            StreamReader f = new StreamReader( data + ".txt"); //відкриваємо файл
                            while (!f.EndOfStream) //считуємо дані з файла
                            {
                                string s = f.ReadLine(); //записуємо дані в рядок
                                client.Send(Encoding.UTF8.GetBytes(s)); //відправляємо дані клієнту
                            }
                            f.Close();
                        }
                        Console.WriteLine(Path.GetFileNameWithoutExtension(file.FullName));
                    }

                    //client.Send(Encoding.UTF8.GetBytes($"Got {count} bytes: " + DateTime.Now.ToShortTimeString())); ; //відправляємо дані клієнту

                    // 7)
                    client.Close(); //закриваємо socket клієнта
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
           


        }
    }
}
