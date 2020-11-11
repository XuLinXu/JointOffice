using System;
using System.Windows.Forms;

namespace TMSTuiSongJointOffice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            JointOfficeRiQingService rq = new JointOfficeRiQingService();
            rq.Star();
            //SMSRiQingDayService day = new SMSRiQingDayService();
            // day.Star();
            //SMSRiQingWeekService week = new SMSRiQingWeekService();
            //week.Star();
            //SMSRiQingMonthService month = new SMSRiQingMonthService();
            //month.Star();
            //TMoShiService s = new TMoShiService();
            //s.Star();
        }
    }
}
