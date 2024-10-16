using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TrainTicketAPI.Data;
using TrainTicketAPI.Models;

namespace TrainTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TicketController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("bookticket")]
        public IActionResult BookTicket([FromBody] TicketBookingData request)
        {
            try
            {
                if (request.NIC == null || string.IsNullOrWhiteSpace(request.NIC))
                {
                    return BadRequest("NIC is required.");
                }

                if (request.ReservedSeats.Count > 5)
                {
                    return BadRequest("Maximum 5 seats can be reserved per ticket.");
                }

                var validSchedule = _context.Schedule.Any(schedule => schedule.ID == request.ScheduleID);
                if (!validSchedule)
                {
                    return BadRequest("Invalid Schedule.");
                }

                var schedule = _context.Schedule.FirstOrDefault(schedule => schedule.ID == request.ScheduleID);

                var availableSeats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(schedule.AvailableSeats);

                foreach (var seat in request.ReservedSeats)
                {
                    if (!availableSeats.Contains(seat))
                    {
                        return BadRequest($"Seat {seat} is not available.");
                    }
                }

                foreach (var seat in request.ReservedSeats)
                {
                    availableSeats.Remove(seat);
                }

                schedule.AvailableSeats = Newtonsoft.Json.JsonConvert.SerializeObject(availableSeats);

                decimal ticketAmount = request.ReservedSeats.Count * schedule.PricePerUnit;

                string reservedSeatsJson = JsonConvert.SerializeObject(request.ReservedSeats);

                var ticket = new Ticket
                {
                    NIC = request.NIC,
                    ReservedSeats = reservedSeatsJson,
                    TicketAmount = ticketAmount,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                _context.Ticket.Add(ticket);
                _context.SaveChanges();

                var response = new
                {
                    TicketID = ticket.ID,
                    TicketAmount = ticket.TicketAmount,
                    ScheduleID = schedule.ID,
                    ReservedSeats = ticket.ReservedSeats
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getallticket")]
        [Authorize]
        public IActionResult GetAllTickets()
        {
            try
            {
                var tickets = _context.Ticket.ToList();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getsingleticket/{id}")]
        [Authorize]
        public IActionResult GetSingleTicket(long id)
        {
            try
            {
                var tickets = _context.Ticket.Find(id);
                if (tickets != null)
                {
                    return Ok(tickets);
                }
                else
                {
                    return NotFound($"Couldn't find a ticket for this ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("deleteticket/{id}")]
        [Authorize]
        public IActionResult DeleteTicket(long id)
        {
            try
            {
                var ticket = _context.Ticket.Find(id);
                if (ticket == null)
                {
                    return NotFound($"Couldn't find a ticket for this ID.");
                }

                _context.Ticket.Remove(ticket);
                _context.SaveChanges();

                return Ok("Ticket deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


        public class TicketBookingData
        {
            [Required(ErrorMessage = "Schedule ID is required.")]
            public long ScheduleID { get; set; }

            [Required(ErrorMessage = "NIC is required.")]
            public string NIC { get; set; }

            [Required(ErrorMessage = "ReservedSeats list is required.")]
            public List<long> ReservedSeats { get; set; }
        }
    }
}
