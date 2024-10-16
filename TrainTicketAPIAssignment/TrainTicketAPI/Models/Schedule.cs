using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainTicketAPI.Models
{
    public class Schedule
    {
        public object[] ScheduleID;
        public object PricePerPerson;

        [Key]
        public long ID { get; set; }

        public DateTime Date { get; set; }

        public string StartStation { get; set; }

        public string DestinationStation { get; set; }

        public string TrainID { get; set; }

        public decimal PricePerUnit { get; set; }

        public string AvailableSeats { get; set; }


        public Schedule()
        {
            AvailableSeats = "[]";
        }
    }
}
