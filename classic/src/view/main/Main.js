/**
 * This class is the main view for the application. It is specified in app.js as the
 * "mainView" property. That setting automatically applies the "viewport"
 * plugin causing this view to become the body element (i.e., the viewport).
 *
 * TODO - Replace this content of this view to suite the needs of your application.
 */
Ext.define('DentResistanceOilCanningUpgrade.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [

        'DentResistanceOilCanningUpgrade.view.main.MainController',
        'DentResistanceOilCanningUpgrade.view.main.MainModel',

        'DentResistanceOilCanningUpgrade.view.main.PageFooter',

        //dent resistance model 1
        'DentResistanceOilCanningUpgrade.view.main.DentResistanceOverview',
        'DentResistanceOilCanningUpgrade.store.GradeStoreModel1',
        'DentResistanceOilCanningUpgrade.view.main.DrM1Form',
        'DentResistanceOilCanningUpgrade.view.main.DrM1FormCalcResults',
        'DentResistanceOilCanningUpgrade.view.main.DrM1FormController',
        //dent resistance model 2
        'DentResistanceOilCanningUpgrade.view.main.DrM2Form',
        'DentResistanceOilCanningUpgrade.store.GradeStoreModel2',
        'DentResistanceOilCanningUpgrade.view.main.DrM2FormCalcResults',
        'DentResistanceOilCanningUpgrade.view.main.DrM2FormController',
        //oil canning calculator
        'DentResistanceOilCanningUpgrade.view.main.OilCanningOverview',
        'DentResistanceOilCanningUpgrade.view.main.OilCanningForm',
        'DentResistanceOilCanningUpgrade.view.main.OcBulkInputForm',
        'DentResistanceOilCanningUpgrade.view.main.OilCanningFormController',
        //bulk oil canning calculator
        'DentResistanceOilCanningUpgrade.grid.OcBulkCalculationGrid',
        'DentResistanceOilCanningUpgrade.grid.OcBulkErrorGrid',
        'DentResistanceOilCanningUpgrade.store.OcBulkCalculationStore',
        'DentResistanceOilCanningUpgrade.store.OcBulkErrorStore',
    ],

    controller: 'main',
    viewModel: 'main',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,
    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            text: 'Dent Resistance/Oil Canning',
            flex: 0
        },
        iconCls: 'fa-th-list'
    },
    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },
    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'left'
        }
    },
    defaults: {
        bodyPadding: 20,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'left',
                    textAlign: 'left'
                },
                tall: {
                    iconAlign: 'top',
                    textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    activeTab: 0,

    items: [
        {
            xtype: 'panel',
            title: 'Dent Resistance Overview',
            iconCls: 'fa-home',
            scrollable: true,
            listeners: {
                show: function () {
                    //Ext.MessageBox.alert('Tab one', 'Tab One was clicked.');
                    //resetTab1();
                }
            },
            items: [{
                xtype: 'dent-resistance-overview'
            }],
        },
        {
            title: 'Oil Canning Overview',
            iconCls: 'fa-user',
            scrollable: true,

            listeners: {
                show: function () {
                    //Ext.MessageBox.alert('Tab two', 'Tab two was clicked.');
                    //resetTab2();
                }
            },
            items: [{
                xtype: 'oil-canning-overview'
            }],
        },
    ]
});

//show the dent resistance model 1 pop up
function showDRModel1() {

    var win = Ext.create('Ext.window.Window', {
        layout: 'fit',
        xtype: 'form',
        width: '100%',
        height: '100%',
        id: 'model1FormWindow',
        items: {
            xtype: 'dr-model1-form'
        },
        listeners: {

        }
    }).show()
}

//show the dent resistance model 2 pop up
function showDRModel2() {
    var win = Ext.create('Ext.window.Window', {
        layout: 'fit',
        xtype: 'form',
        width: '100%',
        height: '100%',
        id: 'model1FormWindow',
        items: {
            xtype: 'dr-model2-form'
        },
        listeners: {

        }
    }).show()
}

//show the oil canning calculation pop up
function showOcCalculator() {

    var win = Ext.create('Ext.window.Window', {
        layout: 'fit',
        xtype: 'form',
        width: '100%',
        height: '100%',
        id: 'oilCanningFormWindow',
        items: {
            xtype: 'oil-canning-form'
        },
        listeners: {

        }
    }).show()

    //var OpenWindow = window.open('OilCanning/OilCanningCalculator.html');
}

//show the bulk oil canning calculation pop up
function showOcBulkInput() {

    var win = Ext.create('Ext.window.Window', {
        layout: 'fit',
        xtype: 'form',
        width: '100%',
        height: '100%',
        id: 'oilCanningBulkInputFormWindow',
        items: {
            xtype: 'oil-canning-bulk-input-form'
        },
        listeners: {

        }
    }).show()
}

