﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMSTuiSongJointOffice
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());



            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new JointOfficeRiQingService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
