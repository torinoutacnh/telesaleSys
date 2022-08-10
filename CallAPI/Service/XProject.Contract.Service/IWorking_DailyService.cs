using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;
using XProject.Core.Models.Tracking;

namespace XProject.Contract.Service
{
    public interface IWorking_DailyService
    {
        List<Working_Daily> GetAll();
        void PushQueue(string message);
        Working_Daily GetById(string id);
        Working_Daily CreateTracking(string Ext, string BrandId);
        Working_Daily CreateTracking_Offline(string Ext, string BrandId);
        void Create(Working_Daily model);
        void Update(Working_Daily model);
        void Delete(string id);

        List<TrackingDetailModel> GetTrackingDetails(TrackingRequest model);
    }
}
