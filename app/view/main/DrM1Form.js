Ext.QuickTips.init();

Ext.define('DentResistanceOilCanningUpgrade.view.main.DrM1Form', {
    extend: 'Ext.form.Panel',
    xtype: 'dr-model1-form',
    border: false,
    controller: 'dr-model1-form-controller',
    scrollable: true,
    url: 'api/DentResistance/CalculateModelOne',

    items:
        [
            {
                xtype: 'panel',
                title: 'Load for 0.1 mm Dent Depth, Model 1',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 1,
                items:
                    [
                        //entry fields
                        {
                            xtype: 'panel',
                            //layout: {
                            //    type: 'hbox',
                            //    align: 'stretch',
                            //},
                            layout: {
                                type: 'hbox',
                                pack: 'center',
                                align: 'middle'
                            },
                            items:
                                [
                                    {
                                        xtype: 'textfield',
                                        //width: '3%',
                                        id: 'GradeKey',
                                        name: 'GradeKey',
                                        bind: '{gradeKeyDrM1Combo.value}',
                                        hidden: true,
                                        readOnly: true,
                                        allowBlank: false,
                                    },
                                    {
                                        xtype: 'textfield',
                                        //width: '3%',
                                        id: 'GradeName',
                                        name: 'GradeName',
                                        hidden: true,
                                        readOnly: false,
                                        allowBlank: false,
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: '11%',
                                        id: 'GradeKeyDrM1Combo',
                                        reference: 'gradeKeyDrM1Combo',
                                        publishes: 'value',
                                        fieldLabel: 'Grade',
                                        labelAlign: 'top',
                                        displayField: 'grade_name',
                                        valueField: 'grade_key',
                                        allowBlank: false,
                                        store: {
                                            type: 'GradeStoreModel1',
                                            autoLoad: true
                                        },
                                        queryMode: 'local',
                                        //queryMode: 'remote',
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
                                        id: 'R1DrM1',
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
                                        id: 'R2DrM1',
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
                                        id: 'MajorStrainDrM1',
                                        name: 'MajorStrain',
                                        fieldLabel: 'Major Strain (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(\.2[5-9]+|\.[3-9]+|1|1\.\d+|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Major strain must be a value between .25 and 2',
                                        value: '.25'
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'MinorStrainDrM1',
                                        name: 'MinorStrain',
                                        fieldLabel: 'Minor Strain (%)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(\.2[5-9]+|\.[3-9]+|1|1\.\d+|2)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Minor strain must be a value between .25 and 2',
                                        value: '.25'
                                    },
                                    {
                                        xtype: 'textfield',
                                        id: 'ThicknessDrM1',
                                        name: 'Thickness',
                                        fieldLabel: 'Thickness (mm)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        allowBlank: false,
                                        regex: /^(\.6[5-9]+|\.7\d*|\.8)$/i,
                                        msgTarget: 'side', // location of the error message
                                        invalidText: 'Thickness must be a value between .65 and .8',
                                        value: '.65'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: '',
                                        fieldLabel: 'Dent Depth',
                                        labelAlign: 'top',
                                        width: '11%',
                                        value: '.1',
                                        editable: false
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: '',
                                        fieldLabel: 'Newtons (N)',
                                        labelAlign: 'top',
                                        width: '11%',
                                        value: 'N/A',
                                        editable: false
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: '',
                                        fieldLabel: 'Lb. Force (lbf)',
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
                                        html: '<center>.25% to 2%</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>.25% to 2%</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>.65 mm to .8 mm</center>',
                                    },
                                    {
                                        xtype: 'panel',
                                        width: '11%',
                                        html: '<center>Constant Value</center>',
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
                            //layout: {
                            //    type: 'hbox',
                            //    align: 'stretch',
                            //},
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
                                            click: 'onCalculateDrM1Click'
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
                                        id: 'DDEChartUlsac',
                                        text: "ULSAC",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartBH210Door1',
                                        text: "BH210-Door 1",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartBH250Door1',
                                        text: "BH250-Door 1",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartBH210Door2',
                                        text: "BH210-Door 2",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartBH250Door2',
                                        text: "BH250-Door 2",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartDP500Door2',
                                        text: "DP500-Door 2",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartBH210Door3',
                                        text: "BH210-Door 3",
                                        margin: '20 10 0 0',
                                        width: '10%',
                                        listeners: {
                                            click: 'onTestResultsClick'
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        id: 'DDEChartDP500Door3',
                                        text: "DP500- Door 3",
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
                id: 'DrM1FormResultsPanel',
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
                id: 'DrM1FormTestResultsPanel',
                width: '100%',
                bodyPadding: '5',
                flex: 5,
                items:
                    [
                        //
                        {
                            xtype: 'panel',
                            id: 'DDEChartUlsacGif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for ULSAC</strong><br><img src="images/DDEChartULSAC.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartBH210Door1Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for BH210-Door 1</strong><br><img src="images/DDEChartBH210Door1.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartBH250Door1Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for BH250-Door 1</strong><br><img src="images/DDEChartBH250Door1.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartBH210Door2Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for BH210-Door 2</strong><br><img src="images/DDEChartBH210Door2.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartBH250Door2Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for BH250-Door 2</strong><br><img src="images/DDEChartBH250Door2.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartDP500Door2Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for DP500-Door 2</strong><br><img src="images/DDEChartDP500Door2.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartBH210Door3Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for BH210-Door 3</strong><br><img src="images/DDEChartBH210Door3.gif" border="1"></center >'
                        },
                        {
                            xtype: 'panel',
                            id: 'DDEChartDP500Door3Gif',
                            cls: 'test-results-image',
                            hidden: true,
                            html: '<p><center><strong>Chart for DP500-Door 3</strong><br><img src="images/DDEChartDP500Door3.gif" border="1"></center >'
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



