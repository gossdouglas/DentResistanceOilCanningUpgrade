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
        'DentResistanceOilCanningUpgrade.view.main.OcBulkInputFormController',
        //bulk oil canning calculator
        'DentResistanceOilCanningUpgrade.grid.OcBulkCalculationGrid',
        'DentResistanceOilCanningUpgrade.grid.OcBulkErrorGrid',
        'DentResistanceOilCanningUpgrade.view.main.OcBulkCalculationController',
        'DentResistanceOilCanningUpgrade.store.OcBulkCalculationStore',
        'DentResistanceOilCanningUpgrade.store.OcBulkErrorStore',

        //'DentResistanceOilCanning.view.charts.line.OcLoadDeflection',
        //'DentResistanceOilCanning.store.OcLoadDeflectionStore',
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
        //{
        //    title: 'Load Deflection',
        //    iconCls: 'fa-user',
        //    scrollable: true,

        //    listeners: {
        //        show: function () {
        //            //Ext.MessageBox.alert('Tab two', 'Tab two was clicked.');
        //            //resetTab2();
        //        }
        //    },
        //    items: [{
        //        xtype: 'oc-load-deflection'
        //    }],
        //},
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

//show the dent resistance model 2 pop up
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

//show the bulk oil canning pop up
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
                    if (xlsxflag) {
                        //convert the contents of an excel sheet into an object
                        var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                        //object which will hold a single line of excel data in json format
                        var tmpObject;

                        //for each excel file row...
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

                            //validate that the values of this excel row are within range
                            //validateBulkOilCanningExcelRow(tmpObject);

                            //push tmpObject to objExcelJson
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
                    else {
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

                            //validate that the values of this excel row are within range
                            //validateBulkOilCanningExcelRow(tmpObject);

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
                            //console.log("resp.data");
                            //console.log(resp.data);

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

//validate an Excel value
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

$(document).ready(function () {

});


///**
// * This class is the main view for the application. It is specified in app.js as the
// * "mainView" property. That setting automatically applies the "viewport"
// * plugin causing this view to become the body element (i.e., the viewport).
// *
// * TODO - Replace this content of this view to suite the needs of your application.
// */
//Ext.define('DentResistanceOilCanningUpgrade.view.main.Main', {
//    extend: 'Ext.tab.Panel',
//    xtype: 'app-main',

//    requires: [
//        'Ext.plugin.Viewport',
//        'Ext.window.MessageBox',

//        'DentResistanceOilCanningUpgrade.view.main.MainController',
//        'DentResistanceOilCanningUpgrade.view.main.MainModel',
//        'DentResistanceOilCanningUpgrade.view.main.List',

//        'DentResistanceOilCanning.view.main.PageFooter',

//        'DentResistanceOilCanningUpgrade.view.charts.line.OcLoadDeflection',
//        'DentResistanceOilCanningUpgrade.store.OcLoadDeflectionStore',
//    ],

//    controller: 'main',
//    viewModel: 'main',

//    ui: 'navigation',

//    tabBarHeaderPosition: 1,
//    titleRotation: 0,
//    tabRotation: 0,

//    header: {
//        layout: {
//            align: 'stretchmax'
//        },
//        title: {
//            bind: {
//                text: '{name}'
//            },
//            flex: 0
//        },
//        iconCls: 'fa-th-list'
//    },

//    tabBar: {
//        flex: 1,
//        layout: {
//            align: 'stretch',
//            overflowHandler: 'none'
//        }
//    },

//    responsiveConfig: {
//        tall: {
//            headerPosition: 'top'
//        },
//        wide: {
//            headerPosition: 'left'
//        }
//    },

//    defaults: {
//        bodyPadding: 20,
//        tabConfig: {
//            responsiveConfig: {
//                wide: {
//                    iconAlign: 'left',
//                    textAlign: 'left'
//                },
//                tall: {
//                    iconAlign: 'top',
//                    textAlign: 'center',
//                    width: 120
//                }
//            }
//        }
//    },

//    items: [{
//        title: 'Home',
//        iconCls: 'fa-home',
//        // The following grid shares a store with the classic version's grid as well!
//        items: [{
//            xtype: 'mainlist'
//        }]
//    }, {
//        title: 'Users',
//        iconCls: 'fa-user',
//        //bind: {
//        //    html: '{loremIpsum}'
//        //}
//        items: [{
//            //xtype: 'line-spline'
//        }]
//    }, {
//        title: 'Load Deflection',
//        iconCls: 'fa-users',
//        //bind: {
//        //    html: '{loremIpsum}'
//        //}
//        items: [{
//            xtype: 'oc-load-deflection'
//        }]
//    }, {
//        title: 'Settings',
//        iconCls: 'fa-cog',
//        bind: {
//            html: '{loremIpsum}'
//        }
//    }]
//});
