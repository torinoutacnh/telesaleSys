using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outsourcing.Core.Common;
using Outsourcing.Data.Models;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Repository;
using Outsourcing.Service.Properties;
namespace Outsourcing.Service
{
    public interface ITrackingAttendenceService
    {
        IEnumerable<TrackingAttendence> GetTrackingAttendences();
        void CreateTrackingAttendence(TrackingAttendence TrackingAttendence);
        void EditTrackingAttendence(TrackingAttendence TrackingAttendenceToEdit);
        void DeleteTrackingAttendence(int TrackingAttendenceId);
        void SaveTrackingAttendence();
        TrackingAttendence GetTrackingAttendenceById(int TrackingAttendenceId);


    }

    public class TrackingAttendenceService : ITrackingAttendenceService
    {
        #region Field
        private readonly ITrackingAttendenceRepository TrackingAttendenceRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public TrackingAttendenceService(ITrackingAttendenceRepository TrackingAttendenceRepository, IUnitOfWork unitOfWork)
        {
            this.TrackingAttendenceRepository = TrackingAttendenceRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for ITrackingAttendenceService
        public IEnumerable<TrackingAttendence> GetTrackingAttendences()
        {
            var TrackingAttendences = TrackingAttendenceRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
            return TrackingAttendences;
        }

        public TrackingAttendence GetTrackingAttendenceById(int TrackingAttendenceId)
        {
            var TrackingAttendence = TrackingAttendenceRepository.GetById(TrackingAttendenceId);
            return TrackingAttendence;
        }
        public void EditTrackingAttendence(TrackingAttendence TrackingAttendenceToEdit)
        {
            TrackingAttendenceToEdit.DateCreated = DateTime.Now;
            TrackingAttendenceToEdit.LastEditedTime = DateTime.Now;
            TrackingAttendenceRepository.Update(TrackingAttendenceToEdit);
            SaveTrackingAttendence();
        }


        public void CreateTrackingAttendence(TrackingAttendence TrackingAttendence)
        {
            TrackingAttendence.DateCreated = DateTime.Now;
            TrackingAttendence.LastEditedTime = DateTime.Now;

            TrackingAttendenceRepository.Add(TrackingAttendence);
            SaveTrackingAttendence();
        }



        public void DeleteTrackingAttendence(int TrackingAttendenceId)
        {
            //Get TrackingAttendence by id.
            var TrackingAttendence = TrackingAttendenceRepository.GetById(TrackingAttendenceId);
            if (TrackingAttendence != null)
            {
                TrackingAttendence.IsDeleted = true;
                TrackingAttendenceRepository.Update(TrackingAttendence);
                SaveTrackingAttendence();
            }
        }
        //


        public void SaveTrackingAttendence()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
