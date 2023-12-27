using practikum;
using Bogus;
using Bogus.DataSets;
using System.Reflection.Emit;
using Xunit;
using Newtonsoft.Json;
using Serilog;
//using Allure.Xunit;
//using Allure.Xunit.Attributes;
namespace TestProject2
{
    public class UnitTest1
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly Faker _faker;

        public UnitTest1()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:3000/");

            _logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            _faker = new Faker();

        }



        [Fact]
        public void CreateUser()
        {
            var generator = new Faker<TestUser>()
            .StrictMode(true)
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.ID, f => f.Random.Int(0, 500))
            .RuleFor(x => x.Surname, f => f.Person.UserName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Age, f => f.Random.Int(15, 50));

            var testUser = generator.Generate();

            Console.WriteLine(testUser.Name);
            Console.WriteLine(testUser.ID); 
            Console.WriteLine(testUser.Surname);
            Console.WriteLine(testUser.Age);
            Console.WriteLine(testUser.LastName);
        }
        //[Fact]
        //public void ReadUser()
        //{
        //    var generator = new Faker<TestUser>()
        //    .StrictMode(true)
        //    .RuleFor(x => x.Name, f => f.)
        //    .RuleFor(x => x.ID, f => f.Random.Int(0, 500))
        //    .RuleFor(x => x.Surname, f => f.Person.UserName)
        //    .RuleFor(x => x.LastName, f => f.Person.LastName)
        //    .RuleFor(x => x.Age, f => f.Random.Int(15, 50));

        //    var testUser = generator.Generate();

        //    Console.WriteLine(testUser.Name);
        //    Console.WriteLine(testUser.ID);
        //    Console.WriteLine(testUser.Surname);
        //    Console.WriteLine(testUser.Age);
        //    Console.WriteLine(testUser.LastName);
        //}


        public async Task Test_Get_User()
        {
            var response = await _client.GetAsync("comments/1");

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadAsStringAsync();
            var retrievedUser = JsonConvert.DeserializeObject<TestUser>(user);

            Assert.NotNull(retrievedUser);
        }


    }
    public class TestUser
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}