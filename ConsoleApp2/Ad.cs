using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dooh.com
{
    public class Ad
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string CreativeID { get; set; }

        public string FrameID { get; set; }

        public int DeliveredPlays { get; set; }

        public string CampaignName { get; set; }

        public string Creative { get; set; }

        public string DistrictName { get; set; }

        public string PostCode { get; set; }


    }
}
