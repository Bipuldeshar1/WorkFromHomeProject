

using NUnit.Framework.Legacy;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;
using WorkFromHome.Domain.Test.mockRepository;

namespace WorkFromHome.Domain.Test
{
    [TestFixture]
    public class EmployeeRepositoryTest
    {
        private MockEmployeeRepository _mockEmployeeRepository;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeRepository = new MockEmployeeRepository();
        }

        [Test]
        public async Task GetAllAsync_ShouldGetAllEmployeeCount()
        {
            var employee = new Manager("bipul", "deshar", "bipul@gmail.com", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            var result = await _mockEmployeeRepository.GetAllAsync(CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task AddAsync_ShouldAddEmployee()
        {
            var employee = new Manager("bipul", "deshar", "bipul@gmail.com", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            var result = await _mockEmployeeRepository.GetAllAsync(CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));

        }

        [Test]
        public async Task GetSingleEmployeeByIdAsync_ShouldGetSingleEmployeeById()
        {
            var employee = new Manager("bipul", "deshar", "bipul@gmail.com", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            var getSingleEmployee = await _mockEmployeeRepository.GetSingleEmployeeByIdAsync(employee.EmployeeGUID, CancellationToken.None);

            Assert.That(getSingleEmployee, Is.Not.Null);
            Assert.That(getSingleEmployee.FirstName, Is.EqualTo("bipul"));
            Assert.That(getSingleEmployee.Email, Is.EqualTo("bipul@gmail.com"));

        }

        [Test]
        public async Task GetSingleEmployeeByEmailAsync_ShouldGetSingleEmployeeByEmail()
        {
            var employee = new Manager("bipul", "deshar", "bipul@gmail.com", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            var result = await _mockEmployeeRepository.GetSingleEmployeeByEmailAsync(employee.Email, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(employee.Email, Is.EqualTo(result.Email));
            Assert.That(employee.FirstName, Is.EqualTo(result.FirstName));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEmployee()
        {
            var employee = new Manager("John", "Doe", "john.doe@example.com", "1234567890", RoleEnums.Manager, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            employee.UpdateEmployee("bipul", "deshar", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.UpdateAsync(employee, CancellationToken.None);

            var updatedEmployee = await _mockEmployeeRepository.GetSingleEmployeeByIdAsync(employee.EmployeeGUID, CancellationToken.None);




            Assert.That(updatedEmployee.FirstName, Is.EqualTo("bipul"));
            Assert.That(updatedEmployee.LastName, Is.EqualTo("deshar"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveEmployeeAndCountShouldBeZero()
        {
            var employee = new Manager("bipul", "deshar", "bipul@gmail.com", "9876543210", RoleEnums.Employee, "Ktm", "male");
            await _mockEmployeeRepository.AddAsync(employee, "a@12345678", CancellationToken.None);

            await _mockEmployeeRepository.DeleteAsync(employee, CancellationToken.None);

            var deletedEmployee = await _mockEmployeeRepository.GetSingleEmployeeByEmailAsync(employee.Email, CancellationToken.None);
            var emp = await _mockEmployeeRepository.GetAllAsync(CancellationToken.None);



            Assert.That(emp, Is.Not.Null);
            Assert.That(emp.Count(), Is.EqualTo(0));
        }
    }
}
