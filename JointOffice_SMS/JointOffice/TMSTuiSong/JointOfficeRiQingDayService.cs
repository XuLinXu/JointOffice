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
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TMSTuiSongJointOffice.Classes;

namespace TMSTuiSongJointOffice
{
    public partial class JointOfficeRiQingDayService : ServiceBase
    {   
        public int shiJian = 60;//60*60;///默认一分钟
        public string starttime = "10:00:00";
        public string endtime = "10:30:00";
        System.Timers.Timer MT = null;
        public  GsmModem port = new GsmModem();

        public JointOfficeRiQingDayService()
        {
            InitializeComponent();
            //获取间隔推送时间
            shiJian = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMSRiQingDayTime"].ToString());
            starttime = System.Configuration.ConfigurationManager.AppSettings["SMSRiQingDayStart"].ToString();
            endtime = System.Configuration.ConfigurationManager.AppSettings["SMSRiQingDayEnd"].ToString();

            if (shiJian == 0)
            {
                shiJian = 1;
            }
            InitService();
        }
        private void InitService()
        {
            base.AutoLog = true;
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.ServiceName = "JointOfficeRiQingDayService";


        }
        public void Star()
        {
            try
            {
                MyTimer();
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
        private bool startstop = true;
        private void MTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            MT.Enabled = false;//

            //周日停止
            string week = DateTime.Today.DayOfWeek.ToString();
            if (week.Equals("Sunday") || week.Equals("Saturday"))  //Saturday
            {
                MT.Enabled = true;
                return;
            }

            //获得时间
            DateTime currentTime = System.DateTime.Now;
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + starttime);
            DateTime time1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + endtime);

            string dataTime = "";
            if (week.Equals("Monday"))
            {
                dataTime = currentTime.AddDays(-3).ToString("yyyy-MM-dd");
            }else
            {
                dataTime = currentTime.AddDays(-1).ToString("yyyy-MM-dd");
            }

            //超时间退出
            if (currentTime > time1 || currentTime < time)
            {
                startstop = true;
                MT.Enabled = true;
                return;
            }

            //时间范围内开始
            if (currentTime >= time && currentTime <= time1 && startstop)
            {
                try
                {
                    //删除文件
                    DirectoryInfo aDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory + "IMGD\\"));
                    FileInfo[] files = aDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                    foreach (FileInfo f in files)
                    {
                        File.Delete(f.FullName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }


                string title = "";
                //代码
                string html = "";
                
                DataTable job = new DataTable();
                DataTable daylist = new DataTable();

                job = BTiMoShiShuJu.GetRiQingWeiXinTuiSongID();
                var value = ConvertToModel(job).FirstOrDefault();
                if (value == null)
                {
                    Console.WriteLine("未配置人员");
                }


                daylist = BTiMoShiShuJu.GetRiQingWeiXinTuiSongDayhuibao(value.value, dataTime);//日志

                int width = 260;
                int height = 60;
                html = @"
                    <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'> 
                    <html xmlns='http://www.w3.org/1999/xhtml'> 
                    <head> 
                    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /> 
                    <style type='text/css'>
                            table.tableizer-table {
                            font-size: 12px;
                            border-collapse:collapse;
                            font-family: 微软雅黑;

                            } 
                            .tableizer-table td {
                            border: 1px solid #000;
                            }
                        </style>
                    </head> 
                    <body>
                            <table class='tableizer-table' id = 'OnePage'>
                            <tr>
                                <td colspan=3 style='color:red' align='center'>"+ dataTime + @"日志填写汇报</td>
                            </tr>
                            <tr>
			                            <td>No.</td>
			                            <td>姓名</td>
			                            <td>状态</td>
                            </tr>";

                int j = 0;
                var state = "";
               
                foreach (DataRow rq in daylist.Rows)
                {

                    if (rq["MoBanTime"] == DBNull.Value )
                    {
                        state = "暂未填写日志";
                    }
                    else
                    { 
                        state = "已填写日志";
                    }

                    html += @"<tr>
			                        <td>" + (j += 1) + @"</td>
			                        <td>" + rq["Name"].ToString() + @"</td>
			                        <td>" + state.ToString() + @"</td>
                                </tr>";


                }

                html += @"
	                            <tr>
                                    <td colspan=3 style='color:red'>备注：每天上午12点推送前一天日志至微信群</td>
                                </tr>
             </table>
                    </body> 
                    </html>";


                string FileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + "_" + "rqrhb" + "_" + "test4";
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "IMGD\\" + FileName + ".html";
                File.WriteAllText(path, html.ToString(), Encoding.UTF8);



                Thread NewTh = new Thread(SC_IMG);
                NewTh.SetApartmentState(ApartmentState.STA);
                NewTh.Start(new object[] { FileName, path, height, width, title });
                while (NewTh.ThreadState == System.Threading.ThreadState.Running)
                {

                }
                startstop = false;
            }
            MT.Enabled = true;
        }
        public void SC_IMG(object para)
        {
            WebPageSnapshot wps = new WebPageSnapshot();
            var arr = (Object[])para;
            wps.Url = arr[1].ToString();
            try
            {
                string path1 = System.AppDomain.CurrentDomain.BaseDirectory + "IMGD\\" + arr[0] + ".JPG";
                //保存到文件
                wps.TakeSnapshot(arr[2].ToString(),arr[3].ToString()).Save(path1);
                byte[] by = SaveImage(path1);
                string name = arr[0].ToString() + ".JPG";//+ "_" + arr[4].ToString()
                string bs = Convert.ToBase64String(by);
                int i = BTiMoShiShuJu.InsertRiQing(name,by,bs);//添加到正式库表

                if (System.IO.File.Exists(wps.Url))//先判断文件是否存在，再执行操作
                    System.IO.File.Delete(wps.Url);//删除html
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
        private void timer5_Tick(object sender, EventArgs e)
        {

        }
        public static List<KeyValue> ConvertToModel(DataTable dt)
        {

            List<KeyValue> ts = new List<KeyValue>();// 定义集合
            Type type = typeof(KeyValue); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                KeyValue t = new KeyValue();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        public class KeyValue
        {
            public string value { get; set; }
        }
    }

}
