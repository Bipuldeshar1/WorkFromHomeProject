


using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.Exceptions;

namespace ClassAndObjectTask1
{
    public class ValidationGuard
    {
        public static DateTime EnsureStartDate(DateTime startDate, DateTime endDate)
        {

            
            if (startDate <= DateTime.Now)
            {
                throw new Exception("Requested From date cannot be the less than or equal to todays date.");
            }

            

            if (startDate > endDate)
            {
                throw new DateTimeException("Requested From date cant be greater than Requested Upto date");
            }
            return startDate;
        }
        public static DateTime EnsureEndDate(DateTime startDate, DateTime endDate)
        {
            
            if (endDate <= DateTime.Now)
            {
                throw new Exception("Requested upto date cannot be the less than or equal to todays date.");
            }
            if (endDate < startDate)
            {

                throw new DateTimeException("Requested upto date should be greater than Requested From date");
            }

            return endDate;
        }
        public static int EnsurePositive(int value, string parameter)
        {
           
            if (value < 0)
            {
                throw new Exception($"{parameter} is negative");
            }
            return value;
        }
        public static string EnsureNotNull(string value, string parameter
            ///[CallerArgumentExpression("value")] string paramName = null
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullException($"{parameter} is null");
            }

            return value;
        }
        public static Guid EnsureGUIDNotNull(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new NullException($"in request Guid is null");
            }
            return value;
        }

        public static string EnsureEmail(string value, string parameter)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullException($"{parameter} is null");
            }
            if (parameter == "email")
            {
                string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
                var resullt = Regex.IsMatch(value, regex);
                if (!resullt)
                {
                    throw new Exception($"{parameter} is invalid");
                }
            }
            return value;
        }

        public static string EnsureNumber(string value, string parameter)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullException($"{parameter} is null");
            }
            //if (value.Length != 10)
            //{
            //    throw new Exception($"{parameter} has to be exactly 10 char");
            //}
            return value;
        }
        public static RoleEnums EnsureRole(RoleEnums role, string parameter)
        {
            if (role.Id < 0 || role.Name == null)
            {
                throw new NullException($"{parameter} is null");
            }
            return role;
        }
        public static long EnsureRequestedBy(long requestedBy, string parameter)
        {
            if (requestedBy < 0)
            {
                throw new NullException($"{parameter} is null");
            }
            return requestedBy;

        }
    }
}
