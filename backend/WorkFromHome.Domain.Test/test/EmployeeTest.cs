
using NUnit.Framework.Legacy;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.Exceptions;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Test
{
    public class EmployeeTest
    {
        private Manager _manager;
        private WorkFromHomeRequest _request;



        [SetUp]
        public void Setup()
        {

            _manager = new Manager("a", "b", "a@gmail.com", "1234567890", RoleEnums.Manager, "Ktm", "male");
            _request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), Guid.NewGuid(), 123,"xyz");

        }


        
        [Test]
        public void EmployeeIsInitShouldSetValuesToField()
        {


            Assert.That(_manager.FirstName, Is.Not.Null);
            Assert.That(_manager.LastName, Is.Not.Null);
            Assert.That(_manager.Email, Is.Not.Null);
            Assert.That(_manager.PhoneNumber, Is.Not.Null);
            Assert.That(_manager.Role, Is.Not.Null);

        }


        //createWFHRequest
        [Test]
        public void WFHRequestCreateRequestListCountIsEqualToOne()
        {

            _manager.CreateWorkFromHomeRequest(_request);
            
            Assert.That(_manager.Requests.Count, Is.EqualTo(1));

        }

        [Test]
        public void UpdateEmployee_ShouldUpdateEmployeeDetails()
        {
            _manager.UpdateEmployee("bipul", "deshar", "1234567890", RoleEnums.Manager, "Ktm", "male");

    
            Assert.That(_manager.FirstName, Is.EqualTo("bipul"));
            Assert.That(_manager.LastName, Is.EqualTo("deshar"));
        }

        [Test]
        public void SubmitWorkFromHomeRequest_RequestIsNOtEmpty()
        {
            _manager.SubmitWorkFromHomeRequest(_request);
        }



        [Test]
        public void UpdateIsDeleted_ShouldSetIsDeletedToTrue()
        {
            _manager.UpdateIsDeleted();
            
            Assert.That(_manager.IsDeleted);
        }




    }
}
