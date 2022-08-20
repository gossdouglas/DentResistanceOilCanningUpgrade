
Ext.define('DentResistanceOilCanningUpgrade.view.main.OilCanningFormController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.oil-canning-form-controller',

    //
    onCalculateOcClick: function (sender, record) {
        var form = this.getView().getForm();
        var formValues = form.getValues();
        var formFields = form.getFields();
        console.clear();
        //console.log("form")
        //console.log(form)
        //console.log(formValues.GradeKey);
        //console.log(formFields);

        if (!form.isValid()) {
            //Ext.Msg.alert('Save not allowed', 'Some required fields are empty.');
            Ext.Message.show({
                title: "Save not allowed",
                msg: "Some required fields are empty.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        }
        else {
            // Submit the Ajax request and handle the response
            form.submit({
                url: 'api/OilCanning/CalculateOilCanning',
                //url: 'api/OilCanning/LoadOilCanning',
                waitMsg: 'Calculating..',
                clientValidation: true,
                submitEmptyText: true,
                success: function (frm, action) {

                    //var model = Ext.create('TiSlots.model.TiSlot');
                    //var model = Ext.create('DentResistanceOilCanningUpgrade.model.CalculationDentReistanceModel');
                    var resp = Ext.decode(action.response.responseText);
                    //console.clear();
                    //console.log("calculation response: ");
                    //console.log(resp);
                    //console.log(resp.data.oilcanning.ocvar);
                    //console.log(resp.data.oilcanning.BH210);
                    //console.log(resp.data.oilcanning.DDQ);

                    var tmpPeakLd = resp.data.oilcanning.peakld;
                    //console.log("tmpPeakLd: " + tmpPeakLd.toFixed(1));

                    if (resp.data.oilcanning.ocvar < 0) {

                        //var tmpPeakLd = resp.data.oilcanning.peakld;
                        //tmpPeakLd = tmpPeakLd.toFixed(1);

                        Ext.getCmp('ChartOcCalculate').setCaptions(
                            {
                                //title: 'Oil Canning at ' + resp.data.oilcanning.peakld.toFixed(1) + 'N'
                                title: 'Oil Canning at ' + tmpPeakLd.toFixed(1) + ' N'
                            }
                        );
                    }
                    else {
                        Ext.getCmp('ChartOcCalculate').setCaptions(
                            {
                                //title: 'Oil Canning at ' + resp.data.oilcanning.peakld.toFixed(1) + 'N'
                                title: 'No Oil Canning < 400 N'
                            }
                        );
                    }

                    Ext.getCmp('DdqOcCalculate').setValue(resp.data.oilcanning.DDQ);
                    Ext.getCmp('Bh210OcCalculate').setValue(resp.data.oilcanning.BH210);

                    //link up to the bulk oil canning store
                    var store = Ext.data.StoreManager.lookup('OcLoadDeflectionStore');
                    //clear that store of past data
                    store.removeAll();
                    //load that store with current data
                    store.add(resp.data.chartList);
                    //Ext.get('ExportOcCalculate').setDisabled(true);

                    //var calcResults = Ext.create('dr-model1-calc-results',
                    //    {
                    //        items: [
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.GradeName,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.R1,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.R2,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.MajorStrain,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.MinorStrain,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.Thickness,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: '.1',
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.RunningTotal,
                    //                editable: false
                    //            },
                    //            {
                    //                xtype: 'textfield',
                    //                width: '11%',
                    //                value: resp.data.FootPounds,
                    //                editable: false
                    //            },
                    //        ]
                    //    }
                    //);

                    //console.log('dr-model1-calc-results');
                    //console.log(calcResults);

                    //Ext.getCmp('DrM1FormResultsPanel').add(calcResults);//works
                },
                failure: function (frm, action) {
                    if (action.failureType === Ext.form.action.Action.CLIENT_INVALID) {
                        Ext.Msg.alert('CLIENT_INVALID', 'Something has been missed. Please check and try again.');
                    }
                    if (action.failureType === Ext.form.action.Action.CONNECT_FAILURE) {
                        Ext.Msg.alert('CONNECT_FAILURE', 'Status: ' + action.response.status + ': ' + action.response.statusText);
                    }
                    if (action.failureType === Ext.form.action.Action.SERVER_INVALID) {
                        Ext.Msg.alert('SERVER_INVALID', action.result.message);
                    }
                }
            });
        }
    },

    onExportOcClick: function (sender, record) {
        var form = this.getView().getForm();
        var formValues = form.getValues();
        var formFields = form.getFields();
        console.clear();
        //console.log("form")
        //console.log(form)
        //console.log(formValues.GradeKey);
        //console.log(formFields);

        var exportDetailedValue = Ext.getCmp('ExportDetailedOcCalculate').getValue();
        console.log("exportDetailedValue: " + exportDetailedValue);

        //if (!form.isValid()) {
        //    //Ext.Msg.alert('Save not allowed', 'Some required fields are empty.');
        //    Ext.Message.show({
        //        title: "Save not allowed",
        //        msg: "Some required fields are empty.",
        //        buttons: Ext.MessageBox.OK,
        //        icon: Ext.MessageBox.WARNING
        //    });
        //    return;
        //}
        //else {
        //    // Submit the Ajax request and handle the response
        //    form.submit({
        //        url: 'api/OilCanning/CalculateOilCanning',
        //        //url: 'api/OilCanning/LoadOilCanning',
        //        waitMsg: 'Calculating..',
        //        clientValidation: true,
        //        submitEmptyText: true,
        //        success: function (frm, action) {

        //            //var model = Ext.create('TiSlots.model.TiSlot');
        //            //var model = Ext.create('DentResistanceOilCanningUpgrade.model.CalculationDentReistanceModel');
        //            var resp = Ext.decode(action.response.responseText);
        //            console.clear();
        //            console.log("calculation response: ");
        //            console.log(resp);
        //            console.log(resp.data.oilcanning.BH210);
        //            console.log(resp.data.oilcanning.DDQ);

        //            Ext.getCmp('DdqOcCalculate').setValue(resp.data.oilcanning.DDQ);
        //            Ext.getCmp('Bh210OcCalculate').setValue(resp.data.oilcanning.BH210);

        //            //link up to the bulk oil canning store
        //            var store = Ext.data.StoreManager.lookup('OcLoadDeflectionStore');
        //            //clear that store of past data
        //            store.removeAll();
        //            //load that store with current data
        //            store.add(resp.data.chartList);
        //            //Ext.get('ExportOcCalculate').setDisabled(true);

        //            //var calcResults = Ext.create('dr-model1-calc-results',
        //            //    {
        //            //        items: [
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.GradeName,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.R1,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.R2,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.MajorStrain,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.MinorStrain,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.Thickness,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: '.1',
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.RunningTotal,
        //            //                editable: false
        //            //            },
        //            //            {
        //            //                xtype: 'textfield',
        //            //                width: '11%',
        //            //                value: resp.data.FootPounds,
        //            //                editable: false
        //            //            },
        //            //        ]
        //            //    }
        //            //);

        //            //console.log('dr-model1-calc-results');
        //            //console.log(calcResults);

        //            //Ext.getCmp('DrM1FormResultsPanel').add(calcResults);//works
        //        },
        //        failure: function (frm, action) {
        //            if (action.failureType === Ext.form.action.Action.CLIENT_INVALID) {
        //                Ext.Msg.alert('CLIENT_INVALID', 'Something has been missed. Please check and try again.');
        //            }
        //            if (action.failureType === Ext.form.action.Action.CONNECT_FAILURE) {
        //                Ext.Msg.alert('CONNECT_FAILURE', 'Status: ' + action.response.status + ': ' + action.response.statusText);
        //            }
        //            if (action.failureType === Ext.form.action.Action.SERVER_INVALID) {
        //                Ext.Msg.alert('SERVER_INVALID', action.result.message);
        //            }
        //        }
        //    });
        //}
    },

});
