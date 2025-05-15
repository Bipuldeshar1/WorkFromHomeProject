using ClassAndObjectTask1;

using System;
using System.Collections.Generic;
using System.Linq;
using WorkFromHome.Domain.baseModel;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.Exceptions;

namespace WorkFromHome.Domain.models
{
    public abstract class Employee : BaseEntity
    {

        private List<Request> _requests = new List<Request>();
        public Guid EmployeeGUID { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Email { get; protected set; }
        public string PhoneNumber { get; protected set; }

        public string Address { get; protected set; }
        public string Gender { get; protected set; }

        public RoleEnums Role { get; protected set; }
        public virtual IReadOnlyCollection<Request> Requests => _requests;
        public bool IsDeleted { get; protected set; }
        public AppUser AppUser { get; protected set; }
        public string UserId { get; protected set; }

        public Employee(string firstName, string lastName, string email, string phoneNumber, RoleEnums role,string address,string gender)
        {

            FirstName = ValidationGuard.EnsureNotNull(firstName, nameof(firstName));
            LastName = ValidationGuard.EnsureNotNull(lastName, nameof(lastName));
            Email = ValidationGuard.EnsureEmail(email, nameof(email));
            PhoneNumber = ValidationGuard.EnsureNumber(phoneNumber, nameof(phoneNumber));
            Address=ValidationGuard.EnsureNotNull(address,nameof(address));
            Gender=ValidationGuard.EnsureNotNull(gender, nameof(gender));
            Role = ValidationGuard.EnsureRole(role,nameof(role));
            IsDeleted = false;
            EmployeeGUID = Guid.NewGuid();

        }

        public void UpdateEmployee(string firstName, string lastName, string phoneNumber, RoleEnums role,string address,string gender)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Role = role;
            UpdatedOn = DateTime.UtcNow;
            Address = address;
            Gender = gender;
        }

        //public virtual string GetEmployeeDetails()
        //{
        //    return $"Employee Detail employeeId {Id} name {FirstName} {LastName} email {Email}  phoneNumber {PhoneNumber}";
        //}

        public virtual void AddEmployee(Employee employee, int managerId, string userId)
        {

            employee.AddedBy = managerId;
            employee.AddedOn = DateTime.UtcNow;
            UserId = userId;
        }

        public virtual void CreateWorkFromHomeRequest(Request request)
        {
            _requests.Add(request);
        }

        public virtual void SubmitWorkFromHomeRequest(Request request)
        {

            request.Submit(request);
        }

        public virtual void ApproveRequest(Request request)
        {

            request.Approve(request);
        }
        public virtual void RejectRequest(Request request)
        {

            request.Reject(request);
        }
        public void UpdateIsDeleted()
        {
            bool deleted = true;
            IsDeleted = deleted;
        }


    }
}
