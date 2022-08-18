using Microsoft.VisualBasic;
using System;
using UpgradeHelpers.Helpers;

namespace DRFormula
{
	internal class Calculations
	{


		private string mstrSupportedOperators = ""; // Holds all of the supported operators

		private double[] mdblOperands = new double[51]; // Holds the operands' stack
		private string[] mstrOperators = ArraysHelper.InitializeArray<string>(26); // Holds the operators' stack

		private int mintLastOperand = 0; // Position of the last operand in the Operand array
		private int mintLastOperator = 0; // Position of the last operand in the Operator array

		private double Simple(double LOperand, string Operator, double ROperand)
		{
			// This function calculates the values and operators that
			// are passed to it
			//
			// It will return the results of the calculation
			//
			// Supported operators:
			//   Addition                (+)
			//   Subtraction             (-)
			//   Multiplication          (*)
			//   Division                (/)
			//   Raise to the power of   (^)

			double result = 0;
			try
			{

				// ========================================================================
				// We want to check to make sure the calculation has a
				// supported operator in it.
				if ((mstrSupportedOperators.IndexOf(Operator) + 1) < 1)
				{

					throw new System.Exception("20000, Simple Calculation, " + "Operator not supported: " + Environment.NewLine + 
					                           Operator);

				}
				// End validation
				// ========================================================================

				double dblTemp = 0;

				// ========================================================================
				// Do the calculation based on what the operator is
				// When adding support for an operator, it must also
				// be added to the mstrSupportedOperators in the
				// Initialize Subroutine of this class

				switch(Operator)
				{
					case "+" : 
						 
						dblTemp = LOperand + ROperand; 
						 
						break;
					case "-" : 
						 
						dblTemp = LOperand - ROperand; 
						 
						break;
					case "*" : 
						 
						dblTemp = LOperand * ROperand; 
						 
						break;
					case "/" : 
						 
						dblTemp = LOperand / ROperand; 
						 
						break;
					case "^" : 
						 
						dblTemp = Math.Pow(LOperand, ROperand); 
						 
						break;
					default:
						 
						dblTemp = 0; 
						 
						break;
				}


				return dblTemp;
			}
			catch (System.Exception excep)
			{

				result = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message);
			}
		}

