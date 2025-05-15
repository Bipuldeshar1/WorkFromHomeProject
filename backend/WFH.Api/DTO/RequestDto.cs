using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WFH.Api.dto
{
    public class RequestDto
    {

        public DateTime RequestFrom { get;  set; }
        public DateTime RequestUpTo { get;  set; }
        public string Reason { get; set; }
        public Guid ManagerGuid { get; set; }
    
    }
}
