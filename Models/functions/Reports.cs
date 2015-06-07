using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentFinanceSupport.Models.functions
{
    public class Reports
    {
        public string date_type { get; set; }
        public string gender { get; set; }

        public string start_date { get; set; }

        public int? Faculity { get; set; }

        public int? Campus { get; set; }
        public int? GrantType { get; set; }
        public DateTime getDate()
        {
            string theFormat = "dd/MM/yyyy";
            switch (this.date_type.ToLower())
            {
                case "month":
                    theFormat = "MM/yyyy";
                    break;
                case "year":
                    theFormat = " yyyy";
                    break;
            }
            DateTime theDate;
            DateTime.TryParseExact
                  (this.start_date,
                   theFormat,
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out theDate);

            return theDate;
        }
    }
}
