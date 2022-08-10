using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Outsourcing.Data.Models;

namespace Labixa.Models
{
    public class HomeViewModel
    {
        public List<string> ListVideo { get; set; }
        public string HomeVideo { get; set; }
        public string Popup { get; set; }
    }

  
}