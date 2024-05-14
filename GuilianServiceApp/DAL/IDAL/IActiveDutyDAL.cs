using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL.IDAL
{
    public interface IActiveDutyDAL
    {
        public bool SaveActiveDuty(ActiveDuty activeDuty);
        public List<ActiveDuty> GetActiveDutiesByRequester(User requester);
        public Tuple<List<ActiveDuty>, List<ServiceProvided>> GetActiveDutiesAndServicesByProvider(User provider);
        public bool UpdateActiveDutyStatus(ActiveDuty activeDuty);
        public ActiveDuty GetActiveDutyById(int activeDutyId);
    }
}
