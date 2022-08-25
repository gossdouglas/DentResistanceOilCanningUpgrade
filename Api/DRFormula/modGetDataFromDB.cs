using Microsoft.VisualBasic;
using System;
using System.Data.Common;
using UpgradeHelpers.DB.ADO;

namespace DRFormula
{
	internal static class modGetDataFromDB
	{


		// Database constants

		public const string strConnect = "DSIspat";
		//public const string strConnect = "DSIspat_test";
		public const string vDataBase = "Ispat Test";
		public const string UserID = "user_dent_resist";
		public const string Password = "$rv_D3nt$!";

		static string ErrorDescription = "";
		static DbConnection conn = null;
		static string sconnectstring = "";
		static bool bl_okay = false;
		static double i_erroradd = 0;
		static int ln_found = 0;
		static ADORecordSetHelper rsdata = null;
		static ADORecordSetHelper rsdataout = null;


		internal static ADORecordSetHelper GetDataFromDB(string vSql)
		{
			ADORecordSetHelper result = null;
			bool initFlag = true, retryFlag = true;
			while (retryFlag)
			{
				retryFlag = false;
				try
				{
					if (initFlag)
					{
						initFlag = false;

						//JHF 2021 Dim oRDS As RDS.DataSpace
						//JHF 2021 Dim oGet As Object

						//JHF 2021    Set oRDS = New RDS.DataSpace
						//JHF 2021    Set oGet = oRDS.CreateObject(GRDS, GetServerName())

						//JHF 2021    Set GetDataFromDB = oGet.GetDATAwUP(strConnect, adOpenStatic, vSql, adLockReadOnly, Userid, Password)
						result = GetDATAwUP(strConnect, CursorTypeEnum.adOpenStatic, vSql, UpgradeHelpers.DB.LockTypeEnum.LockReadOnly, UserID, Password);

						//JHF 2021    Set oGet = Nothing
						//JHF 2021    Set oRDS = Nothing

					}

					return result;
				}
				catch (System.Exception excep)
				{


					//JHF 2021    Set oGet = Nothing
					//JHF 2021    Set oRDS = Nothing
					//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
					throw new System.Exception(Information.Err().Number.ToString() + ", " + "clsDataMngt::GetDataFromDB::" + excep.Source + ", " + excep.Message + " (" + vSql + ")");
					retryFlag = true;
					continue;
				}
			}
			return result;
		}
		internal static ADORecordSetHelper GetDATAwUP(string odbcname, CursorTypeEnum Cursortype, string sqlcode, UpgradeHelpers.DB.LockTypeEnum locktype, string UserID, string Password)
		{
			//******************************************************
			//** Module name: cl_getdayawp.getDATAwUP             **
			//** Purpose: This module will be used to make calls  **
			//**          to the passed ODBC name to select       **
			//**          database table data.                    **
			//** Passed Parameters: ODBCNAME = the name of the    **
			//**                               datasource to use. **
			//**                               This Data source   **
			//**                               must be installed  **
			//**                               on the COM server. **
			//**                    CursorType = The Cursor Type  **
			//**                                 to open the      **
			//**                                 recordset with.  **
			//**                    SQLCODE  = The SQL to run     **
			//**                    Locktype = The locktype to    **
			//**                               Open the recordset **
			//**                               with.              **
			//**                    UserId   = Userid for the     **
			//**                               datasource         **
			//**                    Password = password for the   **
			//**                               user-id.           **
			//** Returns:  Recordset containing data if SQL       **
			//**           execute was sucessful, or an empty     **
			//**           Recordset if it failed.                **
			//**           Err.Description contains the SQL error **
			//**           information.                           **
			//**==================================================**
			//** Maintenance log                                  **
			//**==================================================**
			//** Edward J. Szymoniak  06/26/2000  Created Module  **
			//JHF 2021 - including in DRFormula
			//
			//  jhf - 01/06/2014
			//     added connection.CommandTimeout adjustment
			//     increase from the default value of 30 seconds
			//     to 180 seconds allowing much longer SQL to complete
			//******************************************************

			ADORecordSetHelper result = null;
			Information.Err().Clear();
			ErrorDescription = "";
			try
			{
				//JHF 2021 Set objCtx = GetObjectContext()
				//Set up connection string
				conn = UpgradeHelpers.DB.AdoFactoryManager.GetFactory().CreateConnection();
				UpgradeHelpers.DB.DbConnectionHelper.SetCommandTimeOut(conn, 180);

				//jhf HERE 12/27  sconnectstring = "DSN=" & odbcname & ";User Id=" & Userid & ";Password=" & Password & "; "
				//  JHF 2017-05-24       sconnectstring = "DSN=" & odbcname & ";Trusted_Connection = SSPI; Integrated Security = SSPI;"
				sconnectstring = "DSN=" + odbcname + ";User Id=" + UserID + ";Password=" + Password + "; ";
				//UPGRADE_TODO: (7010) The connection string must be verified to fullfill the .NET data provider connection string requirements. More Information: https://docs.mobilize.net/vbuc/ewis#7010
				//conn.ConnectionString = sconnectstring;
				conn.ConnectionString = "Provider=MSOLEDBSQL;Server=SRCVSFDCSQ117\\AMNS;Database=Dent_Resist_DB;UID=user_dent_resist;PWD=$rv_D3nt$!;";
				conn.Open();

				//Now call the routine to create the recordset
				CreateRs(Cursortype, sqlcode, locktype);
				// Error occurred, so Raise the event
				if (!bl_okay)
				{
					throw new System.Exception(Convert.ToInt32(Constants.vbObjectError + i_erroradd).ToString() + ", , " + ErrorDescription);
				}

				rsdataout = rsdata;
				result = rsdataout;

				//Destroy objects
				//Release the Recordset connection to return to pool
				rsdata.ActiveConnection = null;

				//Close the recordset
				//rsdata.Close
				rsdata = null;

				//Close the connection
				UpgradeHelpers.DB.TransactionManager.DeEnlist(conn);
				conn.Close();
				conn = null;

				//JHF 2021 If Not objCtx Is Nothing Then
				//JHF 2021    objCtx.SetComplete
				//JHF 2021 End If

				return result;
			}
			catch
			{

				//Raise Error Event
				//JHF 2021 If Not objCtx Is Nothing Then
				//JHF 2021     objCtx.SetAbort
				//JHF 2021 End If
				if (ErrorDescription == "")
				{
					ErrorDescription = "Error Connection to Datasource " + odbcname;
					i_erroradd = 9000;
				}

				throw new System.Exception(Convert.ToInt32(Constants.vbObjectError + i_erroradd).ToString() + ", , " + ErrorDescription);
			}
		}


