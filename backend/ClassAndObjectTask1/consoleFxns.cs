
//using WorkFromHome.Domain.Enummerations;
//using WorkFromHome.Domain.Exceptions;

//namespace ClassAndObjectTask1
//{
//    public static class ConsoleFxns
//    {

//        public static Employee Login()
//        {
//            Console.Write("email:");
//            string Email = Console.ReadLine();
//            string filePath = GlobalFile.fileUrl + "employee.txt";

//            List<Manager> managers = Serialization.ReadFile<Manager>(filePath);

//            var user = managers.Find(e => e.Email == Email);
//            if (user == null)
//            {
//                throw new UserNotFoundException("user not found");
//            }
//            return user;

//        }


//        public static void AddEmployee()
//        {
//            Console.Write("FirstName: ");
//            string firstName =Console.ReadLine();
//            Console.Write("LastName: ");
//            string lastName = Console.ReadLine();
//            Console.Write("Email: ");
//            string email = Console.ReadLine();
//            Console.Write("PhoneNumber: ");
//            string phoneNumber = Console.ReadLine();

//            Manager m = new Manager(firstName,lastName,email,phoneNumber, RoleEnums.Employee);
//          //  m.AddEmployee(m);
//        }
//        public static void ApplyWorkFromHomeRequest(Employee employee)
//        {

//            Console.Write("Start Date (yyyy-MM-dd):");
//            DateTime startDate = DateTime.Parse(Console.ReadLine());
//            Console.Write("End Date (yyyy-MM-dd): ");
//            DateTime endDate = DateTime.Parse(Console.ReadLine());

//            List<Manager> managers = Serialization.ReadFile<Manager>(GlobalFile.fileNameEmployee);
//            var m = managers.Find(x => x.Role == RoleEnums.Manager);

//            Manager manager = new Manager(m.FirstName, m.LastName, m.Email, m.PhoneNumber, m.Role);


//            WorkFromHomeRequest WFHRequest = new WorkFromHomeRequest(startDate, endDate, m.EmployeeGUID, employee.EmployeeGUID);
//            manager.CreateWorkFromHomeRequest(WFHRequest);
//            employee.SubmitWorkFromHomeRequest(WFHRequest, m);


//        }

//        public static void ShowUserWorkFromHomeRequestStatus(Employee employee)
//        {

//            List<WorkFromHomeRequest> allRequests = new List<WorkFromHomeRequest>();

//            Console.WriteLine($"logged in as {employee.FirstName}");

//            allRequests = Serialization.ReadFile<WorkFromHomeRequest>(GlobalFile.fileNameRequest);

//            List<WorkFromHomeRequest> employeeRequests = allRequests.FindAll(r => r.RequestedBy == employee.EmployeeGUID);
//            if (employeeRequests.Count <= 0)
//            {
//                throw new RequestNotFoundException($"request not found for {employee.FirstName}");
//            }

//            Console.WriteLine("Work from home Request Applied status:");
//            var index = 1;
//            foreach (var request in employeeRequests)
//            {
//                Console.WriteLine($"{index++} - Request status of {employee.FirstName} and request status is {request.RequestStatus}" +
//                    $" WFH will start from {request.RequestFrom} and ends on {request.RequestUpTo} accepted on {request.UpdatedOn} by {request.ApproverIdentifier}  ");
//            }
//        }

//        public static void ShowAllRequest(Employee employee)
//        {

//            var JsonData = File.ReadAllText(GlobalFile.fileNameRequest);
//            List<WorkFromHomeRequest> AllRequest = JsonConvert.DeserializeObject<List<WorkFromHomeRequest>>(JsonData) ?? new List<WorkFromHomeRequest>();
//            List<WorkFromHomeRequest> requests = AllRequest.FindAll(x => x.RequestStatus == RequestStatusEnums.Pending && x.ApproverIdentifier == employee.EmployeeGUID);
//            var json = JsonConvert.SerializeObject(requests, Formatting.Indented);

//            if (requests.Count > 0)
//            {
//                Console.WriteLine("All Pending Requests are:");

//                Console.WriteLine(json);
//                Console.WriteLine("1.Approve Request");
//                Console.WriteLine("2.Reject Request");
//                Console.WriteLine("3. exit");
//                string inputApprove = Console.ReadLine();
//                if (inputApprove == "1")
//                {
//                    ApproveRequest(employee);
//                }
//                else if (inputApprove == "2")
//                {
//                    RejectRequest(employee);
//                }
//                else
//                {
//                    Console.WriteLine("exit");
//                }
//            }
//            else
//            {
//                Console.WriteLine("NO Request Found");
//                Console.WriteLine("press any key TO Exit");
//                Console.ReadLine();
//            }
//        }

//        public static void ApproveRequest(Employee employee)
//        {

//            List<WorkFromHomeRequest> allRequest = new List<WorkFromHomeRequest>();
//            Manager m = new Manager(employee.FirstName, employee.LastName, employee.Email, employee.PhoneNumber, employee.Role);
//            Console.WriteLine("For Approving Request send RequestId");
//            string requestId = Console.ReadLine();

//            allRequest = Serialization.ReadFile<WorkFromHomeRequest>(GlobalFile.fileNameRequest);
//            var request = allRequest.Find(x => x.RequestGUID.ToString() == requestId);

//            if (request == null)
//            {
//                throw new RequestNotFoundException("request not found");
//            }
//            employee.ApproveRequest(request.RequestGUID);

//        }

//        public static void RejectRequest(Employee employee)
//        {
//            List<WorkFromHomeRequest> allRequest = new List<WorkFromHomeRequest>();
//            Manager m = new Manager(employee.FirstName, employee.LastName, employee.Email, employee.PhoneNumber, employee.Role);
//            Console.WriteLine("For Rejecting Request send RequestId");
//            string requestId = Console.ReadLine();


//            allRequest = Serialization.ReadFile<WorkFromHomeRequest>(GlobalFile.fileNameRequest);
//            var request = allRequest.Find(x => x.RequestGUID.ToString() == requestId);
//            if (request == null)
//            {
//                throw new RequestNotFoundException("request not found");
//            }

//            employee.RejectRequest(request.RequestGUID);

//        }
     
//    }
//}
