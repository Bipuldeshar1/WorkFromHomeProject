
using System;
using WorkFromHome.Domain.Enummerations;

namespace WorkFromHome.Domain.models
{
    public class Manager : Employee
    {
       
        public Manager( string firstName, string lastName, string email, string phoneNumber, RoleEnums role,string address,string gender) : base(firstName, lastName, email, phoneNumber, role,address,gender)
        {
        }

     
    }
}