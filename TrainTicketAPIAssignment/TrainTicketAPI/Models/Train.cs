namespace TrainTicketAPI.Models
{
    public class Train
    {
        public int Id { get; set; }

        public string TrainID { get; set; }

        public string TrainName { get; set; }

        public string AllSeats { get; set; }

        public Train()
        {
            AllSeats = "[]";
        }
    }
}
