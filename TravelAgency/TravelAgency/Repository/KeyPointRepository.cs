using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class KeyPointRepository : IKeyPointRepository
    {
        private const string FilePath = "../../../Resources/Data/keyPoints.csv";
        private readonly Serializer<KeyPoint> _serializer;
        private List<KeyPoint> keyPoints;

        public KeyPointRepository(TourOccurrenceRepository tourOccurrenceRepository)
        {
            _serializer = new Serializer<KeyPoint>();
            keyPoints = _serializer.FromCSV(FilePath);
            LinkKeyPoints(tourOccurrenceRepository);
        }

        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoint>();
            keyPoints = _serializer.FromCSV(FilePath);
        }

        private void LinkKeyPoints(TourOccurrenceRepository tourOccurrenceRepository)
        {
            foreach (KeyPoint keyPoint in keyPoints)
            {
                TourOccurrence tourOccurrence = tourOccurrenceRepository.GetAll().Find(tO => tO.Id == keyPoint.TourOccurrenceId);
                if (tourOccurrence != null)
                {
                    tourOccurrence.KeyPoints.Add(keyPoint);
                }
            }
        }
        public int NextId()
        {
            if (keyPoints.Count == 0)
            {
                return 1;
            }
            return keyPoints[keyPoints.Count - 1].Id + 1;
        }
        public List<KeyPoint> GetAll()
        {
            return keyPoints;
        }
        public KeyPoint Save(KeyPoint keyPoint)
        {
            keyPoint.Id = NextId();
            keyPoints.Add(keyPoint);
            _serializer.ToCSV(FilePath, keyPoints);
            return keyPoint;
        }
        public void UpdateKeyPoint(KeyPoint keyPoint)
        {
            KeyPoint oldKeyPoint = keyPoints.Find(k => k.Id == keyPoint.Id);
            oldKeyPoint.IsChecked = keyPoint.IsChecked;
            _serializer.ToCSV(FilePath, keyPoints);
        }
        public KeyPoint GetById(int id)
        {
            KeyPoint keyPoint = keyPoints.Find(x => x.Id == id);
            return keyPoint;
        }
        public List<KeyPoint> GetByTourOccurrence(int id)
        {
            List<KeyPoint> result = new List<KeyPoint>();
            foreach(KeyPoint k in keyPoints)
            {
                if(k.TourOccurrenceId == id)
                {
                    result.Add(k);
                }
            }
            return result;
        }
    }
}
