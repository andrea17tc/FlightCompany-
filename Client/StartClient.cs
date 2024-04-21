using Networking.network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.service;

namespace Client
{
    internal static class StartClient
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string ip = Convert.ToString(ConfigurationManager.AppSettings["ip"]);
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

            IService server = new ServerObjectProxy(ip, port);
            Form1 loginWindow = new Form1(server);
            SearchWindow searchWindow = new SearchWindow();
            loginWindow.setSearchWindow(searchWindow);
            Application.Run(loginWindow);
        }
    }
}
