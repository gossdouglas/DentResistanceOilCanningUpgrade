Ext.define('DentResistanceOilCanningUpgrade.view.main.DrM2FormController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.dr-model2-form-controller',

    //when the calculate button is pressed...
    onCalculateDrM2Click: function (sender, record) {
        var form = this.getView().getForm();
        var formValues = form.getValues();
        var formFields = form.getFields();
        console.clear();
        //console.log("record")
        //console.log(record)
        //console.log("form")
        //console.log(form)
        //console.log(formValues.GradeKey);
        //console.log(formFields);

        if (!form.isValid()) {
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
                url: 'api/DentResistance/CalculateModelTwo',
                waitMsg: 'Calculating..',
                clientValidation: true,
                submitEmptyText: true,
                success: function (frm, action) {

                    var resp = Ext.decode(action.response.responseText);

                    if (resp.data.Result < 0) {
                        resp.data.Result = "No Dent"
                    }

                    var resp = Ext.decode(action.response.responseText);
                    //console.clear();
                    //console.log("calculation response: ");
                    //console.log(resp);
                    //console.log(resp.data.GradeName);

                    //build a calculation results object from the response
                    var calcResults = Ext.create('dr-model2-calc-results',
                        {
                            items: [
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.GradeName,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.R1,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.R2,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.MajorStrain,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.MinorStrain,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.Thickness,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.PoundsForce,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    width: '11%',
                                    value: resp.data.Result,
                                    editable: false
                                },
                            ]
                        }
                    );

                    //console.log('dr-model1-calc-results');
                    //console.log(calcResults);

                    //add the calculation results to the results panel
                    Ext.getCmp('DrM2FormResultsPanel').add(calcResults);
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

    //clear the results
    onClearResultsClick: function (sender, record) {
        var form = this.getView().getForm();

        Ext.getCmp('DrM2FormResultsPanel').removeAll();
    },

    //when a test data button is clicked
    onTestResultsClick: function (sender, record) {

        //console.clear();
        //get the id of that button
        var btnId = sender.id;
        //load the gif images to an object
        var images = Ext.ComponentQuery.query('panel[cls=test-results-image]');

        //for each image object
        $(images).each(function (index) {

            //console.log('----------');
            //console.log('image number ' + index)
            //console.log('button id ' + btnId)
            //console.log('image id ' + images[index].id)
            //console.log(images[index].id.replace("Gif", ""))

            //if the id of the image matches the id of the button that was clicked...
            if (images[index].id.replace("Gif", "") == btnId) {
                //console.log('image number ' + index + ' clicked.')
                //unhide that image
                images[index].setConfig('hidden', false);
            }
            else {
                //otherwise, hide that image
                images[index].setConfig('hidden', true);
            }
            //console.log('----------');
        });
    },
});
