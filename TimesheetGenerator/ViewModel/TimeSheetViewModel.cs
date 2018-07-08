using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimesheetGenerator.ViewModel
{
    public class TimeSheetViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string CandidateName
        {
            get; set;
        }

        [Required(ErrorMessage = "This field is required")]
        public string ClientName
        {
            get; set;
        }

        [Required(ErrorMessage = "This field is required")]
        public DateTime EndDate
        {
            get; set;
        }

        [Required(ErrorMessage = "This field is required")]
        public string JobTitle
        {
            get; set;
        }

        [Required(ErrorMessage = "This field is required")]
        public int SheetType
        {
            get; set;
        }

        [Required(ErrorMessage = "This field is required")]
        public DateTime StartDate
        {
            get; set;
        }
    }
}