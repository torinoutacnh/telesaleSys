using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labixa.Hubs
{
    public class ChatHub : Hub
    {
        public static List<Staff> ListStaff = new List<Staff>();
        public static string _key = "123456";
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
        /// <summary>
        /// cập nhật trạng thái nhân viên
        /// </summary>
        /// <param name="Ext"></param>
        /// <param name="Status">1: online, 2: On Call, 3: Available, 4: Offline</param>
        public Task StatusWork(string Name, string Ext, string Status, string BrandId)
        {

            Staff staff = new Staff();
            staff.Name = Name;
            staff.Ext = Ext;
            staff.Status = Status;
            staff.BrandId = BrandId;
            if (ListStaff.Exists(p => p.Ext.Equals(Ext) && p.BrandId.Equals(BrandId)))
            {

                //ton tai 
                ListStaff.Find(p => p.Ext.Equals(Ext) && p.BrandId.Equals(BrandId)).Status = Status;
                ListStaff.Find(p => p.Ext.Equals(Ext) && p.BrandId.Equals(BrandId)).Name = Name;
            }
            else if (Status != "")
            {
                //moi co trang thai
                ListStaff.Add(staff);
            }
            else
            {
                //null check
                return Clients.All.updateStatus(Name, Ext, Status, BrandId);
            }
            return Task.CompletedTask;
        }
        public void GetList()
        {
            //Clients.All.listInformation(JsonConvert.SerializeObject(ListStaff));
            foreach (var item in ListStaff)
            {
                if (item.Status == "" || item.BrandId == "")
                {
                    ListStaff.Remove(item);
                }
            }
            Clients.All.listInformation(ListStaff);
        }
    }
    public class Staff
    {
        public string Name { get; set; }
        public string Ext { get; set; }
        public string Status { get; set; }
        public string BrandId { get; set; }
    }
}