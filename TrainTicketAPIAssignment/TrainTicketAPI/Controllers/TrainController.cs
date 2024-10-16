using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TrainTicketAPI.Data;
using TrainTicketAPI.Models;
using static TrainTicketAPI.Controllers.TrainController;

namespace TrainTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TrainController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("addtrain")]
        public IActionResult AddTrain([FromBody] TrainData trainData)
        {

            try
            {
                if (trainData == null || trainData.TrainID == null || trainData.SeatNumbers == null || trainData.SeatNumbers.Count == 0)
                {
                    return BadRequest("Invalid train data.");
                }

                var newTrain = new Train
                {
                    TrainID = trainData.TrainID,
                    TrainName = trainData.TrainName,
                    AllSeats = Newtonsoft.Json.JsonConvert.SerializeObject(trainData.SeatNumbers)
                };

                _context.Train.Add(newTrain);
                _context.SaveChanges();

                return Ok("Train added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getalltrains")]
        [Authorize]
        public IActionResult GetAllTrains()
        {
            try
            {
                var trainList = _context.Train.ToList();
                return Ok(trainList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getsingletraindata/{trainid}")]
        public IActionResult GetSingleTrainData(String trainid)
        {
            try
            {
                var train = _context.Train.FirstOrDefault(train => train.TrainID == trainid);

                if (train != null)
                {
                    return Ok(train);
                }
                else
                {
                    return NotFound($"Can't find the train.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("updatetrain")]
        public IActionResult UpdateTrain([FromBody] TrainData trainData)
        {
            try
            {
                var existingTrainData = _context.Train.FirstOrDefault(train => train.TrainID == trainData.TrainID);
                if (existingTrainData == null)
                {
                    return NotFound($"Can't find the train.");
                }

                if (trainData.TrainName != null)
                {
                    existingTrainData.TrainName = trainData.TrainName;
                }

                if (trainData.SeatNumbers != null && trainData.SeatNumbers.Count != 0)
                {
                    existingTrainData.AllSeats = Newtonsoft.Json.JsonConvert.SerializeObject(trainData.SeatNumbers);
                }

                _context.SaveChanges();

                return Ok("Train updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("deletetrain/{trainid}")]
        [Authorize]
        public IActionResult DeleteTrain(String trainid)
        {
            try
            {
                var train = _context.Train.FirstOrDefault(train => train.TrainID == trainid);
                if (train == null)
                {
                    return NotFound($"Can't find the train.");
                }

                _context.Train.Remove(train);
                _context.SaveChanges();

                return Ok("Train deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public class TrainData
        {
            [Required(ErrorMessage = "TrainID is required.")]
            public string TrainID { get; set; }

            public string TrainName { get; set; }

            [Required(ErrorMessage = "Seat list is required.")]
            public List<int> SeatNumbers { get; set; }
        }
    }
}
