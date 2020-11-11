using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Windows.Forms;
using System.Net;

namespace TMSTuiSongJointOffice.Classes
{
    public class WebPageSnapshot : IDisposable
    {
        string url = "about:blank";

        /**/
        /// <summary>
        /// 简单构造一个 WebBrowser 对象
        /// 更灵活的应该是直接引用浏览器的com对象实现稳定控制
        /// </summary>
        WebBrowser wb = new WebBrowser();
        /**/
        /// <summary>
        /// URL 地址
        /// http://www.cnblogs.com
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        int width = 1024;
        /**/
        /// <summary>
        /// 图象宽度
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        int height = 768;
        /**/
        /// <summary>
        /// 图象高度
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /**/
        /// <summary>
        /// 初始化
        /// </summary>
        protected void InitComobject(string hei,string wid)
        {
            try
            {
                aWidth = "0";
                aHeight = "0";
                wb.ScriptErrorsSuppressed = false;
                wb.ScrollBarsEnabled = false;
                wb.Navigate(this.url);
                HtmlElementCollection ElementCollection = wb.Document.GetElementsByTagName("table");
                //因为没有窗体，所以必须如此
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                    System.Windows.Forms.Application.DoEvents();

                foreach(HtmlElement html in ElementCollection)
                {
                //HtmlElement html = ElementCollection[0];
                    aWidth = html.OffsetRectangle.Width.ToString();
                    aHeight = (int.Parse(aHeight)+(html.OffsetRectangle.Height)).ToString();
                }
                aWidth =   (int.Parse(aWidth) + 20).ToString();
                aHeight = (int.Parse(aHeight) + 40).ToString();

                wb.Size = new Size(int.Parse(aWidth), int.Parse(aHeight));
                wb.Stop();
                if (wb.ActiveXInstance == null)
                    throw new Exception("实例不能为空");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private string aWidth = "0";
        private string aHeight = "0";
        /**/
        /// <summary>
        /// 获取快照
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap TakeSnapshot(string hei,string wid)
        {
            try
            {
                InitComobject(hei,wid);
                if ("".Equals(aWidth))
                {
                    aWidth = wid;
                }
                if ("".Equals(aWidth))
                {
                    aHeight = hei;
                }
                //构造snapshot类，抓取浏览器ActiveX的图象
                SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
                return snap.TakeSnapshot(wb.ActiveXInstance, new Rectangle(0, 0, int.Parse(aWidth), int.Parse(aHeight)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void Dispose()
        {
            wb.Dispose();
        }
    }
}