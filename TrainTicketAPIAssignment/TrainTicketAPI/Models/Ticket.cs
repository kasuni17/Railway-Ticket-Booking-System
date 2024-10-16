using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainTicketAPI.Models
{
    public class Ticket
    {
        [Key]
        public long ID { get; set; }

        public String NIC { get; set; }

        [ForeignKey("Train")]
        public decimal TrainID { get; set; }

        public string ReservedSeats { get; set; }

        public decimal TicketAmount { get; set; }

        public DateOnly Date { get; set; }

        public Ticket()
        {
            ReservedSeats = "[]";
        }
    }
}
