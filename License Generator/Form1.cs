using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace License_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NK_ITMedia_License.License Lic = new NK_ITMedia_License.License(Key1.Text, Key2.Text);
            endkey.Text = Lic.GenerateLicense(ConvertToUnixTime(dateTimePicker1.Value.Date), false, hardwareid.Text, int.Parse(produktid.Text), vorname.Text, Nachname.Text, forumname.Text);
        }
    }
}