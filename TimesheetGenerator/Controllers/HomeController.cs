using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimesheetGenerator.ViewModel;

namespace TimesheetGenerator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult SaveDate(TimeSheetViewModel tsvm)
        {
            List<DateTime> datelists = new List<DateTime>();
            if (ModelState.IsValid)
            {
                if (tsvm.SheetType == 2)
                {
                    GenerateWeeklyTimeSheet(tsvm, datelists);
                }
                else
                {
                    GenerateMonthlyTimeSheet(tsvm, datelists);
                }
                StringBuilder builder = new StringBuilder();
                foreach (var dates in datelists)
                {
                    if ((datelists.IndexOf(dates)) % 2 == 0)
                    {
                        builder.Append(dates.ToString("dd/MM/yyyy"));
                        builder.Append(",");
                    }
                    else
                    {
                        builder.AppendLine(dates.ToString("dd/MM/yyyy"));
                    }
                }
                return File(new System.Text.UTF8Encoding().GetBytes(builder.ToString()), "text/csv", "Timesheet.csv");
            }
            else
            {
                return View("Index", tsvm);
            }
        }

        private void CheckDateInInterval(TimeSheetViewModel tsvm, List<DateTime> datelists)
        {
            if (tsvm.StartDate <= tsvm.EndDate)
            {
                datelists.Add(tsvm.StartDate);
            }
            else
            {
                datelists.Add(tsvm.EndDate);
            }
        }

        private void GenerateMonthlyTimeSheet(TimeSheetViewModel tsvm, List<DateTime> datelists)
        {
            datelists.Add(tsvm.StartDate);
            GetLastDayOfMonth(tsvm);

            if (tsvm.StartDate <= tsvm.EndDate)
            {
                datelists.Add(tsvm.StartDate); //last day of first month
            }

            while (tsvm.StartDate < tsvm.EndDate)
            {
                while (tsvm.StartDate.Day != 1 && tsvm.StartDate.AddDays(1) <= tsvm.EndDate)
                {
                    tsvm.StartDate = tsvm.StartDate.AddDays(1);
                    datelists.Add(tsvm.StartDate);
                }
                GetLastDayOfMonth(tsvm);
                CheckDateInInterval(tsvm, datelists);
            }
        }

        private void GenerateWeeklyTimeSheet(TimeSheetViewModel tsvm, List<DateTime> datelists)
        {
            datelists.Add(tsvm.StartDate);
            while (tsvm.StartDate.DayOfWeek != DayOfWeek.Sunday)
            {
                tsvm.StartDate = tsvm.StartDate.AddDays(1);
            }

            if (tsvm.StartDate <= tsvm.EndDate)
            {
                datelists.Add(tsvm.StartDate); //sunday
            }

            while (tsvm.StartDate < tsvm.EndDate)
            {
                while (tsvm.StartDate.DayOfWeek != DayOfWeek.Monday && tsvm.StartDate.AddDays(1) <= tsvm.EndDate)
                {
                    tsvm.StartDate = tsvm.StartDate.AddDays(1);
                    datelists.Add(tsvm.StartDate);
                }
                tsvm.StartDate = tsvm.StartDate.AddDays(6);

                CheckDateInInterval(tsvm, datelists);
            }
        }

        private void GetLastDayOfMonth(TimeSheetViewModel tsvm)
        {
            int dayinthemonth = DateTime.DaysInMonth(tsvm.StartDate.Year, tsvm.StartDate.Month);
            tsvm.StartDate = new DateTime(tsvm.StartDate.Year, tsvm.StartDate.Month, dayinthemonth);
        }
    }
}