using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Models;

namespace KKT_APP_FA.Units
{
    //==============================================================================================================================================
    //==============================================================================================================================================

    // Работа с аппаратом ТерминаФА (физический уровень)
    public class TerminalFA : IDisposable
    {
        private int READ_TIMEOUT; // таймаут чтения с порта (потока), сек
        private int BUFFER_SIZE; // размер буфера чтения с порта (потока), байт
        private string IP; // IP
        private int PORT; // PORT

        TcpClient tcpClient;
        NetworkStream Port;

        //==============================================================================================================================================
        public TerminalFA(TerminalFASettings settings)
        {
            this.READ_TIMEOUT = settings.READ_TIMEOUT;
            this.BUFFER_SIZE = settings.BUFFER_SIZE;
            this.IP = settings.IP;
            this.PORT = settings.PORT;
            tcpClient = new TcpClient();
            //this.Initialize(); // тут не надо!!!
        }

        //==============================================================================================================================================

        // 1. Соединение с аппаратом, задание сетевых настроек (вызывается снаружи, с логического уровня):
        public TerminalConnectResult Connect()
        {
            try
            {
                tcpClient.Connect(IP, PORT); // параметры коннекта с аппаратом
                Port = tcpClient.GetStream();
                return new TerminalConnectResult() { Connected = true, Code = 0, Description = "Connected!" };
            }
            catch (Exception e)
            {
                return new TerminalConnectResult() { Connected = false, Code = -1, Description = e.Message };
            }
        }

        //==============================================================================================================================================

        // 2. Отправка запроса в аппарат:
        public void SendRequest(byte[] command)
        {
            try
            {
                Port.Write(command, 0, command.Length);
            }
            catch (Exception)
            {
            }
        }

        //==============================================================================================================================================

        // 3. Чтение ответа аппарата:
        public byte[] ReadResponse()
        {
            DateTime startTime = DateTime.Now;

            while (DateTime.Now.Subtract(startTime) < TimeSpan.FromSeconds(READ_TIMEOUT))
            {
                Thread.Sleep(250);
                try
                {
                    var response_list = new List<byte>();
                    byte[] response_temp = new byte[BUFFER_SIZE];
                    var res = BUFFER_SIZE;
                    while (res == BUFFER_SIZE)
                    {
                        res = Port.Read(response_temp, 0, response_temp.Length);
                        response_list.AddRange(response_temp.Take(res).ToList()); //response_temp
                    }
                    var response = new byte[response_list.Count()];
                    response = response_list.ToArray();

                    return response;
                }
                catch (Exception)
                {
                }
                // Dispose не надо, будем запускать с логического уровня
            }
            return null;
        }

        //==============================================================================================================================================

        // 4. Уничтожение (вызывается снаружи, с логического уровня):
        public virtual void Dispose()
        {
            if (Port != null) Port.Dispose();
        }

        //==============================================================================================================================================
    }
}
