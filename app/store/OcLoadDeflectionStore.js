//https://examples.sencha.com/extjs/6.5.3/examples/kitchensink/?classic#line-spline
Ext.define('DentResistanceOilCanningUpgrade.store.OcLoadDeflectionStore', {
    extend: 'Ext.data.Store',
    alias: 'store.oc-load-deflection-store',

    fields: ['theta', 'sin', 'cos', 'tan'],

    constructor: function (config) {
        config = config || {};

        // Create data in construct time instead of defining it
        // on the prototype, so that each example that's using
        // this store works on its own set of data.
        config.data = [
            {
                "Deflection": 0.05,
                "Load": 4.231411
            },
            {
                "Deflection": 0.1,
                "Load": 8.632189
            },
            {
                "Deflection": 0.15,
                "Load": 13.20233
            },
            {
                "Deflection": 0.2,
                "Load": 17.94185
            },
            {
                "Deflection": 0.25,
                "Load": 22.85072
            },
            {
                "Deflection": 0.3,
                "Load": 27.92897
            },
            {
                "Deflection": 0.35,
                "Load": 33.17658
            },
            {
                "Deflection": 0.4,
                "Load": 38.59356
            },
            {
                "Deflection": 0.45,
                "Load": 44.17991
            },
            {
                "Deflection": 0.5,
                "Load": 49.93563
            },
            {
                "Deflection": 0.55,
                "Load": 55.86071
            },
            {
                "Deflection": 0.6,
                "Load": 61.95516
            },
            {
                "Deflection": 0.65,
                "Load": 68.21898
            },
            {
                "Deflection": 0.7,
                "Load": 74.65215
            },
            {
                "Deflection": 0.75,
                "Load": 81.25471
            },
            {
                "Deflection": 0.8,
                "Load": 88.02663
            },
            {
                "Deflection": 0.85,
                "Load": 94.96791
            },
            {
                "Deflection": 0.9,
                "Load": 102.0786
            },
            {
                "Deflection": 0.95,
                "Load": 109.3586
            },
            {
                "Deflection": 1.0,
                "Load": 116.808
            },
            {
                "Deflection": 1.05,
                "Load": 124.4267
            },
            {
                "Deflection": 1.1,
                "Load": 132.1869
            },
            {
                "Deflection": 1.15,
                "Load": 139.9301
            },
            {
                "Deflection": 1.2,
                "Load": 147.6313
            },
            {
                "Deflection": 1.25,
                "Load": 155.2905
            },
            {
                "Deflection": 1.3,
                "Load": 162.9078
            },
            {
                "Deflection": 1.35,
                "Load": 170.4831
            },
            {
                "Deflection": 1.4,
                "Load": 178.0165
            },
            {
                "Deflection": 1.45,
                "Load": 185.5079
            },
            {
                "Deflection": 1.5,
                "Load": 192.9573
            },
            {
                "Deflection": 1.55,
                "Load": 200.3648
            },
            {
                "Deflection": 1.6,
                "Load": 207.7303
            },
            {
                "Deflection": 1.65,
                "Load": 215.0538
            },
            {
                "Deflection": 1.7,
                "Load": 222.3354
            },
            {
                "Deflection": 1.75,
                "Load": 229.5751
            },
            {
                "Deflection": 1.8,
                "Load": 236.7727
            },
            {
                "Deflection": 1.85,
                "Load": 243.9284
            },
            {
                "Deflection": 1.9,
                "Load": 251.0422
            },
            {
                "Deflection": 1.95,
                "Load": 258.114
            },
            {
                "Deflection": 2.0,
                "Load": 265.1438
            },
            {
                "Deflection": 2.05,
                "Load": 271.6898
            },
            {
                "Deflection": 2.1,
                "Load": 277.3845
            },
            {
                "Deflection": 2.15,
                "Load": 282.3387
            },
            {
                "Deflection": 2.2,
                "Load": 286.6636
            },
            {
                "Deflection": 2.25,
                "Load": 290.4704
            },
            {
                "Deflection": 2.3,
                "Load": 293.8701
            },
            {
                "Deflection": 2.35,
                "Load": 296.9738
            },
            {
                "Deflection": 2.4,
                "Load": 299.8925
            },
            {
                "Deflection": 2.45,
                "Load": 302.7375
            },
            {
                "Deflection": 2.5,
                "Load": 305.6197
            },
            {
                "Deflection": 2.55,
                "Load": 308.6503
            },
            {
                "Deflection": 2.6,
                "Load": 311.9404
            },
            {
                "Deflection": 2.65,
                "Load": 315.601
            },
            {
                "Deflection": 2.7,
                "Load": 319.7433
            },
            {
                "Deflection": 2.75,
                "Load": 324.4782
            },
            {
                "Deflection": 2.8,
                "Load": 329.9171
            },
            {
                "Deflection": 2.85,
                "Load": 336.1709
            },
            {
                "Deflection": 2.9,
                "Load": 343.3507
            },
            {
                "Deflection": 2.95,
                "Load": 351.5676
            },
            {
                "Deflection": 3.0,
                "Load": 360.9326
            },
            {
                "Deflection": 3.05,
                "Load": 371.4592
            },
            {
                "Deflection": 3.1,
                "Load": 383.0477
            },
            {
                "Deflection": 3.15,
                "Load": 395.6395
            },
            {
                "Deflection": 3.2,
                "Load": 409.1765
            }
        ];

        this.callParent([config]);
    }

});

