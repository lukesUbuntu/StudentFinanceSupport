using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web;


//Creating our helpers namespace
namespace StudentFinanceSupport.Helpers
{ 

/// <summary>
/// Helpers File by Luke Hardiman
/// Purpose is to use as a utils or Helper in MVC
/// @todo possible move this lists to pre defined classes easily casting for data/reports
/// </summary>
public static class Helpers
{
    //private 
    private static string system_name = "Student Finance Support System";
    private static string system_version = "1.01";
    //public global scope
    public static string SelectOneSpacer = "-- Select One --";
    public static string SystemName()
    {
        return system_name + " version " + system_version;
    }

    /// <summary>
    ///  Generate dropdown for Gender List
    /// </summary>
    /// <returns>List</returns>
    public static List<SelectListItem> DropDownGender()
    {
        List<SelectListItem> genderList = new List<SelectListItem>()
                    {
                         new SelectListItem { Text = "Male",    Value = "male" }, 
                         new SelectListItem { Text = "Female",  Value = "female" },
                    };

        return genderList;
    }

    /// <summary>
    ///  Generate dropdown for Accomitdation Type
    /// </summary>
    /// <returns>List</returns>
    public static List<SelectListItem> AccomoditionTypes()
    {
        List<SelectListItem> accomodationList = new List<SelectListItem>()
                    {
                         new SelectListItem { Text = "Hostel/Boarding",    Value = "hostel" }, 
                         new SelectListItem { Text = "Flat",  Value = "flat" },
                         new SelectListItem { Text = "Own Home",  Value = "own_home" },
                         new SelectListItem { Text = "Living at Home",  Value = "living_home" },
                         new SelectListItem { Text = "Flat",  Value = "flat" }
                    };

        return accomodationList;
    }

    /// <summary>
    ///  Generate dropdown for Marital Status
    /// </summary>
    /// <returns>List</returns>
    public static List<SelectListItem> MaritalStatus()
    {
        List<SelectListItem> MaritalStatusList = new List<SelectListItem>()
                    {
                         new SelectListItem { Text = "NZ Citizen",    Value = "NZ_Citizen" }, 
                         new SelectListItem { Text = "NZ Residency",  Value = "NZ_Residency" },
                         new SelectListItem { Text = "International Student",  Value = "International_Student" }
                    };

        return MaritalStatusList;
    }

    /// <summary>
    ///  Generate dropdown for GrantTypes
    ///  @todo make this into a dynamic and editable list
    /// </summary>
    /// <returns>List</returns>
    public static List<SelectListItem> GrantTypes()
    {
        List<SelectListItem> GrantTypesList = new List<SelectListItem>()
                    {
                         new SelectListItem { Text = "Advice",    Value = "Advice" }, 
                         new SelectListItem { Text = "Petrol Vouchers",  Value = "PetrolVouchers" },
                         new SelectListItem { Text = "Food Vouchers",  Value = "FoodVouchers" },
                         new SelectListItem { Text = "Train Tickets",  Value = "TrainTickets" },
                         new SelectListItem { Text = "Hard Ship",  Value = "Hardship" }
                    };

        return GrantTypesList;
    }

    /// <summary>
    /// Outputs to the system local debug window help for checking strings when debug is not an option
    /// </summary>
    /// <param name="theMessage">The message for console</param>
    /// <param name="theLocation">possible location identify (case many outputs)</param>
    public static void Console(string theMessage, string theLocation = "")
    {
        System.Diagnostics.Debug.WriteLine(string.Format("Helper: {0} -> {1}",theMessage,theLocation));
    }

    public static bool UserLoggedIn()
    {
        
        HttpContext context = HttpContext.Current;
        return (context.Session["logged_in"] != null) && (bool)context.Session["logged_in"] == true;
    }

   
    
    
}


}