using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DentResistanceOilCanningUpgrade.Models;
using DentResistanceOilCanningUpgrade.ViewModels;
using DRFormula;

namespace DentResistanceOilCanningUpgrade.Controllers
{
    public class dentResistanceModelController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: dentResistanceModel
        public ActionResult Index()
        {
            List<SelectListItem> grades = new List<SelectListItem>();
            List<dr_Grades> gradesList = _context.dr_Grades.OrderBy(o => o.grade_name).ToList();

            foreach (dr_Grades grade in gradesList)
            {
                if (grade.model == 1 && grade.publish == 1)
                {
                    var tempText = grade.grade_name.ToString();

                    SelectListItem temp = new SelectListItem
                    {
                        Text = tempText,
                        Value = tempText
                    };
                    grades.Add(temp);
                }
            }

            DRFormula.Formula objDRCalc = new DRFormula.Formula();
            //DRFormula.FormulaClass objDRCalc = new DRFormula.FormulaClass();

            //Formula objDRCalc = new Formula();
            bool bCalculated;
            //found on 18may22 by goss
            //bCalculated = objDRCalc.Calculate(8, 150, 15000, .65, .25, .25);
            //bCalculated = objDRCalc.Calculate(-1, -1, -1, 0, 0, 0);
            //decimal.Round((decimal)objDRCalc.LBF, 2);

            //discovery on 18may22 by goss
            bCalculated = objDRCalc.Calculate(1, 150, 15000, .65, .25, .25);

            double dblFtLb, dblRunningTotal;

            if (bCalculated)
            {
                dblFtLb = Math.Round(objDRCalc.LBF, 2);
                dblRunningTotal = Math.Round(objDRCalc.Newtons, 2);
            }

            ViewBag.gradesList = grades;

            //dr_Grades drGrades = new dr_Grades();

            return View();
            //return View(drGrades);
        }

        [HttpPost]
        public ActionResult Calculate(DrGradesViewModel model)
        {


            //return View("Index");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