		public double Compound(string Math)
		{
			// This function takes a compound formula and calculates it
			// It gets a 'free text' math calculation using the same style
			// syntax as Excel without the '='.
			//   ie: (((3*2)/2)^((4+12)/10))-3
			//
			// This is intended to work with the Formula class.  The formula
			// class builds (via database) the actual formula w/ values
			// instead of variables.  This function does the actual calculation.
			//
			// It will parse out the string and run the calculations through the
			// Simple Function of this Calculation class.

			double result = 0;
			try
			{

				// First we make sure the string passed is valid
				if (!Validate(Math))
				{
					//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
					throw new System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + Environment.NewLine + 
					                           "String Passed: " + Environment.NewLine + 
					                           Math);
				}

				int intCount = 0;

				string strPrevChar = ""; // Holds the value of the previous character
				string strTempNum = ""; // Used to build a number
				double dblTempCalc = 0; // Used to store results of a simple calculation

				// ========================================================
				// Code here for parsing and running the calculations
				// ========================================================

				mintLastOperand = 0;
				mintLastOperator = 0;
				strPrevChar = "";

				int tempForEndVar = Strings.Len(Math);
				for (intCount = 1; intCount <= tempForEndVar; intCount++)
				{


					switch(Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))))
					{
						// Build the number
						case "1" : case "2" : case "3" : case "4" : case "5" : case "6" : case "7" : case "8" : case "9" : case "0" : case "." : case "E" : 
							 
							// Build the number 
							strTempNum = strTempNum + Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))); 
							 
							if ((!Information.IsNumeric(Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)))) && (Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)) != ".") && (Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)) != "E") && (Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)) != "-"))
							{

								//If (Mid$(Math, intCount, 1) <> "E") Then  ' the end of the number is reached

								mdblOperands[mintLastOperand] = Double.Parse(strTempNum);
								mintLastOperand++;

								//End If

							} 
							 
							// Store the operator 
							break;
						case "+" : case "-" : case "*" : case "/" : case "^" : 
							 
							// Have to catch a negative number 
							if (strPrevChar == "(" || strPrevChar == "E" || intCount == 1)
							{

								strTempNum = strTempNum + Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1)));

								if ((!Information.IsNumeric(Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)))) && (Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)) != ".") && (Math.Substring(intCount, System.Math.Min(1, Math.Length - intCount)) != "E"))
								{ // the end of the number is reached

									mdblOperands[mintLastOperand] = Double.Parse(strTempNum);
									mintLastOperand++;

								}

							}
							else
							{

								mstrOperators[mintLastOperator] = Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1)));
								mintLastOperator++;

								strTempNum = "";

							} 
							 
							// A calculation needs to be performed 
							break;
						case ")" : 
							 
							// Run the calculation 
							dblTempCalc = Simple(mdblOperands[mintLastOperand - 2], mstrOperators[mintLastOperator - 1], mdblOperands[mintLastOperand - 1]); 
							 
							MoveStacks(dblTempCalc); 
							 
							strTempNum = ""; 
							 
							break;
						case "(" : case " " : 
							 
							strTempNum = ""; 
							 
							break;
						default:
							 
							// An error occurred 
							throw new System.Exception("20010, Calculations - Compound, " + "An unexpected character in formula: " + Environment.NewLine + 
							                           Math + Environment.NewLine + 
							                           "Found: " + Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))) + 
							                           " at position " + intCount.ToString()); 

					}

					// Store the character temporarily
					strPrevChar = Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1)));

				}

				if (mstrOperators[0] != "")
				{

					// Run the calculation
					dblTempCalc = Simple(mdblOperands[mintLastOperand - 2], mstrOperators[mintLastOperator - 1], mdblOperands[mintLastOperand - 1]);

					MoveStacks(dblTempCalc);

				}


				return mdblOperands[0];
			}
			catch (System.Exception excep)
			{

				result = 0;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message);
			}
		}

		private void MoveStacks(double NewOperand)
		{
			// This moves the numbers in the stacks (Arrays) in
			// a LIFO manner.  And sets the last value to be
			// the 'NewOperand' Value in the Oprands Array

			// Clear out the last operands and operator
			mdblOperands[mintLastOperand - 1] = 0;
			mstrOperators[mintLastOperator - 1] = "";

			// Move the operand pointer back two and
			// operator pointer back one
			mintLastOperand -= 2;
			mintLastOperator--;

			// Put the new value for the last operand
			mdblOperands[mintLastOperand] = NewOperand;
			mintLastOperand++;

		}

		public bool Validate(string Math)
		{
			// This function will validate a compound calculation to ensure that
			// the string adheres to the correct syntax and has valid operators.
			//
			// This checks for:
			//   Equal number of left and right parenthesis
			//   Checks for supported operators

			bool result = false;
			try
			{

				bool bGood = false;

				int intLParens = 0;
				int intRParens = 0;

				int intCount = 0;
				string strValid = "";

				bGood = false; // Default value
				strValid = mstrSupportedOperators + ".()Ee "; // A space must be in there, as it is allowed

				int tempForEndVar = Strings.Len(Math);
				for (intCount = 1; intCount <= tempForEndVar; intCount++)
				{

					// make sure that there are no invalid characters
					if (((strValid.IndexOf(Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1)))) + 1) < 1) && (!Information.IsNumeric(Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))))))
					{

						// An invalid character was detected, send an error with
						// the position of the bad character, along with the invalid
						// character.
						throw new System.Exception("20020, Calculations Validate, " + "There was an invalid character (" + 
						                           Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))) + 
						                           ") at position " + intCount.ToString());

					}

					// if it is a parenthesis, count which one it is
					if (Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))) == "(")
					{
						intLParens++;
					}
					if (Math.Substring(intCount - 1, System.Math.Min(1, Math.Length - (intCount - 1))) == ")")
					{
						intRParens++;
					}

				}

				// Make sure the parenthesis are the same
				if (intLParens == intRParens)
				{
					bGood = true;
				}
				else
				{
					throw new System.Exception("20030, Calculations Validate, Missing Parenthesis");
				}


				return bGood;
			}
			catch (System.Exception excep)
			{

				result = false;
				//UPGRADE_WARNING: (2081) Err.Number has a new behavior. More Information: https://docs.mobilize.net/vbuc/ewis#2081
				throw new System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message);
			}
		}

		public Calculations()
		{

			// To add a supported operator, it also must be
			// added to the select case statement in the
			// Simple function of this class
			mstrSupportedOperators = "-+*/^";

		}
	}
}