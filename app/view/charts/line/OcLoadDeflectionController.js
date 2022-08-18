//https://examples.sencha.com/extjs/6.5.3/examples/kitchensink/?classic#line-spline

//Ext.define('SenchaTesting.view.charts.line.SplineController', {
Ext.define('DentResistanceOilCanningUpgrade.view.charts.line.OcLoadDeflectionController', {
    extend: 'Ext.app.ViewController',
    //alias: 'controller.line-spline',
    alias: 'controller.oc-load-deflection',

    onAxisLabelRender: function (axis, label, layoutContext) {
        return Ext.util.Format.number(label, '0.00');
    },

    onPreview: function () {
        if (Ext.isIE8) {
            Ext.Msg.alert('Unsupported Operation', 'This operation requires a newer version of Internet Explorer.');
            return;
        }
        var chart = this.lookup('chart');
        chart.preview();
    }

});
