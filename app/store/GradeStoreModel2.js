Ext.define('DentResistanceOilCanningUpgrade.store.GradeStoreModel2', {
    extend: 'Ext.data.Store',
    alias: 'store.GradeStoreModel2',
    storeId: 'gradestore-model2',

    data: [
        {
            "grade_key": 27,
            "model": 0,
            "grade_name": "BH180",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 20,
            "model": 0,
            "grade_name": "BH210",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 21,
            "model": 0,
            "grade_name": "BH250",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 23,
            "model": 0,
            "grade_name": "BH280",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 25,
            "model": 0,
            "grade_name": "DDQ+",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 22,
            "model": 0,
            "grade_name": "DP500",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 26,
            "model": 0,
            "grade_name": "DQSK",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 35,
            "model": 0,
            "grade_name": "DR210",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        },
        {
            "grade_key": 24,
            "model": 0,
            "grade_name": "IF-Rephos",
            "publish": 0,
            "normal_anisotropy": 0.0,
            "constants": 0.0,
            "constants_1": null,
            "date_created": "0001-01-01T00:00:00",
            "created_by": null,
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": null
        }
    ],

    model: 'DentResistanceOilCanningUpgrade.model.GradeModel',

    proxy: {
        type: 'ajax',
        url: 'api/DentResistance/GetGrades',
        //extraParams: {
        //    command: 1,
        //    departmentIdentifier: 0,
        //},
        //pageParam: undefined,
        //limitParam: undefined,
        //startParam: undefined,
        //appendId: true,
        //noCache: false,
        reader: {
            type: 'json',
            rootProperty: function (data) {
                if (!data.validated) {
                    window.location.href = data.url;
                } else {

                    //console.clear();
                    //console.log("Returned data: ")
                    //console.log(data.data);
                    return data.data;
                    
                    //console.log(data);
                    //return data;
                }
            }
        }
    },
    
    //autoLoad: true,
    //listeners: {
    //    load: function (store, records, successful, operation, eOpts) {
    //        var rec = records;
    //    }
    //}
});
