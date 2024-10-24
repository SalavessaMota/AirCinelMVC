namespace AirCinelMVC.Data.Dtos
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Confirm { get; set; }
    }
}