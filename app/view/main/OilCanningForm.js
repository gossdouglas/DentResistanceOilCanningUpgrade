//Ext.QuickTips.init();
Ext.define('DentResistanceOilCanningUpgrade.view.main.OilCanningForm', {
    extend: 'Ext.form.Panel',
    xtype: 'oil-canning-form',
    border: false,
    controller: 'oil-canning-form-controller',
    scrollable: true,
    layout: {
        type: 'vbox',
        pack: 'center',
        align: 'center'
    },
    items:
        [
            //command panel
            {
                xtype: 'panel',
                title: 'Prediction of Oil Canning and Dent Resistance of Roof Panels',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 3,
                items:
                    [
                        //entry fields
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                pack: 'center',
                                align: 'middle'
                            },
                            items:
                                [
                                    {
                                        xtype: 'textfield',
                                        id: 'OcvarOcCalculate',
                                        name: 'ocvar',
                                        width: '5%',
                                        hidden: true,
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'PeakldrOcCalculate',
                                        name: 'peakld',
                                        width: '5%',
                                        hidden: true,
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'FvrOcCalculate',
                                        name: 'fvr',
                                        fieldLabel: 'Front View Radius (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        //3000-12000
                                        regex: /^(300[0-9]|30[1-9][0-9]|3[1-9][0-9]{2}|[4-9][0-9]{3}|1[01][0-9]{3}|12000)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'FVR must be a value between 3000 and 12000',
                                        value: 3000
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'SvrOcCalculate',
                                        name: 'svr',
                                        fieldLabel: 'Side View Radius (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        //3000-15000
                                        regex: /^(300[0-9]|30[1-9][0-9]|3[1-9][0-9]{2}|[4-9][0-9]{3}|1[0-4][0-9]{3}|15000)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'SVR must be a value between 3000 and 15000',
                                        value: 3000
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'ThicknessOcCalculate',
                                        name: 'gaugeini',
                                        fieldLabel: 'Thickness (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        //.55-.85
                                        regex: /^(0\.5[5-9]+|0\.6\d*|0\.7\d*|0\.8[0-5]*)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Thickness must be a value between 0.55 and 0.85. Leading zeroes are necessary.',
                                        value: 0.55
                                    },
                                    //span
                                    {
                                        xtype: 'textfield',
                                        id: 'SpanOcCalculate',
                                        name: 'span',
                                        fieldLabel: 'Length Between Bows (mm)',
                                        labelAlign: 'top',
                                        width: '13%',
                                        allowBlank: false,
                                        //150-525
                                        regex: /^(15[0-9]|1[6-9][0-9]|[2-4][0-9]{2}|5[01][0-9]|52[0-5])$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Length Between Bows must be a value between 150 and 525',
                                        value: '150'
                                    },
                                    //emaj
                                    {
                                        xtype: 'textfield',
                                        id: 'EmajOcCalculate',
                                        name: 'emaj',
                                        fieldLabel: 'Major Stretch (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        //0-2
                                        regex: /^(0|0\.0[1-9]+[1-9]*|0\.[1-9]+[1-9]*|1.[0-9]+[0-9]*|1|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Major stretch must be a value between 0 and 2. Leading zeroes are necessary.',
                                        value: 0.0
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'EminOcCalculate',
                                        name: 'emin',
                                        fieldLabel: 'Minor Stretch (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        //0-2
                                        regex: /^(0|0\.0[1-9]+[1-9]*|0\.[1-9]+[1-9]*|1.[0-9]+[0-9]*|1|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Minor stretch must be a value between 0 and 2. Leading zeroes are necessary.',
                                        value: 0.0
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'DdqOcCalculate',
                                        name: 'DDQ',
                                        fieldLabel: 'DDQ',
                                        labelAlign: 'top',
                                        width: '11%',
                                        msgTarget: 'side', // location of the error message
                                        editable: false
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'Bh210OcCalculate',
                                        name: 'BH210',
                                        fieldLabel: 'BH210',
                                        labelAlign: 'top',
                                        width: '11%',
                                        msgTarget: 'side', // location of the error message
                                        editable: false
                                    },
                                ]
                        },
                        //range notes
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                pack: 'center',
                                align: 'middle'
                            },
                            items:
                                [
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>3000 to 12000</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>3000 to 15000</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>.55 to .85</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '13%',
                                        html: '<center>150 to 525</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>0 to 2</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>0 to 2</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>Output Value</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>Output Value</center>',
                                    },
                                    
                                ]
                        },
                        //command buttons
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                pack: 'center',
                                align: 'middle'
                            },
                            items:
                                [
                                    //calculate button
                                    {
                                        xtype: 'button',
                                        text: "Calculate",
                                        //margin: top, right, bottom, left
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onCalculateOcClick'
                                        }
                                    },
                                    //download an excel template
                                    {
                                        xtype: 'button',
                                        text: "Download Excel Template",
                                        width: '15%',
                                        //margin: top, right, bottom, left
                                        margin: '20 10 0 0',
                                        listeners: {
                                            click: function (input, val, opts) {
                                                DownloadOcBulkTemplate();
                                            }
                                        }
                                    },
                                    //import values from an excel file
                                    {
                                        xtype: 'button',
                                        text: "Import File",
                                        //margin: top, right, bottom, left
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        disabled: true,
                                        listeners: {
                                            //click: 'onClearResultsClick'
                                        }
                                    },
                                    //export the calculation to a csv file
                                    {
                                        xtype: 'button',
                                        id: 'ExportOcCalculate',
                                        name: 'ExportOcCalculate',
                                        text: "Export to Excel",
                                        disabled: true,
                                        //margin: top, right, bottom, left
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        disabled: true,
                                        listeners: {
                                            click: function (input, val, opts) {

                                                oilCanning = [];
                                                tmpObject = {
                                                    ocvar: Ext.getCmp('OcvarOcCalculate').getValue(),
                                                    peakld: Ext.getCmp('PeakldrOcCalculate').getValue(),
                                                    fvr: Ext.getCmp('FvrOcCalculate').getValue(),
                                                    svr: Ext.getCmp('SvrOcCalculate').getValue(),
                                                    gaugeini: Ext.getCmp('ThicknessOcCalculate').getValue(),
                                                    span: Ext.getCmp('SpanOcCalculate').getValue(),
                                                    emaj: Ext.getCmp('EmajOcCalculate').getValue(),
                                                    emin: Ext.getCmp('EminOcCalculate').getValue(),
                                                    DDQ: Ext.getCmp('DdqOcCalculate').getValue(),
                                                    BH210: Ext.getCmp('Bh210OcCalculate').getValue()
                                                };

                                                oilCanning.push(tmpObject);
                                                //if export detailed is selected, load and deflection data is included in the .csv
                                                var exportDetailedValue = Ext.getCmp('ExportDetailedOcCalculate').getValue();
                                                //build a .csv
                                                OilCanningToCSVConvertor(oilCanning, getChartList(), "LoadDeflection_Report", exportDetailedValue);
                                            }
                                        }
                                    },
                                    //detailed export?
                                    {
                                        xtype: 'fieldcontainer',
                                        defaultType: 'checkboxfield',
                                        items: [
                                            {
                                                boxLabel: 'Export Detailed',
                                                id: 'ExportDetailedOcCalculate',
                                                value: true
                                            },
                                        ],
                                    }
                                ]
                        },
                    ]
            },
            //results panel
            {
                xtype: 'panel',
                layout: {
                    type: 'hbox',
                    pack: 'center',
                    align: 'middle'
                },
                title: 'Load Deflection Behavior -150 mm Flat Identer',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 10,
                items:
                    [
                        { xtype: 'oc-load-deflection' }
                    ]
            },
            //page footer
            {
                xtype: 'panel',
                width: '100%',
                bodyPadding: '5',
                items:
                    [
                        {
                            xtype: 'page-footer',
                            width: '100%',
                        },
                    ]
            },
        ],
});



