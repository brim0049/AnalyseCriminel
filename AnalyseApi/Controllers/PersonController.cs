using AnalyseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AnalyseApi.Controllers
{

    
    [Route("api/Person")]
    [ApiController]
    public class PersonController : Controller
    {
        private static AnalyseDbContext _db = new AnalyseDbContext();

        //Person

        [HttpPost]
        [Route("Person")]

        public static void AddPerson(Models.Person person)
        {
            _db.Persons.Add(new Models.Person { NIN = person.NIN, FirstName = person.FirstName, LastName = person.LastName, Phone = person.Phone, CriminalRecord = person.CriminalRecord });
            _db.SaveChanges();
        }
        [HttpPatch]
        [Route("Person")]
        public static void UpdatePerson(Models.Person person, string _NIN)
        {
            Models.Person client = _db.Persons.SingleOrDefault(user => (user.NIN == _NIN));
            client.FirstName = person.FirstName is null ? client.FirstName : person.FirstName;
            client.LastName = person.LastName is null ? client.LastName : person.LastName;
            client.Phone = person.Phone is null ? client.Phone : person.Phone;
            Models.Person clientAdd = _db.Persons.Include(User => User.Address).Where(user => (user.NIN == _NIN)).First();
            clientAdd.Address.NoStreet = person.Address.NoStreet is null ? clientAdd.Address.NoStreet : person.Address.NoStreet;
            clientAdd.Address.NameStreet = person.Address.NameStreet is null ? clientAdd.Address.NameStreet : person.Address.NameStreet;
            clientAdd.Address.Ville = person.Address.Ville is null ? clientAdd.Address.Ville : person.Address.Ville;
            _db.SaveChanges();
        }
        [HttpDelete]
        [Route("Person")]
        public static void DeletePerson(string _NIN)
        {
            Models.Person clientCalls = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == _NIN)).First();
            _db.Calls.RemoveRange(clientCalls.Calls);
            Models.Person clientCars = _db.Persons.Include(User => User.Cars).Where(user => (user.NIN == _NIN)).First();
            _db.Cars.RemoveRange(clientCars.Cars);
            Models.Person client = _db.Persons.SingleOrDefault(user => (user.NIN == _NIN));
            _db.Persons.Remove(client);
            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Person")]
        public static List<Person> GetClients()
        {

            List<Models.Person> client = _db.Persons.ToList();
            return client;

        }
        [HttpGet]
        [Route("Person")]
        public static List<Person> GetClientsWithAddress()
        {

            var client = _db.Persons.Include(User => User.Address).ToList();
            return client;

        }
        [HttpGet]
        [Route("Person")]
        public static Person GetClient(string nin)
        {

            Models.Person client = _db.Persons.SingleOrDefault(x => x.NIN.Equals(nin));
            return client;

        }

        //Address

        [HttpPost]
        [Route("Address")]

        public static void AddAddress(Models.Address address)
        {
            _db.Addresses.Add(new Models.Address { NoStreet = address.NoStreet, NameStreet = address.NameStreet, Ville = address.Ville });
            _db.SaveChanges();
        }
        [HttpPatch]
        [Route("Address")]
        public static void UpdatePersonAddress(Models.Address address, string _NIN)
        {

            Models.Person client = _db.Persons.Include(User => User.Address).Where(user => (user.NIN == _NIN)).First();
            client.Address = address;

            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Address")]
        public static Person GetClientAddress(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Address).Where(user => (user.NIN == nin)).First();

            return client;

        }

        //Call

        [HttpPost]
        [Route("Call")]

        public static void AddCall(Models.Call call)
        {
            _db.Calls.Add(new Models.Call { Number = call.Number, Date = call.Date });
            _db.SaveChanges();
        }

        [HttpPatch]
        [Route("Call")]
        public static void UpdatePersonCall(Models.Call call, string _NIN)
        {

            Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == _NIN)).First();
            client.Calls.Add(call);

            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Call")]
        public static List<Call> GetClientCalls(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == nin)).First();
            List<Call> calls = client.Calls.ToList();

            return calls;

        }
        [HttpDelete]
        [Route("Call")]
        public static void DeleteCall( int id)
        {
            Models.Call call = _db.Calls.Where(user => (user.CallId == id)).First();
            _db.Calls.Remove(call);
            _db.SaveChanges();
        }

      
        //Event

        [HttpPatch]
        [Route("Event")]
        public static void UpdatePersonEvent(Models.Event evenement, string _NIN)
        {

            Models.Person client = _db.Persons.Include(User => User.Event).Where(user => (user.NIN == _NIN)).First();
            client.Event =evenement;
            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Event")]
        public static Person GetClientEvent(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Event).Where(user => (user.NIN == nin)).First();

            return client;

        }
        [HttpGet]
        [Route("Event")]
        public static Event GetClientEventAddress(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Event).Where(user => (user.NIN == nin)).First();
            Models.Event address = _db.Events.Include(User => User.Address).Where(user => (user.EventId == client.Event.EventId)).First();


            return address;

        }

        //Car

        [HttpPatch]
        [Route("Car")]
        public static void UpdatePersonCar(Models.Car car, string _NIN)
        {

            Models.Person client = _db.Persons.Include(User => User.Cars).Where(user => (user.NIN == _NIN)).First();
            client.Cars.Add(car);

            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Car")]
        public static List<Car> GetClientCars(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Cars).Where(user => (user.NIN == nin)).First();
            List<Car> cars = client.Cars.ToList();

            return cars;

        }
        [HttpDelete]
        [Route("Car")]
        public static void DeleteCar(int id)
        {
            Models.Car car = _db.Cars.Where(user => (user.CarId == id)).First();
            _db.Cars.Remove(car);
            _db.SaveChanges();
        }

        //Relationship

        [HttpPatch]
        [Route("Relationship")]
        public static void UpdatePersonRelation(Models.Relationship relation, string _NIN)
        {

            Models.Person client = _db.Persons.Include(User => User.Relations).Where(user => (user.NIN == _NIN)).First();
            client.Relations.Add(relation);

            _db.SaveChanges();
        }
        [HttpGet]
        [Route("Relationship")]
        public static List<Relationship> GetClientRelation(string nin)
        {
            Models.Person client = _db.Persons.Include(User => User.Relations).Where(user => (user.NIN == nin)).First();
            List<Relationship> relations = client.Relations.ToList();

            return relations;

        }
    }
}
