using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using TrainTicketAPI.Data;
using TrainTicketAPI.Models;

namespace TrainTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
         
        public ScheduleController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("findtrains")]
        public IActionResult SearchTrains([FromQuery] string starStation, [FromQuery] string destinationStation, [FromQuery] DateTime date)
        {
            try
            {
                string start = starStation.ToLower();
                string destination = destinationStation.ToLower();

                var schedules = _context.Schedule
                    .Where(Schedule => Schedule.StartStation.ToLower() == start && Schedule.DestinationStation.ToLower() == destination && Schedule.Date.Date == date.Date)
                    .Select(schedule => new { schedule.ID, schedule.TrainID, schedule.PricePerUnit })
                    .ToList();


                if (schedules.Any())
                {
                    return Ok(schedules);
                }
                else
                {
                    return NotFound("No trains found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("findavailableseats")]
        public IActionResult GetAvailableSeats([FromQuery] long ScheduleID)
        {
            try
            {
                var schedule = _context.Schedule
                    .Where(schedule => schedule.ID == ScheduleID)
                    .ToList();

                if (schedule.Any())
                {
                    var allAvailableSeats = schedule
                        .SelectMany(schedule => Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(schedule.AvailableSeats))
                        .ToList();

                    if(allAvailableSeats.Count > 0)
                    {
                        return Ok(allAvailableSeats);
                    }
                    else
                    {
                        return NotFound("No available seats for this schedule.");
                    }
                    
                }
                else
                {
                    return NotFound("Invalid schedule ID or train ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("addschedule")]
        public IActionResult AddSchedule([FromBody] ScheduleData scheduleData)
        {
            try
            {
                if (scheduleData == null || scheduleData.Date == null || scheduleData.StartStation == null || scheduleData.DestinationStation == null || scheduleData.TrainID == null || scheduleData.PricePerPerson == null || scheduleData.AvailableSeats == null)
                {
                    return BadRequest("All fields are required.");
                }

                var newSchedule = new Schedule
                {
                    Date = scheduleData.Date,
                    StartStation = scheduleData.StartStation,
                    DestinationStation = scheduleData.DestinationStation,
                    TrainID = scheduleData.TrainID,
                    PricePerUnit = scheduleData.PricePerPerson,
                    AvailableSeats = Newtonsoft.Json.JsonConvert.SerializeObject(scheduleData.AvailableSeats)
                };

                _context.Schedule.Add(newSchedule);
                _context.SaveChanges();

                return Ok("Schedule scheduled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getallschedule")]
        public IActionResult GetAllSchedules()
        {
            try
            {
                var schedules = _context.Schedule.ToList();
                if(schedules.Any())
                {
                    var response = new StringBuilder();
                    foreach (var schedule in schedules)
                    {
                        response.AppendLine($"Schedule ID: {schedule.ID}");
                        response.AppendLine($"Starting station: {schedule.StartStation}");
                        response.AppendLine($"Destination station: {schedule.DestinationStation}");
                        response.AppendLine($"TrainID: {schedule.TrainID}");
                        response.AppendLine($"Price Per Person: {schedule.PricePerUnit}");
                    }

                    return Ok(response.ToString());
                }else
                {
                    return BadRequest("Couldn't found any schedule.");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getsingleschedule/{id}")]
        public IActionResult GetSingleSchedule(long id)
        {
            try
            {
                var schedule = _context.Schedule.Find(id);
                if (schedule != null)
                {
                    return Ok(schedule);
                }
                else
                {
                    return NotFound($"Couldn't find the schedule for this ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("updateschedule/{id}")]
        [Authorize]
        public IActionResult UpdateSchedule(long id, [FromBody] ScheduleData scheduleData)
        {
            try
            {
                var existingSchedule = _context.Schedule.Find(id);
                if (existingSchedule == null)
                {
                    return NotFound($"Couldn't find the schedule for this ID.");
                }

                if (scheduleData.Date != null)
                {
                    existingSchedule.Date = scheduleData.Date;
                }

                if (scheduleData.StartStation != null)
                {
                    existingSchedule.StartStation = scheduleData.StartStation;
                }

                if (scheduleData.DestinationStation != null)
                {
                    existingSchedule.DestinationStation = scheduleData.DestinationStation;
                }

                if (scheduleData.PricePerPerson != null)
                {
                    existingSchedule.PricePerUnit = scheduleData.PricePerPerson;
                }

                _context.SaveChanges();

                return Ok("Schedule updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("deleteschedule/{id}")]
        [Authorize]
        public IActionResult DeleteSchedule(long id)
        {
            try
            {
                var schedule = _context.Schedule.Find(id);
                if (schedule == null)
                {
                    return NotFound($"Couldn't find the schedule for this ID.");
                }

                _context.Schedule.Remove(schedule);
                _context.SaveChanges();

                return Ok("Schedule deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public class ScheduleData
        {
            public DateTime Date { get; set; }

            public string StartStation { get; set; }

            public string DestinationStation { get; set; }

            public string TrainID { get; set; }

            public decimal PricePerPerson { get; set; }

            public List<int> AvailableSeats { get; set; }
        }

    }
}
