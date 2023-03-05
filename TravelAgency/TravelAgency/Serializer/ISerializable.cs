using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Serializer
{
    //Provided in the example from canvas
    public interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] values);
    }
}