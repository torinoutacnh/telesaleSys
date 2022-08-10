using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface IEventZaloService
    {
        List<EventZaloEntity> GetAll();
        EventZaloEntity GetById(string EventZalo_id);
        EventZaloEntity GetEventZaloByName(string name);
        EventZaloEntity Create(EventZaloEntity EventZaloToCreate);
        EventZaloEntity Update(EventZaloEntity EventZaloToEdit);
        void Delete(string id);
    }
}
