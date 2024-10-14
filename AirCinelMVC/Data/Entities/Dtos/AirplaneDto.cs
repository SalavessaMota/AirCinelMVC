namespace AirCinelMVC.Data.Entities.Dtos
{
    public class AirplaneDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public string ImageFullPath { get; set; }
    }
}
