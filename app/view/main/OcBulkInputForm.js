Ext.QuickTips.init();
objExcelJson = new Object();

Ext.define('DentResistanceOilCanningUpgrade.view.main.OcBulkInputForm', {
    extend: 'Ext.form.Panel',
    xtype: 'oil-canning-bulk-input-form',
    border: false,
    controller: 'oil-canning-bulk-input-form-controller',
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
                title: 'Bulk Oil Canning Calculation',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 2,
                items:
                    [
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                align: 'stretch',
                            },
                            items:
                                [
                                    {
                                        xtype: 'panel',
                                        html: '<input type="file" id="excelfile" onChange=""/>',
                                    },                                  
                                    {
                                        xtype: 'button',
                                        text: "Load Excel",
                                        width: '10%',
                                        //margin: top, right, bottom, left
                                        margin: '0 1 0 1',
                                        listeners: {
                                            click: function (input, val, opts) {

                                                ExportToTable();
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        text: "Clear Results",
                                        width: '10%',
                                        //margin: top, right, bottom, left
                                        margin: '0 1 0 1',
                                        listeners: {
                                            click: function (input, val, opts) {

                                                var store = Ext.data.StoreManager.lookup('OcBulkCalculationStore');
                                                store.removeAll();
                                                var store = Ext.data.StoreManager.lookup('OcBulkErrorStore');
                                                store.removeAll();
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        text: "Export",
                                        disabled: true,
                                        width: '10%',
                                        //margin: top, right, bottom, left
                                        margin: '0 1 0 1',
                                        listeners: {
                                            click: function (input, val, opts) {
                                            }
                                        }
                                    },
                                ],
                        },                      
                    ]
            },
            //results panel
            {
                xtype: 'panel',
                id: 'DrM1FormResultsPanel',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 8,
                scrollable: true,
                items:
                    [
                        {
                            xtype: 'OcBulkCalculationGrid',
                        },
                    ]
            },
            //validation error panel
            {
                xtype: 'panel',
                id: 'DrM1FormErrorsPanel',
                titleAlign: 'center',
                width: '100%',
                bodyPadding: '5',
                flex: 3,
                scrollable: true,
                items:
                    [
                        {
                            xtype: 'OcBulkErrorGrid',
                        },
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


