Ext.define('DentResistanceOilCanningUpgrade.model.VmOilCanningErrorModel', {
    extend: 'Ext.data.Model',
    idProperty: 'excelRowNumber',
    fields: [
        { name: 'excelRowNumber', type: 'string' },
        { name: 'errorText', type: 'string' },
    ]
})