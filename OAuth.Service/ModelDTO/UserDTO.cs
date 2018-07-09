
namespace OAuth.Service.ModelDto
{
    public class UserDto : UserInfo
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string DigitalCertificate { get; set; }

        public byte Status { get; set; }

    }
}
