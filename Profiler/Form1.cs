using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Profiler
{
    public partial class Form1 : Form
    {
        private static readonly string myProfilerURL = "https://localhost:7215";
         public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (await SubmitProfilerInfo()) {
                MessageBox.Show("success");
                button1.Enabled = true;
                return;
            }
            MessageBox.Show("Fail");
            button1.Enabled = true;

        }

        public  async Task<bool> SubmitProfilerInfo()
        {
            // Create an instance of the V2ProfilerEntity class
            V2ProfilerEntity profiler = new V2ProfilerEntity()
            {
                FormType = "Create Project 12",
                ProcessorName = "I7-10GEN",
                HardDiskModel = "SSD-512GB",
                OS = "Windows X",
                NetFrameworkVersion = "v.4.1.2",
                ProductName = "prisma payroll",
                ProductVersion = "v.0.0.1"
            };

            // Serialize the V2ProfilerEntity object to a JSON string
            string json = JsonConvert.SerializeObject(profiler);

            // Create a new instance of HttpClient
            HttpClient httpClient = new HttpClient();

            // Set the base URL of the API endpoint
            httpClient.BaseAddress = new Uri(myProfilerURL);

            // Create the HTTP content with the serialized JSON string
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request with the JSON content to the API endpoint
            HttpResponseMessage response = await httpClient.PostAsync("api/profiler", content);

            // Check the response status code
            if (response.IsSuccessStatusCode)
            {
                // Request succeeded
                Console.WriteLine("Profiler info submitted successfully");
                return true;
            }
            else
            {
                // Request failed
                Console.WriteLine("Failed to submit profiler info");
                return false;

            }
        }

        public string SerializeV2Profiler(V2ProfilerEntity profiler)
        {
            string s = JsonConvert.SerializeObject((object)profiler);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }

    }
}
