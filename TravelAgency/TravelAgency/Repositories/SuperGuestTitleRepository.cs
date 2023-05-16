using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class SuperGuestTitleRepository : ISuperGuestTitleRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuestTitles.csv";
        private readonly Serializer<SuperGuestTitle> serializer;
        private List<SuperGuestTitle> titles;

        public SuperGuestTitleRepository()
        {
            serializer = new Serializer<SuperGuestTitle>();
            titles = serializer.FromCSV(FilePath);
        }

        public List<SuperGuestTitle> GetAll()
        {
            return titles;
        }

        public SuperGuestTitle GetActiveByGuest(User guest)
        {
            foreach (SuperGuestTitle title in titles)
            {
                if (title.GuestId == guest.Id && title.IsActive())
                {
                    return title;
                }
            }
            return null;
        }

        public int NextId()
        {
            if (titles.Count < 1)
            {
                return 1;
            }
            return titles.Max(c => c.Id) + 1;
        }

        public SuperGuestTitle Save(SuperGuestTitle title)
        {
            title.Id = NextId();
            titles.Add(title);
            serializer.ToCSV(FilePath, titles);
            return title;
        }

        public void SaveAll()
        {
            serializer.ToCSV(FilePath, titles);
        }
    }
}
