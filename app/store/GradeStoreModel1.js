Ext.define('DentResistanceOilCanningUpgrade.store.GradeStoreModel1', {
    extend: 'Ext.data.Store',
    alias: 'store.GradeStoreModel1',
    storeId: 'gradestore-model1',

    data: [
        {
            "grade_key": "8",
            "model": "0",
            "grade_name": "BH180",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "1",
            "model": "0",
            "grade_name": "BH210",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "5",
            "model": "0",
            "grade_name": "BH250",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "9",
            "model": "0",
            "grade_name": "BH280",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "11",
            "model": "0",
            "grade_name": "DDQ+",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "7",
            "model": "0",
            "grade_name": "DP500",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "12",
            "model": "0",
            "grade_name": "DQSK",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "34",
            "model": "0",
            "grade_name": "DR210",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        },
        {
            "grade_key": "10",
            "model": "0",
            "grade_name": "IF-Rephos",
            "publish": "0",
            "normal_anisotropy": "0",
            "constants": "0",
            "constants_1": "",
            "date_created": "0001-01-01T00:00:00",
            "created_by": "",
            "date_updated": "0001-01-01T00:00:00",
            "updated_by": ""
        }
    ],

    //data: [
    //    {
    //        "grade_key": "8",
    //        "grade_name": "BH180",
    //    },
    //    {
    //        "grade_key": "1",
    //        "grade_name": "BH210",
    //    },
    //    {
    //        "grade_key": "5",
    //        "grade_name": "BH250",
    //    },
    //    {
    //        "grade_key": "9",
    //        "grade_name": "BH280",
    //    },
    //    {
    //        "grade_key": "11",
    //        "grade_name": "DDQ+",
    //    },
    //    {
    //        "grade_key": "7",
    //        "grade_name": "DP500",
    //    },
    //    {
    //        "grade_key": "12",
    //        "grade_name": "DQSK",
    //    },
    //    {
    //        "grade_key": "34",
    //        "grade_name": "DR210",
    //    },
    //    {
    //        "grade_key": "10",
    //        "grade_name": "IF-Rephos",
    //    }
    //]

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
                    //return the actual array

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
