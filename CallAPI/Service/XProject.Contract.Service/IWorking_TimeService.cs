using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface IWorking_TimeService
    {
        List<Working_Time> GetAll();
        Working_Time GetById(string id);
        Working_Time CreateTimeTracking(string Ext, string BrandId, string DailyId);
        Working_Time CreateTimeTracking_Offline(string Ext, string BrandId, string DailyId);
        void Create(Working_Time model);
        void Update(Working_Time model);
        void Delete(string id);
    }
}
