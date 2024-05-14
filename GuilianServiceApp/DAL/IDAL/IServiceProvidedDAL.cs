using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL.IDAL
{
    public interface IServiceProvidedDAL
    {
        public bool SaveServiceProvided(ServiceProvided serviceProvided);
        public List<ServiceProvided> GetAllServiceProvided();
        public List<ServiceProvided> GetAllServiceProvidedByProvider(User currentProvider);
        public bool UpdateServiceProvidedStatus(ServiceProvided serviceProvided);
        public ServiceProvided GetServiceProvidedById(int serviceProvidedId);
    }
}
