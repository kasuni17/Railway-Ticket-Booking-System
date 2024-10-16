using TrainTicketAPI.Models;

namespace TrainTicketAPI.Interfaces
{
    public interface ITokenService
    {
        String CreateToken(User user);
    }
}