//ExportToTable variables
//holds whether an excel row contains data that is out of range
var excelRowAllValid;
var objExcelErrors = [];
var bulkOilCanningList = [];

//fill the bulkOilCanningList array with valid bulk oil canning calculations
function setBulkOilCanningList(bulkOilCanningListData) {

    bulkOilCanningList = bulkOilCanningListData;
    //console.log("bulkOilCanningList");
    //console.log(bulkOilCanningList);
}

//retrieve the bulkOilCanningList array
function getBulkOilCanningList() {

    return bulkOilCanningList;
}

//read the rows of an Excel file in order to carry out several oil canning calculations in one operation
function ExportToTable() {
    /*Checks whether the file is a valid excel file*/
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;

    console.clear();
    //console.log("ExportToTable Entry.")
    //console.log($("#excelfile"));
    //console.log($("#excelfile").val().toLowerCase());

    if (regex.test($("#excelfile").val().toLowerCase())) {
        var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
        if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {
            xlsxflag = true;
        }
        /*Checks whether the browser supports HTML5*/
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            //array which will hold json objects
            var objExcelJson = [];
            //clear the excel errors array
            objExcelErrors.length = 0;
            var excelRowId = 1;

            var fvrMin = 3000;
            var fvrMax = 12000;
            var svrMin = 3000;
            var svrMax = 15000;
            var gaugeiniMin = .55;
            var gaugeiniMax = .85;
            var spanMin = 150;
            var spanMax = 525;
            var emajMin = 0;
            var emajMax = 2;
            var eminMin = 0;
            var eminMax = 2;

            reader.onload = function (e) {
                var data = e.target.result;
                /*Converts the excel data in to object*/
                if (xlsxflag) {
                    var workbook = XLSX.read(data, { type: 'binary' });
                }
                else {
                    var workbook = XLS.read(data, { type: 'binary' });
                }
                /*Gets all the sheetnames of excel in to a variable*/
                var sheet_name_list = workbook.SheetNames;

                //console.log("sheet_name_list: " + sheet_name_list);
                //console.log("sheet_name_list length: " + sheet_name_list.length);

                var sheetNumber = 0; /*This is used for restricting the script to consider only first sheet of excel*/
                sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/

                    //break out of the forEach loop if not on the first sheet of the workbook
                    //console.log("sheetNumber: " + sheetNumber);
                    if (sheetNumber != 0) {
                        return false;
                    }

                    //Convert the cell value to Json
                    if (xlsxflag) {//if the file is an xlsx type...
                        //convert the contents of an excel sheet into an object
                        var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                        //object which will hold a single line of excel data in json format
                        var tmpObject;

                        //for each excel file row...
                        $(exceljson).each(function (index) {

                            //console.log("**************************");
                            //console.log("Processing row " + excelRowNumber + " of the uploaded Excel file...");

                            //set that the values in this excel row are all within range
                            //the rows will be tested below for validity
                            excelRowAllValid = true;

                            //create a tmpObject
                            tmpObject = {
                                excelRowId: excelRowId,
                                ocvar: "",
                                peakld: "",
                                fvr: validateBulkOilCanningExcelRow(fvrMin, fvrMax, Object.values(exceljson[index])[0], "FVR", excelRowId),
                                svr: validateBulkOilCanningExcelRow(svrMin, svrMax, Object.values(exceljson[index])[1], "SVR", excelRowId),
                                gaugeini: validateBulkOilCanningExcelRow(gaugeiniMin, gaugeiniMax, Object.values(exceljson[index])[2], "GAUGEINI", excelRowId),
                                span: validateBulkOilCanningExcelRow(spanMin, spanMax, Object.values(exceljson[index])[3], "SPAN", excelRowId),
                                emaj: validateBulkOilCanningExcelRow(emajMin, emajMax, Object.values(exceljson[index])[4], "EMAJ", excelRowId),
                                emin: validateBulkOilCanningExcelRow(eminMin, eminMax, Object.values(exceljson[index])[5], "EMIN", excelRowId),
                                DDQ: "",
                                BH210: ""
                            };

                            //push tmpObject to objExcelJson if the contents of this row are valid
                            if (excelRowAllValid) {
                                objExcelJson.push(tmpObject);
                            }
                            //increment the excel row number
                            excelRowId++;
                        });

                        sheetNumber++;
                        //console.log("objExcelJson xlsxflag");
                        //console.log(objExcelJson);
                    }
                    else {//if the file is not an xlsx type...
                        //convert the contents of an excel sheet into an object
                        var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                        //object which will hold a single line of excel data in json format
                        var tmpObject;

                        $(exceljson).each(function (index) {

                            console.log("**************************");
                            //console.log("Processing row " + excelRowNumber + " of the uploaded Excel file...");
                            //set that the values in this excel row are within range
                            excelRowAllValid = true;

                            //create a tmpObject
                            tmpObject = {
                                excelRowId: excelRowId,
                                ocvar: "",
                                peakld: "",
                                fvr: validateBulkOilCanningExcelRow(fvrMin, fvrMax, Object.values(exceljson[index])[0], "FVR", excelRowId),
                                svr: validateBulkOilCanningExcelRow(svrMin, svrMax, Object.values(exceljson[index])[1], "SVR", excelRowId),
                                gaugeini: validateBulkOilCanningExcelRow(gaugeiniMin, gaugeiniMax, Object.values(exceljson[index])[2], "GAUGEINI", excelRowId),
                                span: validateBulkOilCanningExcelRow(spanMin, spanMax, Object.values(exceljson[index])[3], "SPAN", excelRowId),
                                emaj: validateBulkOilCanningExcelRow(emajMin, emajMax, Object.values(exceljson[index])[4], "EMAJ", excelRowId),
                                emin: validateBulkOilCanningExcelRow(eminMin, eminMax, Object.values(exceljson[index])[5], "EMIN", excelRowId),
                                DDQ: "",
                                BH210: ""
                            };

                            //push tmpObject to objExcelJson
                            if (excelRowAllValid) {
                                objExcelJson.push(tmpObject);
                            }
                            //increment the excel row number
                            excelRowId++;
                        });

                        sheetNumber++;
                        //console.log("objExcelJson non xlsxflag");
                        //console.log(objExcelJson);
                    }
                });

                //console.log("post to the back end.");
                //console.log(objExcelJson);
                //console.log("objExcelErrors");
                //console.log(objExcelErrors);

                //post to the back end
                Ext.Ajax.request({
                    url: 'api/OilCanning/CalculateBulkOilCanning',
                    method: 'POST',
                    jsonData: JSON.stringify(objExcelJson),
                    async: false,
                    success: function (response, opts) {
                        var resp = Ext.decode(response.responseText);

                        //print the response from the server
                        if (resp.success) {
                            //console.log("resp");
                            //console.log(resp);
                            console.log("resp.data");
                            console.log(resp.data);

                            //if the ocvar calculation is greater than zero, no oil canning is present so 
                            //replace the numerical value of peakld with a string for display
                            $(resp.data).each(function (index) {
                                if (resp.data[index].ocvar > 0) {
                                    resp.data[index].peakld = 'No oil canning < 400 (N)';
                                }
                            });

                            //link up to the bulk oil canning store
                            var store = Ext.data.StoreManager.lookup('OcBulkCalculationStore');
                            //clear that store of past data
                            store.removeAll();
                            //load that store with current data
                            store.add(resp.data);

                            //link up to the bulk oil canning error store
                            var store = Ext.data.StoreManager.lookup('OcBulkErrorStore');
                            //clear that store of past data
                            store.removeAll();
                            //load that store with current data
                            store.add(objExcelErrors);

                            //load the bulk oil canning results array with valid data for later retrieval by an export to Excel request
                            setBulkOilCanningList(resp.data);

                            //console.log("resp.data.length");
                            //console.log(resp.data.length);

                            //if the is bulk oil canning data that can be exported to Excel then enable the export button
                            if (resp.data.length != 0) {
                                Ext.getCmp('ExportBulkOilCanning').setDisabled(false);
                            }
                            //if the is not bulk oil canning data that can be exported to Excel then disable the export button
                            else {
                                Ext.getCmp('ExportBulkOilCanning').setDisabled(true);
                            }
                            
                        }
                        //print the response from the server
                        else {
                            console.log(resp);
                        }
                    },
                    //print the response from the server
                    failure: function (response, opts) {
                        var resp = response;
                        console.log(resp);
                    }
                });
            }
            if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                reader.readAsArrayBuffer($("#excelfile")[0].files[0]);
            }
            else {
                reader.readAsBinaryString($("#excelfile")[0].files[0]);
            }
        }
        else {
            alert("Sorry! Your browser does not support HTML5!");
        }
    }
    else {
        alert("Please upload a valid Excel file!");
    }
}

