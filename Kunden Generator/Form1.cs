using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kunden_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string key1 = "BNGFMa7BqBBWVztOvGaEK21GDlZB313vEz0hAYJQqbDcAWQVtQ"; 
            string key2 = "p3JgAcrpElhxHMXxToms4CtyEUkkFE1VxA2YDkd8vlv2XjqHxDZs5jRA41LzwClT3dIwnLuJXQC";
            NK_ITMedia_License.License Lic = new NK_ITMedia_License.License(key1, key2);
            idBox.Text = Lic.GetHWID();
        }
    }
}
