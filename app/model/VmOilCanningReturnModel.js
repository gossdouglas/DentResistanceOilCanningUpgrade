Ext.define('DentResistanceOilCanningUpgrade.model.VmOilCanningReturnModel', {
    extend: 'Ext.data.Model',
    idProperty: 'excelRowId',
    fields: [
        { name: 'excelRowId', type: 'int' },

        { name: 'ocvar', type: 'string' },
        { name: 'peakld', type: 'string' },

        { name: 'fvr', type: 'number' },
        { name: 'svr', type: 'number' },
        { name: 'gaugeini', type: 'number' },
        { name: 'span', type: 'number' },
        { name: 'emaj', type: 'number' },
        { name: 'emin', type: 'number' },

        { name: 'DDQ', type: 'string' },
        { name: 'BH210', type: 'string' },
        { name: 'Deflection90', type: 'string' },
        { name: 'Deflection100', type: 'string' },
    ]
})