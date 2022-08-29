using Microsoft.VisualBasic;
using System;
using UpgradeHelpers.DB.ADO;
using UpgradeHelpers.Helpers;
using DentResistanceOilCanningUpgrade.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DRFormula
{
	public class Formula
	{
		//25Aug22
		//Tony Goss
		//This logic was converted from the Visual Basic COM+ object used in the classic ASP version of this site
		//As little editing as possible was done to this logic in order to reduce the possiblity of calculation errors
		//caused by the upgrade

		private string mstrConn = "";
		private double mdblNewtons = 0;
		private string strServer = "";

		private object oMngt = null; // Com2Com clsDataMngt object
		private ADORecordSetHelper rsGrade = null; // Grade Recordset
		private ADORecordSetHelper rsFormula = null; // Formula Recordset
		private string strSQL = ""; // To be used to get recordsets
		private double mdblResult = 0;
		// Added for Oil Canning and Stiffness code BDS
		private double mdblEPS3 = 0; // Used to Calculate Actual R1, Actual R2, and Actual T
		private double mdblActualT = 0; // Used to Calculate Actual R1 and Actual R2

		public bool Calculate(int GradeKey, double R1, double R2, double Thickness, double MajorStrain, double MinorStrain)
		{
			List<dr_Grades> rsGradeList = new List<dr_Grades>();

			// This is the main function that will be used from the
			// Web interface.  It will get the values of the formula
			// from the database, calculate each formula using the
			// Calculations.Compound function. It will return the true
			// if everything went ok, false if there was an error.
			//
			// The values will be held in the public (readonly)
			// properties:
			//   Newtons
			//   LBF
			//
			// The GradeKey value passed to this function relates to
			// the primary key (Grade_key) in the Grades table

			bool result2 = false;
			try
			{

				Calculations clcCalc = null;

				double dblNormalAnisotropy = 0;
				double dblCoefficient = 0;
				double dblEffectiveStretch = 0;
				double dblConstant = 0;

				string strDBFormula = ""; // The formula as it comes from
				// the Database
				double dblCalcResults = 0; // Temporarily hold the Calculation
				// Results
				double dblRunningTotal = 0; // Storing the running total
				string strFormula = ""; // We build the string that
				// will contain the formula to
				// pass to calculations.compound
				dblRunningTotal = 0;

				// Set up the databases
				//JHF 2021    Set oMngt = CreateObject("com2com.clsDataMngt", strServer)

				// Get the Grade information from the DB
				strSQL = "SELECT * FROM DR_Grades WHERE Grade_Key = " + GradeKey.ToString() + " and model = 1 ";
				//JHF 2021    Set rsGrade = oMngt.GetDataFromDB(strSQL)
				rsGrade = modGetDataFromDB.GetDataFromDB(strSQL);

				if (rsGrade.RecordCount > 0)
				{

					dblNormalAnisotropy = Convert.ToDouble(rsGrade["Normal_Anisotropy"]);
					dblConstant = Convert.ToDouble(rsGrade["Constants"]);

					dblEffectiveStretch = ((1 + dblNormalAnisotropy) / Math.Sqrt(1 + 2 * dblNormalAnisotropy)) * Math.Sqrt((Math.Pow(MajorStrain / 100d, 2)) + (Math.Pow(MinorStrain / 100d, 2)) + (2 * dblNormalAnisotropy / (1 + dblNormalAnisotropy)) * (MajorStrain / 100d) * (MinorStrain / 100d));

					// Get the Formula information from the DB
					strSQL = "SELECT * FROM DR_Formulas WHERE Grade_Key = " + 
					         GradeKey.ToString() + " and model = 1 ORDER BY Process_Order";
					//JHF 2021        Set rsFormula = oMngt.GetDataFromDB(strSQL)
					rsFormula = modGetDataFromDB.GetDataFromDB(strSQL);

					// Loop through the formulas and add their results
					if (rsFormula.RecordCount > 0)
					{

						dblRunningTotal += dblConstant;
						clcCalc = new Calculations();


						while(!rsFormula.EOF)
						{

							dblCoefficient = Convert.ToDouble(rsFormula["Coefficient"]);
							strDBFormula = Convert.ToString(rsFormula["Formula"]).ToLower();

							// Replace the variables in the free text formula
							// with the values that were calculated or passed
							strDBFormula = StringsHelper.Replace(strDBFormula, "effectivestretch", dblEffectiveStretch.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r1", R1.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r2", R2.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "thickness", Thickness.ToString(), 1, -1, CompareMethod.Binary);

							// This should resemble
							//             (coefficient * formula)
							strFormula = "(" + dblCoefficient.ToString() + "*" + strDBFormula + ")";
							dblCalcResults = clcCalc.Compound(strFormula);

							// Calculate the formula and add it to the running total
							dblRunningTotal += dblCalcResults;

							rsFormula.MoveNext();

						};

						clcCalc = null;

					}

					rsFormula = null;

				}

				oMngt = null;
				rsGrade = null;

				// put the results into the private mdblNewtons variable
				mdblNewtons = dblRunningTotal;

				// All went well

				return true;
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = false;
				mdblNewtons = 0;

				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula Calculate Error: " + Environment.NewLine + excep.Message);
			}
		}
		public bool CalculateM2(int GradeKey, double R1, double R2, double Thickness, double MajorStrain, double MinorStrain, string ModelFormula)
		{
			// This is the main function that will be used from the
			// Web interface.  It will get the values of the formula
			// from the database, calculate each formula using the
			// Calculations.Compound function. It will return the true
			// if everything went ok, false if there was an error.
			//
			// The values will be held in the public (readonly)
			// properties:
			//   Newtons
			//   LBF
			//
			// The GradeKey value passed to this function relates to
			// the primary key (Grade_key) in the Grades table

			bool result2 = false;
			try
			{

				Calculations clcCalc = null;

				double dblNormalAnisotropy = 0;
				double dblCoefficient = 0;
				double dblEffectiveStretch = 0;
				double dblConstant = 0;

				string strDBFormula = ""; // The formula as it comes from
				// the Database
				double dblCalcResults = 0; // Temporarily hold the Calculation
				// Results
				double dblRunningTotal = 0; // Storing the running total
				string strFormula = ""; // We build the string that
				// will contain the formula to
				// pass to calculations.compound
				dblRunningTotal = 0;

				// Set up the databases
				//JHF 2021    Set oMngt = CreateObject("com2com.clsDataMngt", strServer)

				// Get the Grade information from the DB
				strSQL = "SELECT * FROM DR_Grades WHERE Grade_Key = " + GradeKey.ToString() + " and model = 2 ";
				//JHF 2021    Set rsGrade = oMngt.GetDataFromDB(strSQL)
				rsGrade = modGetDataFromDB.GetDataFromDB(strSQL);

				if (rsGrade.RecordCount > 0)
				{

					dblNormalAnisotropy = Convert.ToDouble(rsGrade["Normal_Anisotropy"]);

					if (ModelFormula == "INTERCEPT")
					{
						dblConstant = Convert.ToDouble(rsGrade["Constants"]);
					}
					else
					{
						dblConstant = Convert.ToDouble(rsGrade["Constants_1"]);
					}

					dblEffectiveStretch = ((1 + dblNormalAnisotropy) / Math.Sqrt(1 + 2 * dblNormalAnisotropy)) * Math.Sqrt((Math.Pow(MajorStrain / 100d, 2)) + (Math.Pow(MinorStrain / 100d, 2)) + (2 * dblNormalAnisotropy / (1 + dblNormalAnisotropy)) * (MajorStrain / 100d) * (MinorStrain / 100d));

					// Get the Formula information from the DB
					strSQL = "SELECT * FROM DR_Formulas WHERE Grade_Key = " + 
					         GradeKey.ToString() + " and model = 2 and model_formula = '" + ModelFormula + "' ORDER BY Process_Order";
					//JHF 2021        Set rsFormula = oMngt.GetDataFromDB(strSQL)
					rsFormula = modGetDataFromDB.GetDataFromDB(strSQL);

					// Loop through the formulas and add their results
					if (rsFormula.RecordCount > 0)
					{

						dblRunningTotal += dblConstant;
						clcCalc = new Calculations();


						while(!rsFormula.EOF)
						{

							dblCoefficient = Convert.ToDouble(rsFormula["Coefficient"]);
							strDBFormula = Convert.ToString(rsFormula["Formula"]).ToLower();

							// Replace the variables in the free text formula
							// with the values that were calculated or passed
							strDBFormula = StringsHelper.Replace(strDBFormula, "effectivestretch", dblEffectiveStretch.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r1", R1.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r2", R2.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "thickness", Thickness.ToString(), 1, -1, CompareMethod.Binary);
							// strDBFormula = Replace(strDBFormula, "ModelFormula", CStr(ModelFormula))

							// This should resemble
							//             (coefficient * formula)
							strFormula = "(" + dblCoefficient.ToString() + "*" + strDBFormula + ")";
							dblCalcResults = clcCalc.Compound(strFormula);
							//Debug.Print dblCalcResults

							// Calculate the formula and add it to the running total
							dblRunningTotal += dblCalcResults;

							rsFormula.MoveNext();

						};

						clcCalc = null;

					}

					rsFormula = null;

				}

				oMngt = null;
				rsGrade = null;

				// put the results into the private mdblNewtons variable
				mdblResult = dblRunningTotal;

				// All went well

				return true;
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = false;
				mdblResult = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula CalculateM2 Error: " + Environment.NewLine + excep.Message);
			}
		}

		// =======================================================
		// Added for Oil Canning and Initial Stiffness Models
		// BDS 11/23/2009
		public bool CalculateOilCanning(int IndenterSizeKey, double R1, double R2, double Thickness, double MajorStrain, double MinorStrain, double DeltaSpringback, double FreespanRoofBows = 0)
		{
			// This function is the same concept as the other Calculation Functions, it is used
			// from the web interface to calculate the OilCanning in the same basic manner
			// as the others

			// The "IndenterSizeKey" is the same as the GradeKey for the other calculations
			// It is simply renamed

			// The "FreespanRoofBows" variable is for future use.  It was put in here
			// so that the functionality could be easily added when it is ready
			bool result2 = false;
			double mdblResult = 0;
			try
			{


				int intModel = 0;
				string strModelFormula = "";
				intModel = 3;
				strModelFormula = "OILCANNING";

				double dblR1 = 0;
				double dblR2 = 0;

				// First we set the EPS and ActualT values
				SetEPS3andActualT(Thickness, MajorStrain, MinorStrain);

				// set the Actual R1 and Actual R2
				dblR1 = ActualR1(R1, DeltaSpringback);
				dblR2 = ActualR2(R2, DeltaSpringback);


				return RunCalculations(intModel, strModelFormula, IndenterSizeKey, dblR1, dblR2, mdblActualT, MajorStrain, MinorStrain, DeltaSpringback, FreespanRoofBows) != 0;
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = false;
				mdblResult = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula CalculateOilCanning Error: " + Environment.NewLine + excep.Message);
			}
		}

		public bool CalculateInitialStiffness(int IndenterSizeKey, double R1, double R2, double Thickness, double MajorStrain, double MinorStrain, double DeltaSpringback, double FreespanRoofBows = 0)
		{
			// This function is the same concept as the other Calculation Functions, it is used
			// from the web interface to calculate the InitialStiffness in the same basic manner
			// as the others

			// The "IndenterSizeKey" is the same as the GradeKey for the other calculations
			// It is simply renamed

			// The "FreespanRoofBows" variable is for future use.  It was put in here
			// so that the functionality could be easily added when it is ready
			bool result2 = false;
			double mdblResult = 0;
			try
			{


				int intModel = 0;
				string strModelFormula = "";
				intModel = 3;
				strModelFormula = "INITIALSTIFFNESS";

				double dblR1 = 0;
				double dblR2 = 0;

				// First we set the EPS and ActualT values
				SetEPS3andActualT(Thickness, MajorStrain, MinorStrain);

				// set the Actual R1 and Actual R2
				dblR1 = ActualR1(R1, DeltaSpringback);
				dblR2 = ActualR2(R2, DeltaSpringback);



				return RunCalculations(intModel, strModelFormula, IndenterSizeKey, dblR1, dblR2, mdblActualT, MajorStrain, MinorStrain, DeltaSpringback, FreespanRoofBows) != 0;
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = false;
				mdblResult = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula CalculateInitialStiffness Error: " + Environment.NewLine + excep.Message);
			}
		}

		private double RunCalculations(int Model, string ModelFormula, int IndenterSizeKey, double R1, double R2, double Thickness, double MajorStrain, double MinorStrain, double DeltaSpringback, double FreespanRoofBows = 0)
		{
			double result2 = 0;
			try
			{

				Calculations clcCalc = null;

				double dblNormalAnisotropy = 0;
				double dblCoefficient = 0;
				double dblEffectiveStretch = 0;
				double dblConstant = 0;
				double dblFreespan = 0;

				string strDBFormula = ""; // The formula as it comes from
				// the Database
				double dblCalcResults = 0; // Temporarily hold the Calculation
				// Results
				double dblRunningTotal = 0; // Storing the running total
				string strFormula = ""; // We build the string that
				// will contain the formula to
				// pass to calculations.compound
				dblRunningTotal = 0;

				// Set up the databases
				//JHF 2021    Set oMngt = CreateObject("com2com.clsDataMngt", strServer)

				// Get the Grade information from the DB
				strSQL = "SELECT * FROM DR_Grades WHERE Grade_Key = " + IndenterSizeKey.ToString() + 
				         " and model = " + Model.ToString();

				//JHF 2021    Set rsGrade = oMngt.GetDataFromDB(strSQL)
				rsGrade = modGetDataFromDB.GetDataFromDB(strSQL);

				if (rsGrade.RecordCount > 0)
				{

					dblNormalAnisotropy = Convert.ToDouble(rsGrade["Normal_Anisotropy"]);
					dblConstant = Convert.ToDouble(rsGrade["Constants"]);

					// Get the Formula information from the DB
					strSQL = "SELECT * FROM DR_Formulas WHERE Grade_Key = " + 
					         IndenterSizeKey.ToString() + " and model = " + Model.ToString() + 
					         " and model_formula = '" + ModelFormula + "'" + 
					         " ORDER BY Process_Order";
					//JHF 2021        Set rsFormula = oMngt.GetDataFromDB(strSQL)
					rsFormula = modGetDataFromDB.GetDataFromDB(strSQL);

					// Loop through the formulas and add their results
					if (rsFormula.RecordCount > 0)
					{

						if (ModelFormula.ToUpper() == "INITIALSTIFFNESS" && Convert.ToString(rsFormula["Variable_name"]).ToUpper() == "CONSTANT")
						{

							dblConstant = Convert.ToDouble(rsFormula["Coefficient"]);
							rsFormula.MoveNext();

						}

						dblRunningTotal += dblConstant;
						clcCalc = new Calculations();


						while(!rsFormula.EOF)
						{

							if (ModelFormula.ToUpper() == "INITIALSTIFFNESS" && Convert.ToString(rsFormula["Variable_name"]).ToUpper() == "CONSTANT")
							{

								rsFormula.MoveNext();

							}

							dblCoefficient = Convert.ToDouble(rsFormula["Coefficient"]);
							strDBFormula = Convert.ToString(rsFormula["Formula"]).ToLower();

							// Replace the variables in the free text formula
							// with the values that were calculated or passed
							strDBFormula = StringsHelper.Replace(strDBFormula, "effectivestretch", dblEffectiveStretch.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r1", R1.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "r2", R2.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "thickness", Thickness.ToString(), 1, -1, CompareMethod.Binary);
							strDBFormula = StringsHelper.Replace(strDBFormula, "d", FreespanRoofBows.ToString(), 1, -1, CompareMethod.Binary);

							// This should resemble
							//             (coefficient * formula)
							strFormula = "(" + dblCoefficient.ToString() + "*" + strDBFormula + ")";
							dblCalcResults = clcCalc.Compound(strFormula);

							// Calculate the formula and add it to the running total
							dblRunningTotal += dblCalcResults;

							rsFormula.MoveNext();

						};

						clcCalc = null;

					}

					rsFormula = null;

				}

				oMngt = null;
				rsGrade = null;

				// put the results into the private mdblNewtons variable
				mdblNewtons = dblRunningTotal;

				// All went well

				return -1;
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = 0;
				mdblNewtons = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula RunCalculations Error: " + Environment.NewLine + excep.Message);
			}
		}

		private void SetEPS3andActualT(double Thickness, double MajorStrain, double MinorStrain)
		{
			// EPS3 Formula will be: -((LN(1+MajorStrain/100))+(LN(1+MinorStrain/100)))
			// This value will be stored in the mdblEPS3 private variable to be used in other calculations

			// calculating a logarithm should be as simple as using the Log(double) function
			// might also use: Log10 = Log(X) / Log(10#)

			// ActualT Formula will be: Thickness*(EXP(EPS3))
			// This value will be stored in the mdblActualT private variable

			try
			{

				mdblEPS3 = -((Math.Log(1 + (MajorStrain / 100d))) + (Math.Log(1 + (MinorStrain / 100d))));
				mdblActualT = Thickness * (Math.Exp(mdblEPS3));
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				mdblEPS3 = 0;
				mdblActualT = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula Calculate Actual EPS3 and Actual T Error: " + Environment.NewLine + excep.Message);
			}

		}

		private double ActualR1(double R1, double Delta)
		{
			// we need to ensure that the EPS and ActualT values are set before continuing
			// this error should never happen
			double result2 = 0;
			try
			{


				return R1 * (1 + (Delta / 100d));
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula Calculate Actual R1 Error: " + Environment.NewLine + excep.Message);
			}
		}

		private double ActualR2(double R2, double Delta)
		{
			// we need to ensure that the EPS and ActualT values are set before continuing
			// this error should never happen
			double result2 = 0;
			try
			{


				return R2 * (1 + (Delta / 100d));
			}
			catch (System.Exception excep)
			{

				// Bad things happened
				result2 = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + "Formula Calculate Actual R2 Error: " + Environment.NewLine + excep.Message);
			}
		}
		// End of Oil Canning and Stiffness Models Additions
		// BDS 11/23/2009
		// =======================================================

		public double Result
		{
			get
			{

				return mdblResult;

			}
		}

		public double Newtons
		{
			get
			{

				return mdblNewtons;

			}
		}


		public double LBF
		{
			get
			{

				return mdblNewtons / 4.448d;

			}
		}


		public Formula()
		{

			try
			{

				// NOTE: the server name is SET in ALL CAPS
				strServer = Environment.GetEnvironmentVariable("ComputerName").ToUpper();
			}
			catch (System.Exception excep)
			{

				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + "Formula Initialize" + Environment.NewLine + excep.Source + ", " + excep.Message);
			}

		}
		private static Formula _DefaultInstance = null;
		public static Formula DefaultInstance
		{
			get
			{
				if (_DefaultInstance is null)
				{
					_DefaultInstance = new Formula();
				}
				return _DefaultInstance;
			}
		}

	}
}