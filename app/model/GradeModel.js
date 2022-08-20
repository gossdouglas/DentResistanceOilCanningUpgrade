Ext.define('DentResistanceOilCanningUpgrade.model.GradeModel', {
    extend: 'Ext.data.Model',
    //idProperty: 'GradeModelId',
    idProperty: 'grade_key',
    fields: [

        { name: 'grade_key', type: 'string' },
        { name: 'model', type: 'string' },
        { name: 'grade_name', type: 'string' },
        { name: 'publish', type: 'string' },
        { name: 'normal_anisotropy', type: 'string' },
        { name: 'constants', type: 'string' },
        { name: 'constants_1', type: 'string' },
        { name: 'date_created', type: 'string' },
        { name: 'created_by', type: 'string' },
        { name: 'date_updated', type: 'string' },
        { name: 'updated_by', type: 'string' }
    ]
})