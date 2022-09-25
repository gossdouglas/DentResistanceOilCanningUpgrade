//Ext.QuickTips.init();
Ext.define('DentResistanceOilCanningUpgrade.view.main.DrM2Form', {
    extend: 'Ext.form.Panel',
    xtype: 'dr-model2-form',
    border: false,
    controller: 'dr-model2-form-controller',
    scrollable: true,
    url: 'api/DentResistanc/CalculateModelTwo',

    items:
        [
            //command panel
            {
                xtype: 'panel',
                title: 'Dent Depth for a Given Applied Load, Model 2',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 1,
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
                                        id: 'GradeKey',
                                        name: 'GradeKey',
                                        bind: '{gradeKeyDrM2Combo.value}',
                                        hidden: true,
                                        readOnly: true,
                                        allowBlank: false,
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'GradeName',
                                        name: 'GradeName',
                                        hidden: true,
                                        readOnly: false,
                                        allowBlank: false,
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: '11%',
                                        id: 'GradeKeyDrM2Combo',
                                        reference: 'gradeKeyDrM2Combo',
                                        publishes: 'value',
                                        fieldLabel: 'Grade',
                                        labelAlign: 'top',
                                        displayField: 'grade_name',
                                        valueField: 'grade_key',
                                        allowBlank: false,
                                        store: {
                                            type: 'GradeStoreModel2',
                                            autoLoad: true
                                        },
                                        queryMode: 'local',
                                        allowBlank: false,
                                        msgTarget: 'side', // location of the error message
                                        listeners: {
                                            select: function (combo, record) {

                                                //console.log(this.selection);
                                                //console.log(this.selection.data.grade_name);
                                                Ext.getCmp('GradeName').setValue(record.get('grade_name'));

                                                //Ext.getCmp('AmnsDepartmentIdentifier').setValue(record.get('DepartmentIdentifier'));
                                                //Ext.getCmp('SubDepartmentCombo').setValue('');
                                                //Ext.getCmp('SubDepartmentCombo').getStore().load(
                                                //    {
                                                //        params:
                                                //        {
                                                //            command: 2,
                                                //            departmentIdentifier: record.get('DepartmentIdentifier'),
                                                //        }
                                                //    });
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'R1DrM2',
                                        name: 'R1',
                                        fieldLabel: 'R1 (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(15[0-9]|1[6-9][0-9]|[2-9][0-9]{2}|1[0-9]{3}|2[0-4][0-9]{2}|2500)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'R1 must be a value between 150 and 2500',
                                        value: 150
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'R2DrM2',
                                        name: 'R2',
                                        fieldLabel: 'R2 (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(1500[0-9]|150[1-9][0-9]|15[1-9][0-9]{2}|1[6-9][0-9]{3}|[2-9][0-9]{4}|1[0-4][0-9]{4}|150000)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'R1 must be a value between 15000 and 15000',
                                        value: 15000
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'MajorStrainDrM2',
                                        name: 'MajorStrain',
                                        fieldLabel: 'Major Strain (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(0\.2[5-9]+|0\.[3-9]+|1|1\.\d+|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Major strain must be a value between 0.25 and 2. Leading zeroes are necessary.',
                                        value: 0.25
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'MinorStrainDrM2',
                                        name: 'MinorStrain',
                                        fieldLabel: 'Minor Strain (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(0\.2[5-9]+|0\.[3-9]+|1|1\.\d+|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Minor strain must be a value between 0.25 and 2. Leading zeroes are necessary.',
                                        value: 0.25
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'ThicknessDrM2',
                                        name: 'Thickness',
                                        fieldLabel: 'Thickness (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(0\.6[5-9]+|0\.7\d*|0\.8)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Thickness must be a value between 0.65 and 0.8. Leading zeroes are necessary.',
                                        value: 0.65
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'PoundsForceDrM2',
                                        name: 'PoundsForce',
                                        fieldLabel: 'Force (N)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(\.\d+|\d+\.\d+|\d+)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Pounds of force must be greater than 0.',
                                        value: '1'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'Dent Depth (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        value: 'N/A',
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
                                        xtype: 'label',
                                        text: '',
                                        width: '11%',
                                        html: '<center>Select a grade</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>150 to 2500</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>15000 to 150000</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>0.25% to 2%</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>0.25% to 2%</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>0.65 mm to 0.8 mm</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>Greater than 0</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>Output Value</center>',
                                    },
                                ]
                        },
                        //button panel
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
                                        xtype: 'button',
                                        text: "Calculate",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onCalculateDrM2Click'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        text: "Clear Results",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onClearResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartTest_locations',
                                        text: "Test Locations",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartLocation1',
                                        text: "Location 1",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartLocation2',
                                        text: "Location 2",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartLocation3',
                                        text: "Location 3",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartLocation4',
                                        text: "Location 4",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                ]
                        },
                    ]
            },
            //results panel
            {
                xtype: 'panel',
                id: 'DrM2FormResultsPanel',
                title: 'Results',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 2,
                items:
                    [                      
                    ]
            },
            //test results panel
            {
                xtype: 'panel',
                id: 'DrM2FormTestResultsPanel',
                title: 'Correlation with Test Results',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 5,
                items:
                    [
                        {
                            xtype: 'panel',
                            id: 'DDEChartTest_locationsGif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for Test Locations</strong><br><img src="images/DDEChartTest_locations.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartLocation1Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for location 1</strong><br><img src="images/DDEChartLocation1.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartLocation2Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for location 2</strong><br><img src="images/DDEChartLocation2.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartLocation3Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for location 3</strong><br><img src="images/DDEChartLocation3.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartLocation4Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for location 4</strong><br><img src="images/DDEChartLocation4.gif" border="1"></center >'
                        },
                    ]
            },
            //page footer
            {
                xtype: 'panel',
                width: '100%',
                bodyPadding: '5',
                flex: 9,
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



