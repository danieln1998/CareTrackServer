using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CareTrack.API.Repositories;
using CareTrack.API.Models.Domain;
using Microsoft.Extensions.DependencyInjection;
using CareTrack.API.Data;

namespace CareTrack.API.Services
{
    public class ArduinoDataService : IHostedService
    {
        private readonly ILogger<ArduinoDataService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory; 
        private Timer _timer;

        public ArduinoDataService(ILogger<ArduinoDataService> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _scopeFactory = scopeFactory; 
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ArduinoDataService is starting.");
            _timer = new Timer(FetchDataFromArduino, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void FetchDataFromArduino(object state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope()) 
                {
                    var alertRepository = scope.ServiceProvider.GetRequiredService<IAlertRepository>(); 
                    var dbContext = scope.ServiceProvider.GetRequiredService<CareTrackDbcontext>(); 

                    string url = "http://10.0.0.16:7155/index";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(responseData))
                        {
                            var arduinoData = JsonSerializer.Deserialize<ArduinoData>(responseData);

                            if (arduinoData != null &&
                                (arduinoData.btnPressed != "normal" || arduinoData.emptyBed != "normal"))
                            {
                                var patient = dbContext.Patients
                                    .FirstOrDefault(p => p.DeviceId.HasValue && p.Device.DeviceNumber == arduinoData.deviceNumber);

                                if (patient != null)
                                {
                                    var alert = new Alert
                                    {
                                        Name = arduinoData.btnPressed != "normal" ? arduinoData.btnPressed : arduinoData.emptyBed,
                                        PatientId = patient.Id
                                        
                                    };

                                    await alertRepository.CreateAsync(alert);
                                    _logger.LogInformation($"Alert created for device {arduinoData.deviceNumber}");
                                }
                                else
                                {
                                    _logger.LogWarning($"No patient found for device {arduinoData.deviceNumber}");
                                }
                            }

                            _logger.LogInformation($"Received data from Arduino: {responseData}");
                        }
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("No data available.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ArduinoDataService is stopping.");
            _timer?.Change(Timeout.Infinite, 0); 
            return Task.CompletedTask;
        }
    }

    public class ArduinoData
    {
        public string btnPressed { get; set; }
        public string emptyBed { get; set; }
        public int deviceNumber { get; set; }
    }
}
