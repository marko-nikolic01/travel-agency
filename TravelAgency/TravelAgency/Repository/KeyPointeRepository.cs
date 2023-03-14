using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class KeyPointRepository
    {
        private const string FilePath = "../../../Resources/Data/keyPoints.csv";
        private readonly Serializer<KeyPoint> _serializer;
        private List<KeyPoint> keyPoints;

        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoint>();
            keyPoints = _serializer.FromCSV(FilePath);
        }

        private int GetNewId()
        {
            if (keyPoints.Count == 0)
            {
                return 1;
            }
            return keyPoints[keyPoints.Count - 1].Id + 1;
        }
        public List<KeyPoint> GetKeyPoints()
        {
            return keyPoints;
        }
        public void SaveKeyPoints(KeyPoint keyPoint)
        {
            keyPoint.Id = GetNewId();
            keyPoints.Add(keyPoint);
            _serializer.ToCSV(FilePath, keyPoints);
        }
        public void UpdateKeyPoint(KeyPoint keyPoint)
        {
            KeyPoint oldKeyPoint = keyPoints.Find(k => k.Id == keyPoint.Id);
            oldKeyPoint.IsChecked = keyPoint.IsChecked;
            _serializer.ToCSV(FilePath, keyPoints);
        }

    }
}