//validate an Excel value.  invalid values are pushed onto the objExcelErrors array
function validateBulkOilCanningExcelRow(minValue, maxValue, evaluatedValue, evaluatedName, excelRowId) {

    //holds a description of the range error
    var strErrorText;

    //if the value is within range, simply return the value
    if (evaluatedValue >= minValue && evaluatedValue <= maxValue) {

        //console.log("Row " + excelRowId + " of the uploaded Excel file...");
        //console.log(evaluatedName + " with a value of " + evaluatedValue + " is within " + minValue + " and " + maxValue)
        return evaluatedValue;
    }
    //if the value is not within range, build a description of the error along with the Excel row id
    else {

        //console.log("Row " + excelRowId + " of the uploaded Excel file...");
        //console.log(evaluatedName + " with a value of " + evaluatedValue + " is not within " + minValue + " and " + maxValue);

        strErrorText = evaluatedName + " with a value of " + evaluatedValue + " is not within " + minValue + " and " + maxValue;
        excelRowAllValid = false;

        tmpObject = {
            excelRowId: excelRowId,
            errorText: strErrorText,
        };

        objExcelErrors.push(tmpObject);

        return evaluatedValue;
    }
}

//download a template for bulk oil canning processing
function DownloadOcBulkTemplate() {
    location.href = '/Resources/OilCanningBulkInputTemplate.xlsx'
}

