/**
 * A spline chart is a specialized form of conventional line and area charts. Unlike
 * conventional charts which connect data points with straight lines, a spline draws a
 * fitted curve through the data points. They are used specifically for plotting data that
 * requires the use of curve fittings e.g. impulse-response, product life cycle etc.
 */

//https://examples.sencha.com/extjs/6.5.3/examples/kitchensink/?classic#line-spline

Ext.define('DentResistanceOilCanningUpgrade.view.charts.line.OcLoadDeflection', {
    extend: 'Ext.Panel',
    xtype: 'oc-load-deflection',
    controller: 'oc-load-deflection',

    width: 650,

    tbar: [
        '->',
        {
            text: 'Preview',
            handler: 'onPreview'
        }
    ],

    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 500,
        store: {
            type: 'oc-load-deflection-store'
        },
        insetPadding: '10 20 10 10',
        captions: {
            title: 'Load Deflection'
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            title: 'Load (N)',
            grid: true,
            fields: 'Load',
            //minimum: 0,
            //maximum: 300,
            label: {
                renderer: 'onAxisLabelRender'
            }
        }, {
            type: 'numeric',
            position: 'bottom',
            title: 'Deflection (mm)',
            grid: true,
            fields: 'Deflection',
            //minimum: 0,
            //maximum: 10,
            label: {
                textPadding: 0,
                rotate: {
                    degrees: -45
                }
            }
        }],
        series: [{
            type: 'line',
            xField: 'Deflection',
            yField: 'Load',
            smooth: true,
            highlight: true,
            showMarkers: false
        }]
    }
});