using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTicketFrontEnd
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateTrain updatetrain = new UpdateTrain();
            updatetrain.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTrain addTrain = new AddTrain();
            addTrain.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddSchedule addSchedule = new AddSchedule();
            addSchedule.Show();
        }
    }
}
