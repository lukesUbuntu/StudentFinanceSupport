using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentFinanceSupport.Models.functions
{
    public class Reports
    {
        //valid dates
        //2015
        //06/2015
        //09/06/2015
        
        public string date_type { get; set; }
        public string gender { get; set; }

        [Display(Name = "Date")]
        [RegularExpression("^\\d{0,2}\\/\\d{0,2}\\/\\d{0,4}|^\\d{0,2}\\/\\d{0,4}", ErrorMessage = "Date must be in the following format dd/mm/yyyy or mm/yyyy")]
        public string start_date { get; set; }

        public int? Faculity { get; set; }

        [RegularExpression("^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Must be a age number eg. 21")]
        public int? student_age { get; set; }

        public int? Campus { get; set; }
        public int? GrantType { get; set; }
        public DateTime getBirthYear()
        {
            //minus todays date eg 2015 - 15 = born 2000
            int theAge = Convert.ToInt32(student_age) * -1;
            
            DateTime theDate = DateTime.Now.AddYears(theAge);
            return theDate;
        }
        public DateTime getDate()
        {
            //need to check if date valid
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
