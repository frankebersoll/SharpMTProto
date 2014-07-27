using System.Threading.Tasks;

namespace SharpMTProto.Client
{
    public interface IPersistance
    {
        Task Save(PersistanceInfo persistanceInfo);

        Task<PersistanceInfo> Load();

        Task Clear();
    }
}