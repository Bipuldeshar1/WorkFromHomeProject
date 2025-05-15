//using WFH.Api.dto;
//using WorkFromHome.Domain.models;

//namespace WFH.Api.DTO.Map
//{
//    public static class RequestMap
//    {
//        public static Request ToRequest(this RequestDto requestDto)
//        {
//            return new WorkFromHomeRequest(requestDto.RequestFrom, requestDto.RequestUpTo, Guid.NewGuid(), requestDto.RequestedBy) { };
//        }

//        public static RequestDto ToRequest(this Request requestDto)
//        {
//            return new RequestDto
//            {
//                RequestedBy = requestDto.RequestedBy,
//                RequestFrom = requestDto.RequestFrom,
//                RequestUpTo = requestDto.RequestUpTo,
//            };
//        }
//    }
//}
