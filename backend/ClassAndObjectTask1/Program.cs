using WorkFromHome.Domain.Enummerations;

try
{
    Console.WriteLine("Welcome to the Work From Home System");

    Console.WriteLine("1. Login");
    Console.WriteLine("2. Exit");
    string choice = Console.ReadLine();

    //if (choice == "1")
    //{
    //    Employee loggedInUser = ConsoleFxns.Login();

    //    if (loggedInUser.Role == RoleEnums.Employee)
    //    {
    //        userLoginEmployee(loggedInUser);

    //    }
    //    else if (loggedInUser.Role == RoleEnums.)
    //    {
    //        userLoginManager(loggedInUser);
    //    }
    //}
    //else
    //{
    //    Console.WriteLine("exiting");
    //}
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

void userLoginEmployee(Employee loggedInUser)
{
    EmployeeFxns(loggedInUser);
}
void userLoginManager(Employee loggedInUser)
{
    managerFxns(loggedInUser);
}

void managerFxns(Employee loggedInUser)
{
    Console.WriteLine($"logged in as {loggedInUser.Role} {loggedInUser.FirstName} ");

    Console.WriteLine("1. see all request");
    Console.WriteLine("2. Apply Work From Home");
    Console.WriteLine("3. Add employee");
    Console.WriteLine("4. Check application status");
    Console.WriteLine("5. exit");

    var input = Console.ReadLine();

 //   if (input == "1")
    //{
    //    ConsoleFxns.ShowAllRequest(loggedInUser);
    //}
    //else if (input == "2")
    //{
    //    ConsoleFxns.ApplyWorkFromHomeRequest(loggedInUser);
    //}
    //else if (input == "3")
    //{
    //    ConsoleFxns.AddEmployee();
    //}
    //else if (input == "4")
    //{
    //    ConsoleFxns.ShowUserWorkFromHomeRequestStatus(loggedInUser);
    //}
    //else
    //{
    //    Console.WriteLine("exiting");
    //}
}

void EmployeeFxns(Employee loggedInUser)
{
    Console.WriteLine($"logged in as {loggedInUser.Role} {loggedInUser.FirstName} ");

    Console.WriteLine("1. Apply Work From Home");
    Console.WriteLine("2. Check status");
    Console.WriteLine("3. exit");
    string selected = Console.ReadLine();
 //   if (selected == "1")
 //   {
      //  ConsoleFxns.ApplyWorkFromHomeRequest(loggedInUser);
    //}
    //if (selected == "2")
    //{
    //    ConsoleFxns.ShowUserWorkFromHomeRequestStatus(loggedInUser);
    //}
    //if (selected == "3")
    //{
    //    Console.WriteLine("exit");
    //}
}