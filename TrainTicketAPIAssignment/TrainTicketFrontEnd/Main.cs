namespace TrainTicketFrontEnd
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Schedules schedules = new Schedules();
            schedules.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminLogin adminLogin = new AdminLogin();   
            adminLogin.Show();
        }
    }
}
