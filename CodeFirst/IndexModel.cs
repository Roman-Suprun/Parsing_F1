using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst
{
    public class Collection
    {
        public List<List<string>> CoList = new List<List<string>>();
    }

    public class Team
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public int DriverNumder { get; set; }

        // Ссылка на Driver
        public virtual List<Driver> Drivers { get; set; }
    }

    public class Driver
    {
        
        public Guid DriverId { get; set; }
        public Guid TeamId { get; set; }
        public string DriverName { get; set; }
        

        // Ссылка на Team
        public virtual Team Team { get; set; }

        public virtual Result Result { get; set; }
    }

    public class Result
    {
        [Key, ForeignKey("Driver")]
        //public Guid ResultId { get; set; }
        public Guid DriverId { get; set; }
        public string Position { get; set; }
        public int Laps { get; set; }
        public int Pts { get; set; }
        public string Time { get; set; }


        // Ссылка на Driver
        //[Required]
        public virtual Driver Driver { get; set; }
    }

    

}
