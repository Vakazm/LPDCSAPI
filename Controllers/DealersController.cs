using LPDCSAPI.Classes;
using Microsoft.AspNetCore.Mvc;

namespace LPDCSAPI.Controllers
{
    // API controller, responsible for most of the program's logic
    [ApiController]
    [Route ("api/[controller]")]
    public class DealersController : ControllerBase
    {
        // Add car to specific dealer's collection
        [HttpPost ("AddCar")]
        public IActionResult AddCar ([FromHeader] string token, [FromBody] Car car)
        {
            if (!ModelState.IsValid) { return BadRequest (ModelState); }  // Check if car info entered correctly

            // Try to get dealer by context
            Dealer? dealer = Context.FindDealer (token);
            if (dealer == null) { return BadRequest (new { Message = "Couldn't find any dealer by given token" }); }

            // Dealer found, add car to their collection
            car.ID = dealer.Cars.Count == 0 ? 0 : dealer.Cars.Last ().ID + 1;
            dealer.Cars.Add (car);
            return Ok (new { Message = $"Car was successfully added to collection with id {car.ID}" });
        }

        // Remove car from collection of specific dealer
        [HttpPost ("RemoveCar/{id}")]
        public IActionResult RemoveCar ([FromHeader] string token, int id)
        {
            // Try to get dealer by context
            Dealer? dealer = Context.FindDealer (token);
            if (dealer == null) { return BadRequest (new { Message = "Couldn't find any dealer by given token" }); }

            // Dealer found, trying to remove car from their collection
            if (dealer.Cars.RemoveAll (c => c.ID == id) > 0)
            {
                // Car found, remove from collection
                return Ok (new { Message = $"Car with id {id} was successfully removed from collection" });
            }
            else { return Ok (new { Message = $"There is no car with id {id} in collection" }); }  // Car not found
        }

        // Update car in specific dealer's collection
        [HttpPost ("UpdateCar/{id}")]
        public IActionResult UpdateCar ([FromHeader] string token, int id, [FromBody] Car car)
        {
            if (!ModelState.IsValid) { return BadRequest (ModelState); }  // Check if car info entered correctly

            // Try to get dealer by context
            Dealer? dealer = Context.FindDealer (token);
            if (dealer == null) { return BadRequest (new { Message = "Couldn't find any dealer by given token" }); }

            // Dealer found, search car in their collection
            car.ID = id;
            for (int i = 0; i < dealer.Cars.Count; i ++)
            {
                // Car found, update details
                if (dealer.Cars [i].ID == id)
                {
                    dealer.Cars [i] = car;
                    return Ok (new { Message = $"Car with id {id} was successfully updated" });
                }
            }
            return Ok (new { Message = $"There is no car with id {id} in collection" });  // Car not found
        }

        // Find cars in specific dealer's collection
        [HttpPost ("GetCars")]
        public IActionResult GetCars ([FromHeader] string token, string? make, string? model, int? year)
        {
            // Try to get dealer by context
            Dealer? dealer = Context.FindDealer (token);
            if (dealer == null) { return BadRequest (new { Message = "Couldn't find any dealer by given token" }); }

            // Dealer found, search cars in their collection
            var query = from car in dealer.Cars where
                (string.IsNullOrEmpty (make) || car.Make == make) &&
                (string.IsNullOrEmpty (model) || car.Model == model) &&
                (year == null || car.Year == year)
            select new { car.ID, car };
            /*var query = dealer.Cars.Where (car =>
                (string.IsNullOrEmpty (make) || car.Make == make) &&
                (string.IsNullOrEmpty (model) || car.Model == model) &&
                (year == null || car.Year == year));*/
            return Ok (new { Cars = query.ToArray () });  // Return all cars matching the query
        }
    }
}
