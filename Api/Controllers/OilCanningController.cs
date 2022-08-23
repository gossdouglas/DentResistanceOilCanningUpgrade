using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DentResistanceOilCanningUpgrade.Models;

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DentResistanceOilCanningUpgrade.Api.Models;
using System.Web;

namespace DentResistanceOilCanningUpgrade.Controllers
{
    public class OilCanningController : ApiController
    {
        [HttpPost]
        public ReturnObject<VmOilCanningReturn> CalculateOilCanning(OilCanning model)
        {
            VmOilCanningReturn oilCanningReturn = new VmOilCanningReturn();
            List<Chart> chartList = new List<Chart>();

            double ocvar = model.ocvar;
            double peakld = model.peakld;
            double fvr = model.fvr;
            double svr = model.svr;
            double gaugeini = model.gaugeini;
            double span = model.span;
            double emaj = model.emaj;
            double emin = model.emin;
            double DDQ = model.DDQ;
            double BH210 = model.BH210;

            double[] numArray1 = new double[21];
            double[] numArray2 = new double[11];
            double[] numArray3 = new double[11];
            double[] numArray4 = new double[11];
            double[] numArray5 = new double[11];
            float[] numArray6 = new float[213];
            float[] numArray7 = new float[213];
            int num1 = 0;
            double x1 = Math.Log(1.0 + emaj / double.Parse("100.0"));
            double x2 = Math.Log(1.0 + emin / double.Parse("100.0"));
            double d = (x1 + x2) * -1.0;
            double x3 = gaugeini * Math.Exp(d);
            double x4 = double.Parse("1") / fvr * 10000.0;
            double x5 = double.Parse("1") / svr * 10000.0;
            double num2 = double.Parse("1.8");
            double num3 = (double.Parse("1") + num2) / Math.Sqrt(1.0 + 2.0 * num2);
            double num4 = double.Parse("2") * num2 / (1.0 + num2);
            double num5 = num3 * Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0) + num4 * x1 * x2);
            double num6 = double.Parse("-72.53232126") + x4 * double.Parse("15.59828914") + x5 * double.Parse("13.0651791") + x3 * double.Parse("213.1634827") + num5 * double.Parse("100") * double.Parse("-1.95848059") + Math.Pow(x4, 2.0) * double.Parse("-0.795970382") + Math.Pow(x5, 2.0) * double.Parse("-0.797708151") + Math.Pow(x3, 2.0) * double.Parse("-27.44855967") + Math.Pow(num5 * double.Parse("100"), 2.0) * double.Parse("0.098745276") + x4 * x5 * double.Parse("-0.364333318") + x4 * x3 * double.Parse("-21.50222199") + x5 * x3 * double.Parse("-18.23194649") + x4 * (num5 * double.Parse("100.0")) * double.Parse("0.137396826") + x5 * (num5 * double.Parse("100.0")) * double.Parse("-0.217857199") + x3 * (num5 * double.Parse("100.0")) * double.Parse("15.4962963");
            DDQ = num6;
            double num7 = double.Parse("1.36");
            double num8 = (double.Parse("1") + num7) / Math.Sqrt(1.0 + 2.0 * num7);
            double num9 = double.Parse("2") * num7 / (1.0 + num7);
            double num10 = num8 * Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0) + num9 * x1 * x2);
            double num11 = x4 * 4.236321106 - 28.68952216 + x5 * 9.637663376 + x3 * 149.0650957 + num10 * 100.0 * -2.694921243 + Math.Pow(x4, 2.0) * -0.927863682 + Math.Pow(x5, 2.0) * -1.783005598 + Math.Pow(x3, 2.0) * 130.2316872 + Math.Pow(num10 * 100.0, 2.0) * -2.233762661 + x4 * x5 * 0.071358337 + x4 * x3 * -0.229851852 + x5 * x3 * -5.062709231 + x4 * (num10 * 100.0) * -0.1787746 + x5 * (num10 * 100.0) * 0.103470248 + x3 * (num10 * 100.0) * 44.96068783;
            BH210 = num11;

            double num12 = x3 - double.Parse("0.7");
            double num13 = double.Parse("10000.0") / fvr - double.Parse("2.0");
            double num14 = double.Parse("10000.0") / svr - double.Parse("2.0");
            double num15 = double.Parse("1000.0") / span - double.Parse("4.0");
            numArray2[2] = Math.Exp(double.Parse("4.27941") + double.Parse("3.192017") * num12 + double.Parse("0.276748") * num13 + double.Parse("0.077946") * num14 + double.Parse("0.401123") * num15 - double.Parse("2.500264") * num12 * num12 + 0.0 * num13 * num13 + double.Parse("0.053851") * num14 * num14 + double.Parse("0.084306") * num15 * num15 + double.Parse("0.047732") * num12 * num13 + double.Parse("0.153347") * num12 * num14 - double.Parse("0.49623") * num12 * num15 + double.Parse("0.018811") * num13 * num14 + 0.0 * num13 * num15 - double.Parse("0.166958") * num14 * num15);
            numArray2[3] = Math.Exp(double.Parse("4.613227") + double.Parse("2.800366") * num12 + double.Parse("0.182534") * num13 - double.Parse("0.109508") * num14 + double.Parse("0.602971") * num15 - double.Parse("2.466414") * num12 * num12 - double.Parse("0.022599") * num13 * num13 + double.Parse("0.057154") * num14 * num14 + double.Parse("0.097518") * num15 * num15 + double.Parse("0.139263") * num12 * num13 + double.Parse("0.323791") * num12 * num14 - double.Parse("0.589021") * num12 * num15 + double.Parse("0.005832") * num13 * num14 - double.Parse("0.047279") * num13 * num15 - double.Parse("0.195309") * num14 * num15);
            numArray2[4] = Math.Exp(double.Parse("5.003404") + double.Parse("2.616944") * num12 + double.Parse("0.116201") * num13 - double.Parse("0.211101") * num14 + double.Parse("0.762237") * num15 - double.Parse("1.79095") * num12 * num12 - double.Parse("0.007297") * num13 * num13 + double.Parse("0.054589") * num14 * num14 + double.Parse("0.10057") * num15 * num15 + double.Parse("0.126543") * num12 * num13 + double.Parse("0.141616") * num12 * num14 - double.Parse("0.517168") * num12 * num15 - double.Parse("0.01239") * num13 * num14 - double.Parse("0.044924") * num13 * num15 - double.Parse("0.193162") * num14 * num15);
            numArray2[5] = Math.Exp(double.Parse("5.373838") + double.Parse("2.113913") * num12 + double.Parse("0.070318") * num13 - double.Parse("0.274353") * num14 + double.Parse("0.874416") * num15 - double.Parse("2.529494") * num12 * num12 - double.Parse("0.000384") * num13 * num13 + double.Parse("0.076426") * num14 * num14 + double.Parse("0.099986") * num15 * num15 + double.Parse("0.179211") * num12 * num13 + double.Parse("0.151922") * num12 * num14 - double.Parse("0.733827") * num12 * num15 - double.Parse("0.001944") * num13 * num14 - double.Parse("0.049825") * num13 * num15 - double.Parse("0.192579") * num14 * num15);
            numArray2[6] = Math.Exp(double.Parse("5.772223") + double.Parse("2.533498") * num12 + double.Parse("0.028164") * num13 - double.Parse("0.400536") * num14 + double.Parse("1.173905") * num15 - double.Parse("2.443019") * num12 * num12 + double.Parse("0.00487") * num13 * num13 + double.Parse("0.111013") * num14 * num14 + double.Parse("0.187753") * num15 * num15 + double.Parse("0.100495") * num12 * num13 + double.Parse("0.078791") * num12 * num14 - double.Parse("0.381425") * num12 * num15 + double.Parse("0.010735") * num13 * num14 - double.Parse("0.058385") * num13 * num15 - double.Parse("0.234094") * num14 * num15);
            numArray2[7] = Math.Exp(6.178011 + double.Parse("2.201") * num12 - double.Parse("0.023444") * num13 - double.Parse("0.513154") * num14 + double.Parse("1.392907") * num15 - double.Parse("2.184983") * num12 * num12 + double.Parse("0.012779") * num13 * num13 + double.Parse("0.131457") * num14 * num14 + double.Parse("0.225974") * num15 * num15 + double.Parse("0.050317") * num12 * num13 + double.Parse("0.124977") * num12 * num14 - double.Parse("0.575138") * num12 * num15 + double.Parse("0.019902") * num13 * num14 - double.Parse("0.077663") * num13 * num15 - double.Parse("0.270135") * num14 * num15);
            numArray2[8] = Math.Exp(6.471374 + double.Parse("2.025984") * num12 - double.Parse("0.074294") * num13 - double.Parse("0.552174") * num14 + double.Parse("1.420372") * num15 - double.Parse("3.159497") * num12 * num12 + double.Parse("0.017696") * num13 * num13 + double.Parse("0.143595") * num14 * num14 + double.Parse("0.19991") * num15 * num15 - double.Parse("0.000382") * num12 * num13 + double.Parse("0.120918") * num12 * num14 - double.Parse("0.658293") * num12 * num15 + double.Parse("0.032529") * num13 * num14 - double.Parse("0.097144") * num13 * num15 - double.Parse("0.262298") * num14 * num15);
            numArray2[9] = Math.Exp(double.Parse("6.955466") + double.Parse("1.987479") * num12 - double.Parse("0.161447") * num13 - double.Parse("0.665285") * num14 + double.Parse("1.758484") * num15 - double.Parse("2.392414") * num12 * num12 + double.Parse("0.016408") * num13 * num13 + double.Parse("0.149745") * num14 * num14 + double.Parse("0.276407") * num15 * num15 + double.Parse("0.061953") * num12 * num13 + double.Parse("0.229413") * num12 * num14 - double.Parse("0.640081") * num12 * num15 + double.Parse("0.055921") * num13 * num14 - double.Parse("0.134679") * num13 * num15 - double.Parse("0.296333") * num14 * num15);
            numArray2[10] = numArray2[9] * numArray2[9] / numArray2[8];
            numArray3[2] = numArray2[2] * (0.229131 - double.Parse("0.533769") * num12 - double.Parse("0.09538") * num13 - double.Parse("0.171593") * num14 + double.Parse("0.192254") * num15 + double.Parse("0.15805") * num12 * num12 + double.Parse("0.017829") * num14 * num14 - double.Parse("0.004949") * num15 * num15 - double.Parse("0.022224") * num12 * num13 - double.Parse("0.023265") * num12 * num14 - double.Parse("0.107278") * num12 * num15 - double.Parse("0.007157") * num13 * num14 + double.Parse("0.006589") * num14 * num15);
            numArray3[3] = numArray2[3] * (0.391165 - double.Parse("0.421133") * num12 - double.Parse("0.06414") * num13 - double.Parse("0.099538") * num14 + double.Parse("0.130754") * num15 + double.Parse("1.024189") * num12 * num12 - double.Parse("0.004901") * num13 * num13 + double.Parse("0.000543") * num14 * num14 - double.Parse("0.02751") * num15 * num15 + double.Parse("0.034047") * num12 * num13 + double.Parse("0.050731") * num12 * num14 - double.Parse("0.083898") * num12 * num15 - double.Parse("0.027176") * num13 * num14 + double.Parse("0.025477") * num13 * num15 + double.Parse("0.025167") * num14 * num15);
            numArray3[4] = numArray2[4] * (double.Parse("0.403497") - double.Parse("0.313428") * num12 - double.Parse("0.019231") * num13 - double.Parse("0.017605") * num14 + double.Parse("0.061348") * num15 - double.Parse("0.178529") * num12 * num12 + double.Parse("0.004678") * num13 * num13 + double.Parse("0.005024") * num14 * num14 - double.Parse("0.038606") * num15 * num15 - double.Parse("0.018657") * num12 * num13 - double.Parse("0.110628") * num12 * num14 - double.Parse("0.025661") * num12 * num15 - double.Parse("0.009588") * num13 * num14 + double.Parse("0.026129") * num13 * num15 + double.Parse("0.047928") * num14 * num15);
            numArray3[5] = numArray2[5] * (double.Parse("0.350108") - double.Parse("0.132292") * num12 - double.Parse("0.003085") * num13 + double.Parse("0.021036") * num14 + double.Parse("0.005417") * num15 - double.Parse("0.558879") * num12 * num12 + double.Parse("0.00585") * num13 * num13 + double.Parse("0.006212") * num14 * num14 - double.Parse("0.044316") * num15 * num15 - double.Parse("0.054108") * num12 * num13 - double.Parse("0.06248") * num12 * num14 + double.Parse("0.017229") * num12 * num15 + double.Parse("0.00384") * num13 * num14 + double.Parse("0.021152") * num13 * num15 + double.Parse("0.055015") * num14 * num15);
            numArray3[6] = numArray2[6] * (double.Parse("0.283007") - double.Parse("0.055599") * num12 - double.Parse("0.001509") * num13 + double.Parse("0.031342") * num14 - double.Parse("0.036797") * num15 - double.Parse("0.403868") * num12 * num12 + double.Parse("0.004872") * num13 * num13 + double.Parse("0.00918") * num14 * num14 - double.Parse("0.051491") * num15 * num15 - double.Parse("0.024149") * num12 * num13 - double.Parse("0.028595") * num12 * num14 + double.Parse("0.003327") * num12 * num15 + double.Parse("0.006365") * num13 * num14 + double.Parse("0.014414") * num13 * num15 + double.Parse("0.052277") * num14 * num15);
            numArray3[7] = numArray2[7] * (double.Parse("0.229139") - double.Parse("0.04943") * num12 - double.Parse("0.005106") * num13 + double.Parse("0.038833") * num14 - double.Parse("0.079926") * num15 + double.Parse("0.10034") * num12 * num12 - double.Parse("0.001251") * num13 * num13 + double.Parse("0.002882") * num14 * num14 - double.Parse("0.062055") * num15 * num15 - double.Parse("0.027398") * num12 * num13 + double.Parse("0.011251") * num12 * num14 - double.Parse("0.013299") * num12 * num15 + double.Parse("0.009892") * num13 * num14 + double.Parse("0.006774") * num13 * num15 + double.Parse("0.052234") * num14 * num15);
            numArray3[8] = numArray2[8] * (double.Parse("0.189993") - double.Parse("0.161041") * num12 - double.Parse("0.010419") * num13 + double.Parse("0.037259") * num14 - double.Parse("0.088346") * num15 - double.Parse("0.00172") * num12 * num12 + double.Parse("0.001801") * num13 * num13 + double.Parse("0.000522") * num14 * num14 - double.Parse("0.059061") * num15 * num15 - double.Parse("0.007702") * num12 * num13 + double.Parse("0.049029") * num12 * num14 - double.Parse("0.075455") * num12 * num15 + double.Parse("0.011599") * num13 * num14 + double.Parse("0.004558") * num13 * num15 + double.Parse("0.04975") * num14 * num15);
            numArray3[9] = numArray2[9] * (double.Parse("0.160086") - double.Parse("0.217281") * num12 - double.Parse("0.017121") * num13 + double.Parse("0.036687") * num14 - double.Parse("0.116599") * num15 + double.Parse("0.095436") * num12 * num12 - double.Parse("0.000802") * num13 * num13 - double.Parse("0.003654") * num14 * num14 - double.Parse("0.066166") * num15 * num15 + double.Parse("0.026926") * num12 * num13 + double.Parse("0.046008") * num12 * num14 - double.Parse("0.100392") * num12 * num15 + double.Parse("0.01371") * num13 * num14 + double.Parse("0.000813") * num13 * num15 + double.Parse("0.047239") * num14 * num15);
            numArray3[10] = numArray2[10] * (2.0 * numArray3[9] / numArray2[9] - numArray3[8] / numArray2[8]);
            peakld = Math.Exp(double.Parse("4.366247") + double.Parse("3.740587") * num12 + double.Parse("0.433377") * num13 + double.Parse("0.350224") * num14 + double.Parse("0.085641") * num15 - double.Parse("3.027345") * num12 * num12 - double.Parse("0.11474") * num13 * num13 - double.Parse("0.039947") * num14 * num14 + double.Parse("0.051843") * num15 * num15 - double.Parse("0.13102") * num12 * num13 + double.Parse("0.094794") * num12 * num14 + double.Parse("0.116543") * num12 * num15 - double.Parse("0.007454") * num13 * num14 - double.Parse("0.013298") * num13 * num15 - double.Parse("0.099503") * num14 * num15 + double.Parse("0.008318") * num15 * num15 * num15 + double.Parse("0.044045") * num14 * num14 * num15 - double.Parse("0.022015") * num14 * num15 * num15 - double.Parse("0.187397") * num12 * num13 * num15 + double.Parse("0.102058") * num12 * num14 * num15);
            double num16 = double.Parse("54.807929") + double.Parse("163.619243") * num12 + double.Parse("15.998347") * num13 + double.Parse("2.568738") * num14 + double.Parse("13.968088") * num15 + double.Parse("53.781422") * num12 * num12 - 1.993944 * num13 * num13 + double.Parse("1.058319") * num14 * num14 + double.Parse("5.637651") * num15 * num15 + double.Parse("37.888375") * num12 * num13 - double.Parse("0.422129") * num12 * num14 + double.Parse("12.143284") * num12 * num15 + double.Parse("0.319604") * num13 * num14 + double.Parse("0.75981") * num13 * num15 - double.Parse("11.219268") * num14 * num15;
            if (Strings.Len(Conversions.ToString(num16)) == 0)
                num16 = double.Parse("0.0");
            double num17 = double.Parse("1.271147") + double.Parse("1.124717") * num12 + double.Parse("0.113626") * num13 + double.Parse("0.184286") * num14 - double.Parse("0.631231") * num12 * num12 + double.Parse("0.015319") * num13 * num13 + double.Parse("0.002371") * num14 * num14 + double.Parse("0.017226") * num15 * num15 + double.Parse("0.194801") * num12 * num13 + double.Parse("0.187503") * num12 * num14 - double.Parse("0.12826") * num12 * num15 - double.Parse("0.060003") * num13 * num14 + double.Parse("0.000307") * num13 * num15 + double.Parse("0.044967") * num14 * num15;
            double num18;
            if (numArray3[2] > num16)
            {
                double num19 = num16 + numArray2[2] - numArray3[2];
                num18 = (num19 - Math.Sqrt(num19 * num19 - double.Parse("4.0") * peakld * (num16 - numArray3[2]))) / (num16 - numArray3[2]);
                if (double.Parse("1.9") < num18)
                    num18 = double.Parse("1.9");
            }
            else
                num18 = double.Parse("1.9");
            if (num18 < num17)
                num17 = num18;
            double num20 = num16;
            double num21 = (peakld - num16 * num17) / (num17 * num17);
            numArray3[1] = (double.Parse("4.0") * (numArray2[2] - peakld) - (double.Parse("4.0") - num17 * num17) * numArray3[2]) / ((double.Parse("2.0") - num17) * (double.Parse("2.0") - num17));
            numArray4[1] = (numArray3[2] - (numArray2[2] - peakld) / (double.Parse("2.0") - num17)) / (double.Parse("2.0") - num17);
            numArray2[1] = numArray2[2] - 2.0 * numArray3[1] - double.Parse("4.0") * numArray4[1];
            int index1 = 2;
            do
            {
                numArray4[index1] = -numArray3[checked(index1 + 1)] - 2.0 * numArray3[index1] + 3.0 * (numArray2[checked(index1 + 1)] - numArray2[index1]);
                numArray5[index1] = numArray3[checked(index1 + 1)] + numArray3[index1] + 2.0 * (numArray2[index1] - numArray2[checked(index1 + 1)]);
                checked { ++index1; }
            }
            while (index1 <= 9);
            double a = -1.0;
            int num22 = 0;
            int index2 = 1;
            do
            {
                numArray6[index2] = 0.05f * (float)checked(index2 - 1);
                if (index2 % 20 == 1)
                    ++a;
                numArray7[index2] = (double)numArray6[index2] > num17 ? (a >= 2.0 ? (float)(numArray2[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * (numArray3[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * (numArray4[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * numArray5[checked((int)Math.Round(a))]))) : (float)(numArray2[1] + (double)numArray6[index2] * (numArray3[1] + (double)numArray6[index2] * numArray4[1]))) : numArray6[index2] * (float)(num20 + num21 * (double)numArray6[index2]);

                checked { ++num22; }
                if ((double)numArray7[index2] < double.Parse("400.0"))
                    checked { ++index2; }
                else
                    break;
            }
            while (index2 <= 201);
            int index3 = 1;
            do
            {
                checked { ++num1; }
                string str1 = numArray6[index3].ToString();
                string str2 = Conversions.ToString(numArray7[index3]);
                if ((double)numArray7[index3] > 0.0)
                    //oilCanningReturn.chartList.Add(new Chart(Conversions.ToDouble(str1), Conversions.ToDouble(str2)));
                    chartList.Add(new Chart(Conversions.ToDouble(str1), Conversions.ToDouble(str2)));
                if ((double)numArray7[index3] < 400.0)
                    checked { ++index3; }
                else
                    break;
            }
            while (index3 <= 201);
            ocvar = numArray3[1] + num17 * (2.0 * numArray4[1]);
            //return oilCanning;

            oilCanningReturn.chartList = chartList;
            model.ocvar = Math.Round(ocvar, 2);
            model.peakld = Math.Round(peakld, 2);
            //model.BH210 = BH210;
            //model.DDQ = DDQ;
            model.BH210 = Math.Round(BH210, 3);
            model.DDQ = Math.Round(DDQ, 2);
            oilCanningReturn.oilcanning = model;

            if (chartList != null)
            {
                return new ReturnObject<VmOilCanningReturn>() { success = true, data = oilCanningReturn, validated = true };
            }
            else
            {
                return new ReturnObject<VmOilCanningReturn>() { success = false, data = oilCanningReturn, validated = true };
            }
        }

        [HttpPost]
        public ReturnObject<List<VmBulkOilCanningReturn>> CalculateBulkOilCanning(List<OilCanning> model)
        {
            List<VmBulkOilCanningReturn> oilCanningReturnList = new List<VmBulkOilCanningReturn>();

            //for each posted oil canning calculation...
            foreach (OilCanning oc in model)
            {
                //create a new VmOilCanningReturn object
                VmBulkOilCanningReturn oilCanningReturn = new VmBulkOilCanningReturn();
                //create a new list of chart objects
                List<Chart> chartList = new List<Chart>();

                double ocvar = oc.ocvar;
                double peakld = oc.peakld;

                double fvr = oc.fvr;
                double svr = oc.svr;
                double gaugeini = oc.gaugeini;
                double span = oc.span;
                double emaj = oc.emaj;
                double emin = oc.emin;

                double DDQ = oc.DDQ;
                double BH210 = oc.BH210;

                double loadMin = 0.0;
                double loadMax = 0.0;
                double DeflactionMin = 0.0;
                double DeflactionMax = 0.0;

                double[] numArray1 = new double[21];
                double[] numArray2 = new double[11];
                double[] numArray3 = new double[11];
                double[] numArray4 = new double[11];
                double[] numArray5 = new double[11];
                float[] numArray6 = new float[213];
                float[] numArray7 = new float[213];
                int num1 = 0;
                double x1 = Math.Log(1.0 + emaj / double.Parse("100.0"));
                double x2 = Math.Log(1.0 + emin / double.Parse("100.0"));
                double d = (x1 + x2) * -1.0;
                double x3 = gaugeini * Math.Exp(d);
                double x4 = double.Parse("1") / fvr * 10000.0;
                double x5 = double.Parse("1") / svr * 10000.0;
                double num2 = double.Parse("1.8");
                double num3 = (double.Parse("1") + num2) / Math.Sqrt(1.0 + 2.0 * num2);
                double num4 = double.Parse("2") * num2 / (1.0 + num2);
                double num5 = num3 * Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0) + num4 * x1 * x2);
                double num6 = double.Parse("-72.53232126") + x4 * double.Parse("15.59828914") + x5 * double.Parse("13.0651791") + x3 * double.Parse("213.1634827") + num5 * double.Parse("100") * double.Parse("-1.95848059") + Math.Pow(x4, 2.0) * double.Parse("-0.795970382") + Math.Pow(x5, 2.0) * double.Parse("-0.797708151") + Math.Pow(x3, 2.0) * double.Parse("-27.44855967") + Math.Pow(num5 * double.Parse("100"), 2.0) * double.Parse("0.098745276") + x4 * x5 * double.Parse("-0.364333318") + x4 * x3 * double.Parse("-21.50222199") + x5 * x3 * double.Parse("-18.23194649") + x4 * (num5 * double.Parse("100.0")) * double.Parse("0.137396826") + x5 * (num5 * double.Parse("100.0")) * double.Parse("-0.217857199") + x3 * (num5 * double.Parse("100.0")) * double.Parse("15.4962963");
                DDQ = num6;
                double num7 = double.Parse("1.36");
                double num8 = (double.Parse("1") + num7) / Math.Sqrt(1.0 + 2.0 * num7);
                double num9 = double.Parse("2") * num7 / (1.0 + num7);
                double num10 = num8 * Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0) + num9 * x1 * x2);
                double num11 = x4 * 4.236321106 - 28.68952216 + x5 * 9.637663376 + x3 * 149.0650957 + num10 * 100.0 * -2.694921243 + Math.Pow(x4, 2.0) * -0.927863682 + Math.Pow(x5, 2.0) * -1.783005598 + Math.Pow(x3, 2.0) * 130.2316872 + Math.Pow(num10 * 100.0, 2.0) * -2.233762661 + x4 * x5 * 0.071358337 + x4 * x3 * -0.229851852 + x5 * x3 * -5.062709231 + x4 * (num10 * 100.0) * -0.1787746 + x5 * (num10 * 100.0) * 0.103470248 + x3 * (num10 * 100.0) * 44.96068783;
                BH210 = num11;

                double num12 = x3 - double.Parse("0.7");
                double num13 = double.Parse("10000.0") / fvr - double.Parse("2.0");
                double num14 = double.Parse("10000.0") / svr - double.Parse("2.0");
                double num15 = double.Parse("1000.0") / span - double.Parse("4.0");
                numArray2[2] = Math.Exp(double.Parse("4.27941") + double.Parse("3.192017") * num12 + double.Parse("0.276748") * num13 + double.Parse("0.077946") * num14 + double.Parse("0.401123") * num15 - double.Parse("2.500264") * num12 * num12 + 0.0 * num13 * num13 + double.Parse("0.053851") * num14 * num14 + double.Parse("0.084306") * num15 * num15 + double.Parse("0.047732") * num12 * num13 + double.Parse("0.153347") * num12 * num14 - double.Parse("0.49623") * num12 * num15 + double.Parse("0.018811") * num13 * num14 + 0.0 * num13 * num15 - double.Parse("0.166958") * num14 * num15);
                numArray2[3] = Math.Exp(double.Parse("4.613227") + double.Parse("2.800366") * num12 + double.Parse("0.182534") * num13 - double.Parse("0.109508") * num14 + double.Parse("0.602971") * num15 - double.Parse("2.466414") * num12 * num12 - double.Parse("0.022599") * num13 * num13 + double.Parse("0.057154") * num14 * num14 + double.Parse("0.097518") * num15 * num15 + double.Parse("0.139263") * num12 * num13 + double.Parse("0.323791") * num12 * num14 - double.Parse("0.589021") * num12 * num15 + double.Parse("0.005832") * num13 * num14 - double.Parse("0.047279") * num13 * num15 - double.Parse("0.195309") * num14 * num15);
                numArray2[4] = Math.Exp(double.Parse("5.003404") + double.Parse("2.616944") * num12 + double.Parse("0.116201") * num13 - double.Parse("0.211101") * num14 + double.Parse("0.762237") * num15 - double.Parse("1.79095") * num12 * num12 - double.Parse("0.007297") * num13 * num13 + double.Parse("0.054589") * num14 * num14 + double.Parse("0.10057") * num15 * num15 + double.Parse("0.126543") * num12 * num13 + double.Parse("0.141616") * num12 * num14 - double.Parse("0.517168") * num12 * num15 - double.Parse("0.01239") * num13 * num14 - double.Parse("0.044924") * num13 * num15 - double.Parse("0.193162") * num14 * num15);
                numArray2[5] = Math.Exp(double.Parse("5.373838") + double.Parse("2.113913") * num12 + double.Parse("0.070318") * num13 - double.Parse("0.274353") * num14 + double.Parse("0.874416") * num15 - double.Parse("2.529494") * num12 * num12 - double.Parse("0.000384") * num13 * num13 + double.Parse("0.076426") * num14 * num14 + double.Parse("0.099986") * num15 * num15 + double.Parse("0.179211") * num12 * num13 + double.Parse("0.151922") * num12 * num14 - double.Parse("0.733827") * num12 * num15 - double.Parse("0.001944") * num13 * num14 - double.Parse("0.049825") * num13 * num15 - double.Parse("0.192579") * num14 * num15);
                numArray2[6] = Math.Exp(double.Parse("5.772223") + double.Parse("2.533498") * num12 + double.Parse("0.028164") * num13 - double.Parse("0.400536") * num14 + double.Parse("1.173905") * num15 - double.Parse("2.443019") * num12 * num12 + double.Parse("0.00487") * num13 * num13 + double.Parse("0.111013") * num14 * num14 + double.Parse("0.187753") * num15 * num15 + double.Parse("0.100495") * num12 * num13 + double.Parse("0.078791") * num12 * num14 - double.Parse("0.381425") * num12 * num15 + double.Parse("0.010735") * num13 * num14 - double.Parse("0.058385") * num13 * num15 - double.Parse("0.234094") * num14 * num15);
                numArray2[7] = Math.Exp(6.178011 + double.Parse("2.201") * num12 - double.Parse("0.023444") * num13 - double.Parse("0.513154") * num14 + double.Parse("1.392907") * num15 - double.Parse("2.184983") * num12 * num12 + double.Parse("0.012779") * num13 * num13 + double.Parse("0.131457") * num14 * num14 + double.Parse("0.225974") * num15 * num15 + double.Parse("0.050317") * num12 * num13 + double.Parse("0.124977") * num12 * num14 - double.Parse("0.575138") * num12 * num15 + double.Parse("0.019902") * num13 * num14 - double.Parse("0.077663") * num13 * num15 - double.Parse("0.270135") * num14 * num15);
                numArray2[8] = Math.Exp(6.471374 + double.Parse("2.025984") * num12 - double.Parse("0.074294") * num13 - double.Parse("0.552174") * num14 + double.Parse("1.420372") * num15 - double.Parse("3.159497") * num12 * num12 + double.Parse("0.017696") * num13 * num13 + double.Parse("0.143595") * num14 * num14 + double.Parse("0.19991") * num15 * num15 - double.Parse("0.000382") * num12 * num13 + double.Parse("0.120918") * num12 * num14 - double.Parse("0.658293") * num12 * num15 + double.Parse("0.032529") * num13 * num14 - double.Parse("0.097144") * num13 * num15 - double.Parse("0.262298") * num14 * num15);
                numArray2[9] = Math.Exp(double.Parse("6.955466") + double.Parse("1.987479") * num12 - double.Parse("0.161447") * num13 - double.Parse("0.665285") * num14 + double.Parse("1.758484") * num15 - double.Parse("2.392414") * num12 * num12 + double.Parse("0.016408") * num13 * num13 + double.Parse("0.149745") * num14 * num14 + double.Parse("0.276407") * num15 * num15 + double.Parse("0.061953") * num12 * num13 + double.Parse("0.229413") * num12 * num14 - double.Parse("0.640081") * num12 * num15 + double.Parse("0.055921") * num13 * num14 - double.Parse("0.134679") * num13 * num15 - double.Parse("0.296333") * num14 * num15);
                numArray2[10] = numArray2[9] * numArray2[9] / numArray2[8];
                numArray3[2] = numArray2[2] * (0.229131 - double.Parse("0.533769") * num12 - double.Parse("0.09538") * num13 - double.Parse("0.171593") * num14 + double.Parse("0.192254") * num15 + double.Parse("0.15805") * num12 * num12 + double.Parse("0.017829") * num14 * num14 - double.Parse("0.004949") * num15 * num15 - double.Parse("0.022224") * num12 * num13 - double.Parse("0.023265") * num12 * num14 - double.Parse("0.107278") * num12 * num15 - double.Parse("0.007157") * num13 * num14 + double.Parse("0.006589") * num14 * num15);
                numArray3[3] = numArray2[3] * (0.391165 - double.Parse("0.421133") * num12 - double.Parse("0.06414") * num13 - double.Parse("0.099538") * num14 + double.Parse("0.130754") * num15 + double.Parse("1.024189") * num12 * num12 - double.Parse("0.004901") * num13 * num13 + double.Parse("0.000543") * num14 * num14 - double.Parse("0.02751") * num15 * num15 + double.Parse("0.034047") * num12 * num13 + double.Parse("0.050731") * num12 * num14 - double.Parse("0.083898") * num12 * num15 - double.Parse("0.027176") * num13 * num14 + double.Parse("0.025477") * num13 * num15 + double.Parse("0.025167") * num14 * num15);
                numArray3[4] = numArray2[4] * (double.Parse("0.403497") - double.Parse("0.313428") * num12 - double.Parse("0.019231") * num13 - double.Parse("0.017605") * num14 + double.Parse("0.061348") * num15 - double.Parse("0.178529") * num12 * num12 + double.Parse("0.004678") * num13 * num13 + double.Parse("0.005024") * num14 * num14 - double.Parse("0.038606") * num15 * num15 - double.Parse("0.018657") * num12 * num13 - double.Parse("0.110628") * num12 * num14 - double.Parse("0.025661") * num12 * num15 - double.Parse("0.009588") * num13 * num14 + double.Parse("0.026129") * num13 * num15 + double.Parse("0.047928") * num14 * num15);
                numArray3[5] = numArray2[5] * (double.Parse("0.350108") - double.Parse("0.132292") * num12 - double.Parse("0.003085") * num13 + double.Parse("0.021036") * num14 + double.Parse("0.005417") * num15 - double.Parse("0.558879") * num12 * num12 + double.Parse("0.00585") * num13 * num13 + double.Parse("0.006212") * num14 * num14 - double.Parse("0.044316") * num15 * num15 - double.Parse("0.054108") * num12 * num13 - double.Parse("0.06248") * num12 * num14 + double.Parse("0.017229") * num12 * num15 + double.Parse("0.00384") * num13 * num14 + double.Parse("0.021152") * num13 * num15 + double.Parse("0.055015") * num14 * num15);
                numArray3[6] = numArray2[6] * (double.Parse("0.283007") - double.Parse("0.055599") * num12 - double.Parse("0.001509") * num13 + double.Parse("0.031342") * num14 - double.Parse("0.036797") * num15 - double.Parse("0.403868") * num12 * num12 + double.Parse("0.004872") * num13 * num13 + double.Parse("0.00918") * num14 * num14 - double.Parse("0.051491") * num15 * num15 - double.Parse("0.024149") * num12 * num13 - double.Parse("0.028595") * num12 * num14 + double.Parse("0.003327") * num12 * num15 + double.Parse("0.006365") * num13 * num14 + double.Parse("0.014414") * num13 * num15 + double.Parse("0.052277") * num14 * num15);
                numArray3[7] = numArray2[7] * (double.Parse("0.229139") - double.Parse("0.04943") * num12 - double.Parse("0.005106") * num13 + double.Parse("0.038833") * num14 - double.Parse("0.079926") * num15 + double.Parse("0.10034") * num12 * num12 - double.Parse("0.001251") * num13 * num13 + double.Parse("0.002882") * num14 * num14 - double.Parse("0.062055") * num15 * num15 - double.Parse("0.027398") * num12 * num13 + double.Parse("0.011251") * num12 * num14 - double.Parse("0.013299") * num12 * num15 + double.Parse("0.009892") * num13 * num14 + double.Parse("0.006774") * num13 * num15 + double.Parse("0.052234") * num14 * num15);
                numArray3[8] = numArray2[8] * (double.Parse("0.189993") - double.Parse("0.161041") * num12 - double.Parse("0.010419") * num13 + double.Parse("0.037259") * num14 - double.Parse("0.088346") * num15 - double.Parse("0.00172") * num12 * num12 + double.Parse("0.001801") * num13 * num13 + double.Parse("0.000522") * num14 * num14 - double.Parse("0.059061") * num15 * num15 - double.Parse("0.007702") * num12 * num13 + double.Parse("0.049029") * num12 * num14 - double.Parse("0.075455") * num12 * num15 + double.Parse("0.011599") * num13 * num14 + double.Parse("0.004558") * num13 * num15 + double.Parse("0.04975") * num14 * num15);
                numArray3[9] = numArray2[9] * (double.Parse("0.160086") - double.Parse("0.217281") * num12 - double.Parse("0.017121") * num13 + double.Parse("0.036687") * num14 - double.Parse("0.116599") * num15 + double.Parse("0.095436") * num12 * num12 - double.Parse("0.000802") * num13 * num13 - double.Parse("0.003654") * num14 * num14 - double.Parse("0.066166") * num15 * num15 + double.Parse("0.026926") * num12 * num13 + double.Parse("0.046008") * num12 * num14 - double.Parse("0.100392") * num12 * num15 + double.Parse("0.01371") * num13 * num14 + double.Parse("0.000813") * num13 * num15 + double.Parse("0.047239") * num14 * num15);
                numArray3[10] = numArray2[10] * (2.0 * numArray3[9] / numArray2[9] - numArray3[8] / numArray2[8]);
                peakld = Math.Exp(double.Parse("4.366247") + double.Parse("3.740587") * num12 + double.Parse("0.433377") * num13 + double.Parse("0.350224") * num14 + double.Parse("0.085641") * num15 - double.Parse("3.027345") * num12 * num12 - double.Parse("0.11474") * num13 * num13 - double.Parse("0.039947") * num14 * num14 + double.Parse("0.051843") * num15 * num15 - double.Parse("0.13102") * num12 * num13 + double.Parse("0.094794") * num12 * num14 + double.Parse("0.116543") * num12 * num15 - double.Parse("0.007454") * num13 * num14 - double.Parse("0.013298") * num13 * num15 - double.Parse("0.099503") * num14 * num15 + double.Parse("0.008318") * num15 * num15 * num15 + double.Parse("0.044045") * num14 * num14 * num15 - double.Parse("0.022015") * num14 * num15 * num15 - double.Parse("0.187397") * num12 * num13 * num15 + double.Parse("0.102058") * num12 * num14 * num15);
                double num16 = double.Parse("54.807929") + double.Parse("163.619243") * num12 + double.Parse("15.998347") * num13 + double.Parse("2.568738") * num14 + double.Parse("13.968088") * num15 + double.Parse("53.781422") * num12 * num12 - 1.993944 * num13 * num13 + double.Parse("1.058319") * num14 * num14 + double.Parse("5.637651") * num15 * num15 + double.Parse("37.888375") * num12 * num13 - double.Parse("0.422129") * num12 * num14 + double.Parse("12.143284") * num12 * num15 + double.Parse("0.319604") * num13 * num14 + double.Parse("0.75981") * num13 * num15 - double.Parse("11.219268") * num14 * num15;
                if (Strings.Len(Conversions.ToString(num16)) == 0)
                    num16 = double.Parse("0.0");
                double num17 = double.Parse("1.271147") + double.Parse("1.124717") * num12 + double.Parse("0.113626") * num13 + double.Parse("0.184286") * num14 - double.Parse("0.631231") * num12 * num12 + double.Parse("0.015319") * num13 * num13 + double.Parse("0.002371") * num14 * num14 + double.Parse("0.017226") * num15 * num15 + double.Parse("0.194801") * num12 * num13 + double.Parse("0.187503") * num12 * num14 - double.Parse("0.12826") * num12 * num15 - double.Parse("0.060003") * num13 * num14 + double.Parse("0.000307") * num13 * num15 + double.Parse("0.044967") * num14 * num15;
                double num18;
                if (numArray3[2] > num16)
                {
                    double num19 = num16 + numArray2[2] - numArray3[2];
                    num18 = (num19 - Math.Sqrt(num19 * num19 - double.Parse("4.0") * peakld * (num16 - numArray3[2]))) / (num16 - numArray3[2]);
                    if (double.Parse("1.9") < num18)
                        num18 = double.Parse("1.9");
                }
                else
                    num18 = double.Parse("1.9");
                if (num18 < num17)
                    num17 = num18;
                double num20 = num16;
                double num21 = (peakld - num16 * num17) / (num17 * num17);
                numArray3[1] = (double.Parse("4.0") * (numArray2[2] - peakld) - (double.Parse("4.0") - num17 * num17) * numArray3[2]) / ((double.Parse("2.0") - num17) * (double.Parse("2.0") - num17));
                numArray4[1] = (numArray3[2] - (numArray2[2] - peakld) / (double.Parse("2.0") - num17)) / (double.Parse("2.0") - num17);
                numArray2[1] = numArray2[2] - 2.0 * numArray3[1] - double.Parse("4.0") * numArray4[1];
                int index1 = 2;
                do
                {
                    numArray4[index1] = -numArray3[checked(index1 + 1)] - 2.0 * numArray3[index1] + 3.0 * (numArray2[checked(index1 + 1)] - numArray2[index1]);
                    numArray5[index1] = numArray3[checked(index1 + 1)] + numArray3[index1] + 2.0 * (numArray2[index1] - numArray2[checked(index1 + 1)]);
                    checked { ++index1; }
                }
                while (index1 <= 9);
                double a = -1.0;
                int num22 = 0;
                int index2 = 1;
                do
                {
                    numArray6[index2] = 0.05f * (float)checked(index2 - 1);
                    if (index2 % 20 == 1)
                        ++a;
                    numArray7[index2] = (double)numArray6[index2] > num17 ? (a >= 2.0 ? (float)(numArray2[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * (numArray3[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * (numArray4[checked((int)Math.Round(a))] + ((double)numArray6[index2] - a) * numArray5[checked((int)Math.Round(a))]))) : (float)(numArray2[1] + (double)numArray6[index2] * (numArray3[1] + (double)numArray6[index2] * numArray4[1]))) : numArray6[index2] * (float)(num20 + num21 * (double)numArray6[index2]);
                    checked { ++num22; }
                    if ((double)numArray7[index2] < double.Parse("400.0"))
                        checked { ++index2; }
                    else
                        break;
                }
                while (index2 <= 201);
                int index3 = 1;
                do
                {
                    checked { ++num1; }
                    string str1 = numArray6[index3].ToString();
                    string str2 = Conversions.ToString(numArray7[index3]);

                    if ((double)numArray7[index3] > 0.0)
                        chartList.Add(new Chart(Conversions.ToDouble(str1), Conversions.ToDouble(str2)));

                    if ((double)numArray7[index3] < 400.0)
                    {
                        if (Conversions.ToDouble(str2) > 90.0 & loadMax == 0.0)
                        {
                            DeflactionMax = Conversions.ToDouble(str1);
                            loadMax = Conversions.ToDouble(str2);
                            DeflactionMin = (double)numArray6[checked(index3 - 1)];
                            loadMin = (double)numArray7[checked(index3 - 1)];

                            //calculate deflection at 90 newtons and 100 newtons
                            oilCanningReturn.Deflection90 = Math.Round(DeflactionMin + (DeflactionMax - DeflactionMin) / (loadMax - loadMin) * (90.0 - loadMin), 2);
                            oilCanningReturn.Deflection100 = Math.Round(DeflactionMin + (DeflactionMax - DeflactionMin) / (loadMax - loadMin) * (100.0 - loadMin), 2);
                        }
                        checked { ++index3; }
                    }
                    else
                        break;
                }
                while (index3 <= 201);
                ocvar = numArray3[1] + num17 * (2.0 * numArray4[1]);

                //set this oil canning return's chartList object to chartList
                //oilCanningReturn.chartList = chartList;
                oc.ocvar = ocvar;
                oc.peakld = peakld;
                oc.BH210 = BH210;
                oc.DDQ = DDQ;

                oilCanningReturn.excelRowId = oc.excelRowId;
                oilCanningReturn.ocvar = ocvar;
                oilCanningReturn.peakld = Math.Round(peakld, 1);

                oilCanningReturn.fvr = oc.fvr;
                oilCanningReturn.svr = oc.svr;
                oilCanningReturn.gaugeini = oc.gaugeini;
                oilCanningReturn.span = oc.span;
                oilCanningReturn.emaj = oc.emaj;
                oilCanningReturn.emin = oc.emin;

                oilCanningReturn.BH210 = Math.Round(BH210, 1);
                oilCanningReturn.DDQ = Math.Round(DDQ, 1);

                //add oilCanningReturn to oilCanningReturnList
                oilCanningReturnList.Add(oilCanningReturn);
            }

            if (oilCanningReturnList != null)
            {
                return new ReturnObject<List<VmBulkOilCanningReturn>>() { success = true, data = oilCanningReturnList, validated = true };
            }
            else
            {
                return new ReturnObject<List<VmBulkOilCanningReturn>>() { success = false, data = oilCanningReturnList, validated = true };
            }
        }
    }
}