var chartList = [];
function setChartList(chartListData) {

    chartList = chartListData;
    //console.log("chartList");
    //console.log(chartList);
}

function getChartList() {

    return chartList;
}

//write oil canning results to a .csv
function OilCanningToCSVConvertor(oilCanningData, JSONData, ReportTitle, exportDetailed) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    //console.log("oilCanningData");
    //console.log(oilCanningData);

    var CSV = '';
    //Set Report title in first row or line

    currentDateTime = new Date().toLocaleString();
    CSV += 'Oil Canning Results,Ran On: ' + currentDateTime + '\r\n';
    CSV += 'Front View Radius (mm),Side View Radius (mm),Thickness (mm),Free Span Between Bows (mm),Major Stretch (%),Minor Stretch (%)\r\n';
    CSV += oilCanningData[0].fvr + ',' + oilCanningData[0].svr + ',' + oilCanningData[0].gaugeini + ',' + oilCanningData[0].span + ',' + oilCanningData[0].emaj + ',' + oilCanningData[0].emin + '\r\n';

    //if a detailed report is requested, include the deflection and load data points
    if (exportDetailed) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';

        //1st loop is to extract each row
        for (var i = 0; i < arrData.length; i++) {
            var row = "";

            //2nd loop will extract each column and convert it in string comma-seprated
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
                console.log(arrData[i][index]);
            }

            row.slice(0, row.length - 1);

            //add a line break after each row
            CSV += row + '\r\n';
        }
    }

    if (oilCanningData[0].ocvar > 0) {
        oilCanningData[0].peakld = 'No Oil canning < 400 N'
    }

    //console.log("ocvar: " + oilCanningData[0].ocvar);
    //console.log("peakld: " + oilCanningData[0].peakld);

    CSV += 'Oil Canning Load,0.1mm dent load (DDQ+),0.1mm dent load (BH210)\r\n';
    CSV += oilCanningData[0].peakld + ',' + oilCanningData[0].DDQ + ',' + oilCanningData[0].BH210, + '\r\n';

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

//write bulk oil canning results to a .csv
function BulkOilCanningToCSVConvertor(JSONData, ReportTitle, exportDetailed) {
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    console.log("JSONData");
    console.log(JSONData);

    var CSV = '';
    //Set Report title in first row or line

    currentDateTime = new Date().toLocaleString();
    CSV += 'Bulk Oil Canning Results, Ran On: ' + currentDateTime + '\r\n';
    CSV += 'Excel Row Id, Front View Radius (mm), Side View Radius (mm), Thickness (mm), Free Span Between Bows (mm), Major Stretch (%), Minor Stretch (%), Oil Canning Load (N), Dentload_DDQ (N), Dentload_BH210 (N), Deflection @ 90N (mm), Deflection @ 100N (mm) \r\n';

    //if a detailed report is requested, include the deflection and load data points
    if (exportDetailed) {
        var row = "";
        row = row.slice(0, -1);

        //1st loop is to extract each row
        for (var i = 0; i < arrData.length; i++) {
            var row = "";
            //console.log("JSONData" + i)
            //console.log(JSONData[i])

            row += JSONData[i].excelRowId + ',' + JSONData[i].fvr + ',' + JSONData[i].svr + ',' + JSONData[i].gaugeini + ',' +
                JSONData[i].span + ',' + JSONData[i].emaj + ',' + JSONData[i].emin + ',' + JSONData[i].peakld + ',' +
                JSONData[i].DDQ + ',' + JSONData[i].BH210 + ',' + JSONData[i].Deflection90 + ',' + JSONData[i].Deflection100 + ',';

            //console.log("row:");
            //console.log(row);

            row.slice(0, row.length - 1);

            //add a line break after each row
            CSV += row + '\r\n';
        }
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
