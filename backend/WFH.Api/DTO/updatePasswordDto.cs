namespace WFH.Api.DTO
{
    public class updatePasswordDto
    {
        public string Email { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
