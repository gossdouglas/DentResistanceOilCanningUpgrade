using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DentResistanceOilCanningUpgrade.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using DRFormula;

namespace DentResistanceOilCanningUpgrade.Controllers
{
    public class DentResistanceController : ApiController
    {
        // DB Context
        //private readonly ApplicationDbContext _context = new ApplicationDbContext();

        //calculate dent resistance for model 1
        [HttpPost]
        public ReturnObject<CalculationDentReistance> CalculateModelOne(CalculationDentReistance model)
        {
            bool bCalculated;
            //create a new instance of the forumula object
            Formula objDRCalc = new Formula();
            //perform a calculation with passed values
            bCalculated = objDRCalc.Calculate(model.GradeKey, model.R1, model.R2, model.Thickness, model.MajorStrain, model.MinorStrain);
            //if the calculation is successful...
            if (bCalculated)
            {
                model.FootPounds = Math.Round(objDRCalc.LBF, 2);
                model.RunningTotal = Math.Round(objDRCalc.Newtons, 2);

                return new ReturnObject<CalculationDentReistance>() { success = true, data = model, validated = true };
            }
            else
            {
                return new ReturnObject<CalculationDentReistance>() { success = false, data = model, validated = true };
            }
        }

        //calculate dent resistance for model 2
        [HttpPost]
        public ReturnObject<CalculationDentReistance> CalculateModelTwo(CalculationDentReistance model)
        {
            bool bCalculated;
            double dblResultIntercept, dblResultSlope;
            //create a new instance of the forumula object
            Formula objDRCalc = new Formula();
            //perform a calculation with passed values
            bCalculated = objDRCalc.CalculateM2(model.GradeKey, model.R1, model.R2, model.Thickness, model.MajorStrain, model.MinorStrain, "INTERCEPT");
            //if the calculation is successful...
            if (bCalculated)
            {
                dblResultIntercept = objDRCalc.Result;
                //perform a calculation with passed values
                bCalculated = objDRCalc.CalculateM2(model.GradeKey, model.R1, model.R2, model.Thickness, model.MajorStrain, model.MinorStrain, "SLOPE");
                //if the calculation is successful...
                if (bCalculated)
                {
                    dblResultSlope = objDRCalc.Result;
                    model.Result = Math.Round((model.PoundsForce - dblResultIntercept) / dblResultSlope, 3);

                    return new ReturnObject<CalculationDentReistance>() { success = true, data = model, validated = true };
                }
                else
                {
                    return new ReturnObject<CalculationDentReistance>() { success = false, data = model, validated = true };
                }
            }
            else
            {
                return new ReturnObject<CalculationDentReistance>() { success = false, data = model, validated = true };
            }
        }

        ////not used currently.  the values for the combo boxes are hard coded.
        //[HttpGet]
        //public ReturnObject<List<dr_Grades>> GetGrades(dr_Grades drGrades)
        //{
        //    //a new object that will eventually be sent to the view
        //    List<dr_Grades> gradesList = new List<dr_Grades>();
            
        //    string mainconn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    SqlConnection sqlconn = new SqlConnection(mainconn);

        //    //open a SQL connection to the database
        //    sqlconn.Open();

        //    //prepare the query string
        //    string sqlquery1 =
        //        "SELECT Grade_key, Grade_name FROM DR_Grades WHERE model = @drModel ORDER BY Grade_name";

        //    //prepare a method to interact with the results of the SQL string
        //    SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
        //    sqlcomm1.Parameters.AddWithValue("@drModel", drGrades.model);
        //    SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
        //    DataTable dt1 = new DataTable();
        //    sda1.Fill(dt1);
        //    foreach (DataRow dr1 in dt1.Rows)
        //    {
        //        //create an object for each record in the SQL results
        //        dr_Grades grade = new dr_Grades();

        //        grade.grade_key = (int)dr1[0];
        //        grade.grade_name = dr1[1].ToString();

        //        //add it to a list
        //        gradesList.Add(grade);
        //    }

        //    //close the SQL connection to the database
        //    sqlconn.Close();

        //    return new ReturnObject<List<dr_Grades>>() { success = false, data = gradesList, validated = true };

        //    //if (true)
        //    //{

        //    //    return new ReturnObject<dr_Grades>() { success = true, data = model, validated = true };
        //    //}
        //    //else
        //    //{
        //    //    return new ReturnObject<dr_Grades>() { success = false, data = model, validated = true };
        //    //}
        //}

    }
}
