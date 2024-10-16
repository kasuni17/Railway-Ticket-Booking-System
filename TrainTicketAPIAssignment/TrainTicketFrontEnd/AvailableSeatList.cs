using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TrainTicketAPI.Controllers.TicketController;

namespace TrainTicketFrontEnd
{
    public partial class AvailableSeatList : Form
    {
        private string seatList;
        private object scheduleId;

        private readonly HttpClient _httpClient;

        public AvailableSeatList(string seatsInfo, object scheduleId)
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7206/api/");

            this.seatList = seatsInfo;
            this.scheduleId = scheduleId;

            if (seatList == null)
            {
                MessageBox.Show("No data to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                label2.Text = seatList;
                label6.Text = Convert.ToInt32(scheduleId).ToString();
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate NIC
                if (string.IsNullOrWhiteSpace(NICTextBox.Text))
                {
                    MessageBox.Show("NIC is required.");
                    return;
                }

                // Parse seat numbers
                var seatNumbers = new List<long>();
                if (!string.IsNullOrWhiteSpace(SeatNumbersTextBox.Text))
                {
                    var seatNumbersInput = SeatNumbersTextBox.Text.Split(',');
                    foreach (var seatNumberInput in seatNumbersInput)
                    {
                        if (long.TryParse(seatNumberInput.Trim(), out long seatNumber))
                        {
                            seatNumbers.Add(seatNumber);
                        }
                        else
                        {
                            MessageBox.Show($"Invalid seat number: {seatNumberInput}");
                            return;
                        }
                    }
                }

                // Check maximum seats
                if (seatNumbers.Count > 5)
                {
                    MessageBox.Show("Maximum 5 seats can be reserved per ticket.");
                    return;
                }

                var requestData = new TicketBookingData
                {
                    ScheduleID = Convert.ToInt64(scheduleId), 
                    NIC = NICTextBox.Text,
                    ReservedSeats = seatNumbers
                };


                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Ticket/bookticket", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Ticket booked successfully
                    var ticketInfo = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    MessageBox.Show($"Ticket booked successfully!\nTicket ID: {ticketInfo.ticketID}\nTicket Amount: {ticketInfo.ticketAmount}\nSchedule ID: {ticketInfo.scheduleID}\nReserved Seats: {ticketInfo.reservedSeats}");
                    // Optionally, clear input fields or perform any other actions
                }
                else
                {
                    // Failed to book ticket
                    MessageBox.Show("Failed to book ticket: " + responseContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}