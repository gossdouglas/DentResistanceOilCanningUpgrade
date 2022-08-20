Ext.define('DentResistanceOilCanningUpgrade.grid.OcBulkCalculationGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'OcBulkCalculationGrid',
    //controller is found by alias
    controller: 'oc-bulk-calculation-controller',
    id: 'oc-bulk-calculation-grid',
    forceFit: true,
    requires: [
        'DentResistanceOilCanningUpgrade.store.OcBulkCalculationStore',
    ],
    title: 'The columns of the source Excel sheet must be arranged in the order and of the range shown below',
    titleAlign: 'center',
    store: {
        type: 'oc-bulk-calculation-store',
    },
    plugins: [
        {
            ptype: 'gridfilters',
        }
    ],
    columns: [
        { text: 'Excel <br> Row', dataIndex: 'excelRowId', autoSizeColumn: true, maxWidth: 100, align: 'center' },
        { text: 'FVR <br> 3000 to 12000 mm <br> Input Value', dataIndex: 'fvr', align: 'center', style: 'background-color: "gray"'},
        { text: 'SVR <br> 3000 to 15000 mm <br> Input Value', dataIndex: 'svr', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Thickness <br> 0.55 to 0.85 mm <br> Input Value', dataIndex: 'gaugeini', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Span <br> 150 to 525 mm <br> Input Value', dataIndex: 'span', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Major Stretch <br> 0 to 2% <br> Input Value', dataIndex: 'emaj', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Minor Stretch <br> 0 to 2% <br> Input Value', dataIndex: 'emin', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Oil Canning Load (N) <br>-<br> Output Value', dataIndex: 'peakld', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Dentload DDQ (N) <br>-<br> Output Value', dataIndex: 'DDQ', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Dentload BH210 (N) <br>-<br> Output Value', dataIndex: 'BH210', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Deflection @ 90N (mm) <br>-<br> Output Value', dataIndex: 'Deflection90', autoSizeColumn: true, minWidth: 100, align: 'center' },
        { text: 'Deflection @ 100N (mm) <br>-<br> Output Value', dataIndex: 'Deflection100', autoSizeColumn: true, minWidth: 100, align: 'center' },
    ],
});
