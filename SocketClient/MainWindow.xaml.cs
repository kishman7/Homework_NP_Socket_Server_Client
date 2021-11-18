using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Socket
        //Connect
        //Send
        //Receive

        const int port = 2020;
        const int size = 1024;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Socket
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Connect
                client.Connect(IPAddress.Parse("127.0.0.1"), port);
                if (client.Connected) //перевіряємо чи клієнт підключений
                {
                    client.Send(Encoding.UTF8.GetBytes(text.Text)); //передаємо повідомлення, яке прописано в TextBox
                    text.Clear();

                    var buffer = new byte[size];
                    client.Receive(buffer); //отримуємо відповідь від сервера
                    result.Content = Encoding.UTF8.GetString(buffer); //виводимо відповідь в Label
                }
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