		private static void CreateRs(CursorTypeEnum Cursortype, string sqlcode, UpgradeHelpers.DB.LockTypeEnum locktype)
		{
			try
			{
				//Check for a valid cursortype parameter passed
				if (Cursortype == CursorTypeEnum.adOpenForwardOnly || Cursortype == CursorTypeEnum.adOpenStatic || Cursortype == CursorTypeEnum.adOpenDynamic || Cursortype == CursorTypeEnum.adOpenKeyset)
				{
					// all okay proceed
				}
				else
				{
					Information.Err().Clear();
					ErrorDescription = "Invalid CursorType Value Passed " + ((int) Cursortype).ToString();
					i_erroradd = 1000;
					bl_okay = false;
					return;
				}

				//Check for a valid Locktype parameter passed
				if (locktype == UpgradeHelpers.DB.LockTypeEnum.LockReadOnly || locktype == UpgradeHelpers.DB.LockTypeEnum.LockPessimistic || locktype == UpgradeHelpers.DB.LockTypeEnum.LockOptimistic)
				{
					// all okay
				}
				else
				{
					Information.Err().Clear();
					ErrorDescription = "Invalid Locktype Value Passed " + ((int) locktype).ToString();
					i_erroradd = 1030;
					bl_okay = false;
					return;
				}

				//Check to make sure SQL is not blank
				if (sqlcode != "")
				{
					//all okay proceed
				}
				else
				{
					Information.Err().Clear();
					ErrorDescription = "Blank SQL statement Passed ";
					i_erroradd = 1010;
					bl_okay = false;
					return;
				}

				//Check SQL code to make sure no deletes, inserts, or updates
				// exist in the code
				ln_found = 0;
				//Search for Update
				ln_found = (sqlcode.ToUpper().IndexOf(" UPDATE ") + 1);
				//Update Found, Generate Error
				if (ln_found > 0)
				{
					Information.Err().Clear();
					ErrorDescription = "Update SQL statement Passed ";
					i_erroradd = 1011;
					bl_okay = false;
					return;
				}
				//Search for Delete
				ln_found = (sqlcode.ToUpper().IndexOf(" DELETE ") + 1);
				//Delete Found, Generate Error
				if (ln_found > 0)
				{
					Information.Err().Clear();
					ErrorDescription = "Delete SQL statement Passed ";
					i_erroradd = 1012;
					bl_okay = false;
					return;
				}
				//Search for Insert
				ln_found = (sqlcode.ToUpper().IndexOf(" INSERT ") + 1);
				//Insert Found, Generate Error
				if (ln_found > 0)
				{
					Information.Err().Clear();
					ErrorDescription = "Insert SQL statement Passed ";
					i_erroradd = 1013;
					bl_okay = false;
					return;
				}

				//Create the recordset
				rsdata = new ADORecordSetHelper("");
				//Execute the recordset to retireve the data
				//Set the cursor to a client side cursor
				rsdata.CursorLocation = CursorLocationEnum.adUseClient;
				rsdata.Open(sqlcode, conn, locktype);
				bl_okay = true;
			}
			catch (System.Exception excep)
			{

				//Rescordset Retireval Error
				ErrorDescription = excep.Message;
				i_erroradd = 2000;
				bl_okay = false;
			}
		}
	}
}