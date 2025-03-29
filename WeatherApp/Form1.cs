using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private string APIKey = "05e8f64bd988a5dd9da27e329ee27d3e";

        public Form1()
        {
            InitializeComponent();
            LoadSearchHistory(); // Load history when form opens
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSearchHistory();
        }

        private void LoadSearchHistory()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
                                      AppDomain.CurrentDomain.BaseDirectory +
                                      "WeatherApp.mdf;Integrated Security=True;Connect Timeout=30;";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CityName, SearchDate FROM SearchHistory ORDER BY SearchDate DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        searchHistoryListBox.Items.Clear(); // Clear previous history
                        while (reader.Read())
                        {
                            string cityName = reader.GetString(0);
                            DateTime searchDate = reader.GetDateTime(1);
                            searchHistoryListBox.Items.Add($"{cityName} - {searchDate:g}");
                        }
                    }
                }
            }
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            GetWeather();
        }

        private void GetWeather()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string url = $"https://api.openweathermap.org/data/2.5/weather?q={searchTextbox.Text}&appid={APIKey}";
                    var json = web.DownloadString(url);
                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    // Display weather details
                    labelCondition.Text = Info.weather[0].main;
                    labelDetails.Text = Info.weather[0].description;
                    labelSunrise.Text = ConvertDateTime(Info.sys.sunrise).ToShortTimeString();
                    labelSunset.Text = ConvertDateTime(Info.sys.sunset).ToShortTimeString();
                    labelWindSpeed.Text = Info.wind.speed.ToString();
                    labelPressure.Text = Info.main.pressure.ToString();

                    // Save search history
                    SaveSearchHistory(searchTextbox.Text);
                }
            }
            catch (Exception)
            {
                // Display an error message if the city is not found
                labelCondition.Text = "City Doesn't Exist";
                labelDetails.Text = "";
                labelSunrise.Text = "";
                labelSunset.Text = "";
                labelWindSpeed.Text = "";
                labelPressure.Text = "";
            }
        }

        private void SaveSearchHistory(string cityName)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
                                      AppDomain.CurrentDomain.BaseDirectory +
                                      "WeatherApp.mdf;Integrated Security=True;Connect Timeout=30;";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO SearchHistory (CityName, SearchDate) VALUES (@CityName, @SearchDate)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CityName", cityName);
                    command.Parameters.AddWithValue("@SearchDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }

            LoadSearchHistory(); // Reload history after saving
        }

        private DateTime ConvertDateTime(long sec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            return day.AddSeconds(sec).ToLocalTime();
        }
    }
}
