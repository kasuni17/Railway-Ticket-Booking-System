using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using TrainTicketAPI.Controllers;

namespace TrainTicketFrontEnd
{
    public partial class AdminLogin : Form
    {
        private readonly HttpClient _httpClient;

        public AdminLogin()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7206/api/");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            // Create an object representing the sign-in data
            var signInData = new
            {
                UserName = userName,
                Password = password
            };
            // Serialize the sign-in data to JSON
            string json = JsonConvert.SerializeObject(signInData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Create an HttpClient instance
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync("User/signin", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to an object
                    var authenticatedUser = JsonConvert.DeserializeObject<AuthenticatedUser>(responseContent);

                    // Authentication successful, handle further actions
                    // For example, store the token and navigate to the next screen
                    MessageBox.Show($"Welcome, {authenticatedUser.Username}!");

                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                }
                else
                {
                    // Handle the case where the request was not successful
                    MessageBox.Show($"Failed to log in: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
