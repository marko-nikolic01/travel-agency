using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class ProgramStatus : ISerializable
    {
        public bool IsFirstTimeOpening { get; set; }
        public ProgramStatus() { }
        public string[] ToCSV()
        {
            string[] csvValues = { IsFirstTimeOpening.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            IsFirstTimeOpening = bool.Parse(values[0]);
        }
    }
}
