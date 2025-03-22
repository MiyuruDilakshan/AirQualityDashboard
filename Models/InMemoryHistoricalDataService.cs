namespace AirQualityDashboard.Models
{
    public class InMemoryHistoricalDataService
    {
        private readonly Dictionary<int, List<HistoricalReading>> _data = new();

        public void AddReading(int sensorId, int aqi)
        {
            if (!_data.ContainsKey(sensorId))
            {
                _data[sensorId] = new List<HistoricalReading>();
            }
            _data[sensorId].Add(new HistoricalReading { AQI = aqi, RecordedAt = DateTime.UtcNow });
        }

        public List<HistoricalReading> GetReadings(int sensorId, int count)
        {
            if (_data.TryGetValue(sensorId, out var readings))
            {
                return readings.OrderByDescending(r => r.RecordedAt).Take(count).ToList();
            }
            return new List<HistoricalReading>();
        }
    }
}