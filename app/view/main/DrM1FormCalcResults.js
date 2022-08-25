Ext.define('DentResistanceOilCanningUpgrade.view.main.DrM1FormCalcResults', {
    extend: 'Ext.panel.Panel',
    xtype: 'dr-model1-calc-results',
    alias: 'dr-model1-calc-results',
    layout: {
        type: 'hbox',
        pack: 'center',
        align: 'middle'
    },
    items:
        [
            {
                xtype: 'textfield',
                width: '11%',
                value: 'Grade Name',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'R1',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'R2',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'Major Strain',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'Minor Strain',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'Thickness',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: '.1',
                value: 'Dent Depth',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'RunningTotal',
                editable: false
            },
            {
                xtype: 'textfield',
                width: '11%',
                value: 'FootPounds',
                editable: false
            },
        ]
});