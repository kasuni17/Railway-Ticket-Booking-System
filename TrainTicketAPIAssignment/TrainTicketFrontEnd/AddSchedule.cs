using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrainTicketAPI.Controllers.ScheduleController;
using static TrainTicketAPI.Controllers.TrainController;

namespace TrainTicketFrontEnd
{
    public partial class AddSchedule : Form
    {
        private readonly HttpClient _httpClient;

        public AddSchedule()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7206/api/");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var scheduleData = new ScheduleData
                {
                    Date = dateTimePicker1.Value, // Assuming you have a DateTimePicker control named dateTimePicker1 for selecting the date
                    StartStation = textBox1.Text,
                    DestinationStation = textBox2.Text,
                    TrainID = textBox3.Text, // Assuming textBox3 contains the TrainID as a string
                    PricePerPerson = decimal.Parse(textBox4.Text), // Assuming textBox4 contains the PricePerPerson as a string
                    AvailableSeats = textBox5.Text.Split(',').Select(int.Parse).ToList() // Assuming textBox5 contains comma-separated seat numbers
                };

                var json = JsonConvert.SerializeObject(scheduleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Schedule/addschedule", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Schedule added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to add schedule. Error: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
