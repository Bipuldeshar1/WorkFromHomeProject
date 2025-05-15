using WorkFromHome.Domain.models;

namespace WFH.Api.DTO
{
    public class RegEmpDto
    {
        public Employee employee { get; set; }
        public string password {  get; set; }
    }
}
