using System;
using Xunit;
using LawLab.Models;
using LawLab.Repository;
using LawLab.Controllers;
using Moq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LawLab.Components;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using LawLab.Hubs;
using Microsoft.AspNetCore.SignalR;
using LawLab.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using System.Data;

namespace LawLab.Tests
{
    public class StudentTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GetStudents() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Student[] GetStudents() => new Student[]
        {
            new Student
            {
                FirstName = "Имя студента 1",
                FamilyName = "Фамилия студента 1",
                UniversityYear = 3,
                UniversityName = "Белорусский государственный университет",
                FacultyName = "Юридический факультет",
                Summary = "Студент, увлекающийся семейным правом",
                Email = "student@mail.by",
                Password = "suchsecretpassword12345"
            },
            new Student
            {
                FirstName = "Имя студента 2",
                FamilyName = "Фамилия студента 2",
                UniversityYear = 4,
                UniversityName = "Белорусский государственный экономический университет",
                FacultyName = "Юридический факультет",
                Summary = "Студент, занимающийся наследственным правом",
                Email = "studentik@mail.by",
                Password = "suchsecretpassword12345"
            },
            new Student
            {
                FirstName = "Имя студента 3",
                FamilyName = "Фамилия студента 3",
                UniversityYear = 5,
                UniversityName = "Белорусский государственный университет",
                FacultyName = "Юридический факультет",
                Summary = "Студент, увлекающийся семейным правом",
                Email = "stut@mail.by",
                Password = "suchsecretpassword12345"
            },
            new Student
            {
                FirstName = "Имя студента 4",
                FamilyName = "Фамилия студента 4",
                UniversityYear = 1,
                UniversityName = "Белорусский государственный экономический университет",
                FacultyName = "Юридический факультет",
                Summary = "Студент, занимающийся наследственным правом",
                Email = "studen@mail.by",
                Password = "suchsecretpassword12345"
            }
        };
    }

    public class CommonTests
    {
        public IHost host { get; set; }
        public CommonTests()
        {
            host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    env.ContentRootPath = Directory.GetCurrentDirectory();
                    env.EnvironmentName = "Development";
                });

                builder.UseStartup<Startup>();
            }).Build();
        }

        [Fact]
        public void DisplayCurrentUser()
        {
            // Arrange
            var dbContext = host.Services.GetService<AppDbContext>();
            var userManager = host.Services.GetService<UserManager<AppUser>>();
            var roleManager = host.Services.GetService<RoleManager<IdentityRole>>();
            var mockIUserValidator = host.Services.GetService<IUserValidator<AppUser>>();
            var mockIPasswordValidator = host.Services.GetService<IPasswordValidator<AppUser>>();
            var mockIPasswordHasher = host.Services.GetService<IPasswordHasher<AppUser>>();

            var controller = new FormController(dbContext,
            userManager,
            roleManager,
            mockIUserValidator,
            mockIPasswordValidator,
            mockIPasswordHasher);

            // Act
            var model = (controller.Invoke("Иванов Иван") as ViewViewComponentResult)?.ViewData.Model as string;

            // Assert
            Assert.Equal("Иванов Иван", model);

        }

        [Fact]
        public async Task CanSetName()
        {
            // Arrange
            var dbContext = host.Services.GetService<AppDbContext>();
            var userManager = host.Services.GetService<UserManager<AppUser>>();
            var roleManager = host.Services.GetService<RoleManager<IdentityRole>>();
            var signalRContext = host.Services.GetService<IConnectionManager>();
            var hub = new ChatHub(dbContext, userManager, roleManager);
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClientContext.Setup(m => m.Items).Returns(new Dictionary<object, object>());
            hub.Context = mockClientContext.Object;

            // Act
            await hub.SetName("Ivan");

            // Assert
            Assert.Equal("Ivan", hub.GetName().Result);
        }

        [Fact]
        public void DislplayAnonimousIfNameWasNotSelected()
        {
            // Arrange
            var dbContext = host.Services.GetService<AppDbContext>();
            var userManager = host.Services.GetService<UserManager<AppUser>>();
            var roleManager = host.Services.GetService<RoleManager<IdentityRole>>();
            var signalRContext = host.Services.GetService<IConnectionManager>();
            var hub = new ChatHub(dbContext, userManager, roleManager);
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClientContext.Setup(m => m.Items).Returns(new Dictionary<object, object>());
            hub.Context = mockClientContext.Object;

            // Act
            // Do Nothing

            // Assert
            Assert.Equal("Имя не установлено", hub.GetName().Result);
        }

        [Fact]
        public void Retrive4BestStudents()
        {
            // Arrange
            var context = host.Services.GetService<AppDbContext>();
            var component = new BestStudentsViewComponent(context);

            // Act
            var viewComponentResult = component.Invoke() as ViewViewComponentResult;
            Student[] bestStudents = ((IEnumerable<Student>)viewComponentResult.ViewData.Model).ToArray();

            // Assert
            Assert.Equal(4, bestStudents.Length);
        }

        [Fact]
        public void CanDoDifferentOperationUsingRepositoryPattern()
        {
            // Arrange
            var repositoryStudent = host.Services.GetService<IGenericRepository<Student>>();
            var repositoryClient = host.Services.GetService<IGenericRepository<Client>>();

            // Act
            Student student = repositoryStudent.Get(2);
            Client client = repositoryClient.Get(2);

            // Assert
            Assert.Equal(student.StudentId, client.ClientId);
        }

        [Fact]
        public void IndexActionOfChatControllerHasNoModel()
        {
            // Arrange
            var dbContext = host.Services.GetService<AppDbContext>();
            var userManager = host.Services.GetService<UserManager<AppUser>>();
            var roleManager = host.Services.GetService<RoleManager<IdentityRole>>();
            var controller = new ChatController(dbContext, userManager, roleManager);

            // Act
            var isNull = (controller.Index() as ViewResult).ViewData?.Model ?? null;

            // Assert
            Assert.Null(isNull);
        }
    }
}
