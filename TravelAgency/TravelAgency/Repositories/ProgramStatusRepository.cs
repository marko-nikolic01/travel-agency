using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class ProgramStatusRepository
    {
        private const string FilePath = "../../../Resources/Data/programstatus.csv";
        private readonly Serializer<ProgramStatus> _serializer;
        private List<ProgramStatus> programStatus;

        public ProgramStatusRepository()
        {
            _serializer = new Serializer<ProgramStatus>();
            programStatus = _serializer.FromCSV(FilePath);
        }
        public ProgramStatus GetProgramStatus()
        {
            return programStatus[0];
        }
        public void SetProgramStatus()
        {
            programStatus[0].IsFirstTimeOpening = false;
            _serializer.ToCSV(FilePath, programStatus);
        }
    }
}
