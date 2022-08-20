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
    'This bulk input will allow Ford to run many inquires at one time reducing the time it takes them to analyze various thicknesses. ' +
    '<p><center><img src="images/oilcanningroof.jpg" border="1"><br>'

//var str3 = '<canvas width="500" height="500" style="width: 200px; height: 200px;"></canvas><div class="demo">' +
//    //'<p>&#215; Angle offset and arc</p><pre>' +
//    //'data-fgColor="#66CC66" data-angleOffset=-125 data-angleArc=250 data-rotation=anticlockwise</pre>' +
//    '<input class="knob" data-angleOffset=-125 data-angleArc=250 data-fgColor="#66EE66" data-rotation="anticlockwise" value="35">' +
//    '</div>'

//var str3 = '<canvas width="500" height="500" style="width: 200px; height: 200px;"></canvas><div class="demo">' +
//    //'<p>&#215; Angle offset and arc</p><pre>' +
//    //'data-fgColor="#66CC66" data-angleOffset=-125 data-angleArc=250 data-rotation=anticlockwise</pre>' +
//    '<input class="knob" data-angleoffset="-125" data-anglearc="250" data-fgcolor="#66EE66" data-rotation="anticlockwise" value="35" style="width: 104px; height: 66px; position: absolute; vertical-align: middle; margin-top: 66px; margin-left: -152px; border: 0px; background: none; font: bold 40px Arial; text-align: center; color: rgb(102, 238, 102); padding: 0px; appearance: none;">'

Ext.define('DentResistanceOilCanningUpgrade.view.main.OilCanningOverview', {
    extend: 'Ext.form.Panel',
    xtype: 'oil-canning-overview',
    items:
        [
            {
                xtype: 'panel',
                //id: 'dr-overview',
                width: '100%',
                bodyPadding: '5',
                flex: 9,
                //hidden: true,
                items:
                    [
                        {
                            xtype: 'panel',
                            title: 'Models',
                            titleAlign: 'center',
                            width: '100%',
                            bodyPadding: '5',
                            flex: 9,
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
                        //{
                        //    xtype: 'panel',
                        //    title: 'Brief Description of Dent Resistance models',
                        //    titleAlign: 'center',
                        //    width: '100%',
                        //    bodyPadding: '5',
                        //    flex: 9,
                        //    items:
                        //        [
                        //            {
                        //                xtype: 'panel',
                        //                layout: {
                        //                    type: 'hbox',
                        //                    align: 'stretch',
                        //                },
                        //                items:
                        //                    [
                        //                        {
                        //                            xtype: 'panel',
                        //                            width: '100%',
                        //                            html: str3
                        //                        },
                        //                    ]
                        //            },
                        //        ]
                        //},
                        //{
                        //    xtype: 'panel',
                        //    title: 'References',
                        //    titleAlign: 'center',
                        //    width: '100%',
                        //    bodyPadding: '5',
                        //    flex: 9,
                        //    items:
                        //        [
                        //            {
                        //                xtype: 'panel',
                        //                layout: {
                        //                    type: 'hbox',
                        //                    align: 'stretch',
                        //                },
                        //                items:
                        //                    [
                        //                        {
                        //                            xtype: 'panel',
                        //                            width: '5%',
                        //                            html: '1.'
                        //                        },
                        //                        {
                        //                            xtype: 'panel',
                        //                            width: '95%',
                        //                            html: str4
                        //                        },
                        //                    ]
                        //            },
                        //        ]
                        //},
                        {
                            xtype: 'panel',
                            //title: 'References',
                            //titleAlign: 'center',
                            width: '100%',
                            bodyPadding: '5',
                            flex: 9,
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
                                                //{
                                                //    xtype: 'panel',
                                                //    width: '5%',
                                                //    html: '1.'
                                                //},
                                                {
                                                    xtype: 'panel',
                                                    width: '95%',
                                                    html: str5
                                                },
                                            ]
                                    },
                                ]
                        },
                    ]
            },

            //{
            //    xtype: 'panel',
            //    title: 'Models',
            //    titleAlign: 'center',
            //    width: '100%',
            //    bodyPadding: '5',
            //    flex: 9,
            //    items:
            //        [
            //            {
            //                xtype: 'panel',
            //                layout: {
            //                    type: 'hbox',
            //                    align: 'stretch',
            //                },
            //                items:
            //                    [
            //                        {
            //                            xtype: 'panel',
            //                            width: '5%',
            //                            html: '1.'
            //                        },                                  
            //                        {
            //                            xtype: 'panel',
            //                            width: '95%',
            //                            html: str1
            //                        },
            //                    ]
            //            },
            //            {
            //                xtype: 'panel',
            //                layout: {
            //                    type: 'hbox',
            //                    align: 'stretch',
            //                },
            //                items:
            //                    [
            //                        {
            //                            xtype: 'panel',
            //                            width: '5%',
            //                            html: '2.'
            //                        },
            //                        {
            //                            xtype: 'panel',
            //                            width: '95%',
            //                            html: str2
            //                        },
            //                    ]
            //            },
            //        ]
            //},
            //{
            //    xtype: 'panel',
            //    title: 'Brief Description of Dent Resistance models',
            //    titleAlign: 'center',
            //    width: '100%',
            //    bodyPadding: '5',
            //    flex: 9,
            //    items:
            //        [
            //            {
            //                xtype: 'panel',
            //                layout: {
            //                    type: 'hbox',
            //                    align: 'stretch',
            //                },
            //                items:
            //                    [
            //                        {
            //                            xtype: 'panel',
            //                            width: '100%',
            //                            html: str3
            //                        },
            //                    ]
            //            },
            //        ]
            //},
            //{
            //    xtype: 'panel',
            //    title: 'References',
            //    titleAlign: 'center',
            //    width: '100%',
            //    bodyPadding: '5',
            //    flex: 9,
            //    items:
            //        [
            //            {
            //                xtype: 'panel',
            //                layout: {
            //                    type: 'hbox',
            //                    align: 'stretch',
            //                },
            //                items:
            //                    [
            //                        {
            //                            xtype: 'panel',
            //                            width: '5%',
            //                            html: '1.'
            //                        },
            //                        {
            //                            xtype: 'panel',
            //                            width: '95%',
            //                            html: str4
            //                        },
            //                    ]
            //            },
            //        ]
            //},
            //{
            //    xtype: 'panel',
            //    //title: 'References',
            //    //titleAlign: 'center',
            //    width: '100%',
            //    bodyPadding: '5',
            //    flex: 9,
            //    items:
            //        [
            //            {
            //                xtype: 'panel',
            //                layout: {
            //                    type: 'hbox',
            //                    align: 'stretch',
            //                },
            //                items:
            //                    [
            //                        //{
            //                        //    xtype: 'panel',
            //                        //    width: '5%',
            //                        //    html: '1.'
            //                        //},
            //                        {
            //                            xtype: 'panel',
            //                            width: '95%',
            //                            html: str5
            //                        },
            //                    ]
            //            },
            //        ]
            //},
        ],
});