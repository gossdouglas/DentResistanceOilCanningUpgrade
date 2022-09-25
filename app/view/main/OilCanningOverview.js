var str1 = '<a href="#" onclick="showOcCalculator()"><b>Oil Canning and Dent Resistance Calculator</b></a><br>' +
    'Oil canning is a phenomenon that results in a sudden drop in load when a panel is loaded. Depending on the severity of oil canning, ' +
    'this phenomenon can be accompanied by a loud noise. The oil canning load is the maximum load in the load displacement curve and is used ' +
    'as a measure of the stability of the panel structure. The model predicts the oil canning load and the load deflection ' +
    'behavior of the roof panel for when it is loaded using a 150mm flat indenter.' +

    '<p>A commonly accepted measure of quasi-static dent resistance is the load to produce a defined permanent set. The model predicts ' +
    'The model predicts the load for a permanent set of 0.1mm when loaded using a 25.4mm hemispherical indenter which is consistent with the ' +
    'measure recommended by the Auto/Steel Partnership.' +
    '<p>Note: The oil canning resistance and initial stiffness of panels can depend significantly on the type of indenter, the indenter size and the support conditions for the panel. </p>'

var str2 = '<a href="#" onclick="showOcBulkInput()"><b>Bulk Input</b></a><br>' +
    'This bulk input will allow users to run many inquires at one time reducing the time it takes them to analyze various scenarios. ' +
    '<p><center><img src="images/oilcanningroof.jpg" border="1"><br>'

Ext.define('DentResistanceOilCanningUpgrade.view.main.OilCanningOverview', {
    extend: 'Ext.form.Panel',
    xtype: 'oil-canning-overview',
    items:
        [
            {
                xtype: 'panel',
                width: '100%',
                bodyPadding: '5',
                flex: 9,
                items:
                    [
                        //oil canning
                        {
                            xtype: 'panel',
                            title: 'Models',
                            titleAlign: 'center',
                            width: '100%',
                            bodyPadding: '5',
                            flex: 9,
                            items:
                                [
                                    //oil canning and dent resistance calculator
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
                                                    width: '5%',
                                                    html: '1.'
                                                },
                                                {
                                                    xtype: 'panel',
                                                    width: '95%',
                                                    html: str1
                                                },
                                            ]
                                    },
                                    //bulk input
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
                                                    width: '5%',
                                                    html: '2.'
                                                },
                                                {
                                                    xtype: 'panel',
                                                    width: '95%',
                                                    html: str2
                                                },
                                            ]
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
                        }
                    ]
            },
        ],
});