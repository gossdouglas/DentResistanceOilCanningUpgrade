Ext.define('DentResistanceOilCanningUpgrade.grid.OcBulkErrorGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'OcBulkErrorGrid',
    id: 'oc-bulk-error-grid',
    forceFit: true,
    requires: [
        'DentResistanceOilCanningUpgrade.store.OcBulkErrorStore'
    ],   
    title: 'Un-processed Excel rows due to range errors in the source Excel file',
    titleAlign: 'center',
    columnLines: true,
    store: {
        //store is found by alias
        type: 'oc-bulk-error-store',
    },
    columns: [
        { text: 'Excel Row', dataIndex: 'excelRowId', align: 'left', flex: 1, autoSizeColumn: true, maxWidth: 100 },
        { text: 'Description of the problem', dataIndex: 'errorText', align: 'left', flex: 1, autoSizeColumn: true, minWidth: 100 },
    ],
});
