using GSMMODEM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TMSTuiSongJointOffice.Classes;

namespace TMSTuiSongJointOffice
{
    public partial class JointOfficeRiQingService : ServiceBase
    {
        public int shiJian = 60;//60*60;///默认一分钟
        System.Timers.Timer MT = null;
        private JointOfficeRiQingDayService sMSRiQingDayService;
        public GsmModem port = new GsmModem();

        public JointOfficeRiQingService()
        {
            InitializeComponent();
            //获取间隔推送时间
            shiJian = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMSRiQingTime"].ToString());

            if (shiJian == 0)
            {
                shiJian = 1;
            }
            sMSRiQingDayService = new JointOfficeRiQingDayService();
            InitService();
        }
        private void InitService()
        {
            base.AutoLog = true;
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.ServiceName = "JointOfficeRiQingService";


        }
        public void Star()
        {
            try
            {
                //EventLogEx ex = new EventLogEx();
                //ex.WriteEntryEx("开始启动时间触发.");
                MyTimer();
                sMSRiQingDayService.Star();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                MyTimer();
                sMSRiQingDayService.Star();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MyTimer()
        {
            MT = new System.Timers.Timer(shiJian * 1000);

            MT.Elapsed += new System.Timers.ElapsedEventHandler(MTimedEvent);
            MT.Enabled = true;
            MT.Start();


        }

        private void MTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MT.Enabled = false;//

                //获得时间
                DateTime currentTime = System.DateTime.Now;

                try
                {
                    //删除文件
                    DirectoryInfo aDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory + "IMG\\"));
                    FileInfo[] files = aDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                    foreach (FileInfo f in files)
                    {
                        File.Delete(f.FullName);
                    }
                }
                catch (Exception ex) { }


                string title = "";
                //代码
                string html = "";

                DateTime endTime = currentTime;
                DataTable rqsj = BTiMoShiShuJu.GetRiQingWeiXinTuiSongjob();

                if (rqsj == null || rqsj.Rows.Count == 0)
                {
                    MT.Enabled = true;
                    return;
                }


                int width = 0;

                //目前是有job执行
                foreach (DataRow r in rqsj.Rows)
                {
                    if (r["MoBanTime"] == null || "".Equals(r["MoBanTime"].ToString()))
                    {
                        continue;
                    }
                    if (r["Name"] == null || "".Equals(r["Name"].ToString()))
                    {
                        continue;
                    }
                    if (r["JobName"] == null || "".Equals(r["JobName"].ToString()))
                    {
                        continue;
                    }
                    if (r["TMSstate"].ToString() == "1")
                    {
                        continue;
                    }

                    string insertDate = r["MoBanTime"].ToString();
                    string insertJob = r["JobName"].ToString();
                    string insertName = r["Name"].ToString();
                    string insertID = r["Id"].ToString();


                    int height = 60;
                    html = @"
                    <!DOCTYPE html PUBLIC '- //W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                        <html xmlns='http: //www.w3.org/1999/xhtml'>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                            <style type='text/css'>
                                table.tableizer-table {
                                    font-size: 15px;
                                    border-collapse: collapse;
                                    width: 500px;
                                    word-break: break-all;
                                    word-wrap: break-word;
                                    font-family: 微软雅黑;
                                }

                                .tableizer-table td {
                                    min-width:80px;
                                    border: 1px solid #000;
                                    word-wrap: break-word;
                                }
                            </style>
                        </head>
                        <body>";

                    width = 620;
                    html += @"

                            <table class='tableizer-table' id='OnePage'>
                                <tr>
                                    <td style='width:35px;'>姓名</td>
                                    <td style='color:red;font-size:15px;width:80px;'>" + insertName + @"</td>
                                    <td style='width:35px'>职务</td>
                                    <td>" + insertJob + @"</td>
                                    <td style='width:35px;'>时间</td>
                                    <td style='font-size:15px;color:red;width:145px;'>" + insertDate + @"</td>
                                </tr>
                            </table>
                            <table class='tableizer-table' id='OnePage'>
                                <tr>
                                    <td style='background:#dee9f4;border-width:1px'>工作计划</td>
                                </tr>
                                <tr>
                                    <td> " + r["WorkPlan"].ToString() + @" </td>
                                </tr>
                                <tr>
                                    <td style='background:#dee9f4;border-width:1px'>工作总结</td>
                                </tr>
                                <tr>                               
                                    <td> " + r["WorkSummary"].ToString() + @" </td>                                    
                                </tr>
                                <tr>
                                    <td style='background:#dee9f4;border-width:1px'>工作经验</td>
                                </tr>
                                <tr>
                                    <td> " + r["Experience"].ToString() + @" </td>
                                </tr>
                            </table>
                        </ body >
                    </ html > ";


                    string FileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + "_" + "mr" + "_" + "test4".ToString();
                    string path = System.AppDomain.CurrentDomain.BaseDirectory + "IMG\\" + FileName + ".html";
                    File.WriteAllText(path, html.ToString(), Encoding.UTF8);

                    Thread NewTh = new Thread(SC_IMG);
                    NewTh.SetApartmentState(ApartmentState.STA);
                    NewTh.Start(new object[] { FileName, path, height, width, title, insertDate, insertJob, insertName, insertID});
                    while (NewTh.ThreadState == System.Threading.ThreadState.Running)
                    {

                    }
                }
                
                MT.Enabled = true;
            }
            catch (Exception ex)
            {
                MT.Enabled = true;
            }
        }
        public void SC_IMG(object para)
        {
            WebPageSnapshot wps = new WebPageSnapshot();
            var arr = (Object[])para;
            wps.Url = arr[1].ToString();
            try
            {
                string path1 = System.AppDomain.CurrentDomain.BaseDirectory + "IMG\\" + arr[0] + ".JPG";
                //保存到文件
                wps.TakeSnapshot(arr[2].ToString(),arr[3].ToString()).Save(path1);
                byte[] by = SaveImage(path1);
                string name = arr[0] + ".JPG";//+ "_" + arr[4].ToString() 
                string bs = Convert.ToBase64String(by);
                int i = BTiMoShiShuJu.InsertRiQing(name, by, bs);//添加到正式库表
                if (i != 0)
                {
                    int k = BTiMoShiShuJu.UpdateState(arr[8].ToString()); // 添加到正式库后 状态改为已发送
                }
                if (System.IO.File.Exists(wps.Url))//先判断文件是否存在，再执行操作
                    System.IO.File.Delete(wps.Url);//删除html
            }
            catch (Exception ex)
            {

            }
            wps.Dispose();
        }

        //将图片以二进制流
        public byte[] SaveImage(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中
            return imgBytesIn;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }
    }
}
