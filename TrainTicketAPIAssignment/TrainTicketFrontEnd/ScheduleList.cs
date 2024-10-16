using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TrainTicketAPI.Models;
using System.Net.Http;
using System.Security.Policy;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace TrainTicketFrontEnd
{
    public partial class ScheduleList : Form
    {
        private string startStation;
        private string destinationStation;
        private DateTime date;

        private readonly HttpClient _httpClient;

        public class Schedule
        {
            public long Id { get; set; }
            public string TrainID { get; set; }
            public decimal PricePerUnit { get; set; }
        }

        public ScheduleList(string startStation, string destinationStation, DateTime date)
        {
            InitializeComponent();

            this.startStation = startStation;
            this.destinationStation = destinationStation;
            this.date = date;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7206/api/");

            LoadSchedulesAsync();
        }
        private async Task LoadSchedulesAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = $"Schedule/findtrains?starStation={startStation}&destinationStation={destinationStation}&date={date:yyyy-MM-dd}";
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var schedules = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Schedule>>(json);

                    if (schedules.Count > 0)
                    {
                        dataGridView1.DefaultCellStyle.Font = new Font("Century Gothic", 13);
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView1.DataSource = schedules;

                        DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                        buttonColumn.Name = "AvailableSeats";
                        buttonColumn.HeaderText = "Available Seats";
                        buttonColumn.Text = "Check Seats";
                        buttonColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(buttonColumn);

                        dataGridView1.CellContentClick += DataGridView1_CellContentClick;
                    }
                    else
                    {
                        MessageBox.Show("No schedules found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["AvailableSeats"].Index)
            {
                try
                {
                    var scheduleId = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;

                    if (scheduleId != null)
                    {
                        string uri = $"Schedule/findavailableseats?scheduleId={scheduleId}&";
                        HttpResponseMessage response = await _httpClient.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            string seatsInfo = await response.Content.ReadAsStringAsync();

                            // Navigate to AvailableSeatList screen
                            AvailableSeatList availableSeatListForm = new AvailableSeatList(seatsInfo, scheduleId);
                            availableSeatListForm.Show();
                        }
                        else
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                string errorMessage = await response.Content.ReadAsStringAsync();
                                MessageBox.Show($"No available seats for this schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid schedule or train ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
