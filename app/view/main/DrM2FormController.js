
Ext.define('DentResistanceOilCanningUpgrade.view.main.DrM2FormController', {
    extend: 'Ext.app.ViewController',

    //alias: 'controller.tislotform',
    alias: 'controller.dr-model2-form-controller',

    //control: {
    //    //tie button to an action
    //    'button[name=locationAdd]': {
    //        click: 'onAddClick'
    //    },

    //    //removed from scope by Sean
    //    //'button[name=locationDelete]': {
    //    //    click: 'onDeleteClick'
    //    //},

    //    //tie button to an action
    //    'button[name=locationRefresh]': {
    //        click: 'onGridRefreshClick'
    //    }
    //},

    //save button for a new ti-slot
    onCalculateDrM2Click: function (sender, record) {
        var form = this.getView().getForm();
        var formValues = form.getValues();
        var formFields = form.getFields();
        //console.log("record")
        //console.log(record)
        //console.log("form")
        //console.log(form)
        console.log(formValues.GradeKey);
        console.log(formFields);

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
                url: 'api/DentResistance/CalculateModelTwo',
                waitMsg: 'Calculating..',
                clientValidation: true,
                submitEmptyText: true,
                success: function (frm, action) {

                    //var model = Ext.create('TiSlots.model.TiSlot');
                    //var model = Ext.create('DentResistanceOilCanningUpgrade.model.CalculationDentReistanceModel');
                    var resp = Ext.decode(action.response.responseText);

                    if (resp.data.Result < 0) {
                        resp.data.Result = "No Dent"
                    }

                    //console.clear();
                    //console.log("calculation response: ");
                    //console.log(resp);
                    //console.log(resp.data.Result);

                    //console.log("calculation results object: ");
                    //console.log(calculationResults);
                    //console.log(calculationResults.config);

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

    //save button for a new ti-slot
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

    //save button for a new ti-slot
    //onUlsacClick: function (sender, record) {
    //    //var form = this.getView().getForm();

    //    //Ext.getCmp('DrM1FormTestResultsPanel').update('<center><strong>xxx.</strong><br><img src="images/DDEChartULSAC.gif" border="1"></center>');
    //    //Ext.getCmp('DDEChartUlsacGif').setConfig('hidden', false);
    //    console.log('sender:')
    //    console.log(sender)
    //    console.log('record:')
    //    console.log(record)
    //},

    ////save button for a new ti-slot
    //onCreateClick: function (sender, record) {
    //    var form = this.getView().getForm();

    //    if (!form.isDirty()) {
    //        Ext.Msg.alert('Status', 'No new data to create.');
    //        return;
    //    }
    //    else if (!form.isValid()) {
    //        Ext.Msg.alert('Save not allowed', 'Some required fields are empty.');
    //        //Ext.Message.show({
    //        //    title: "Save not allowed",
    //        //    msg: "Some required fields are empty.",
    //        //    buttons: Ext.MessageBox.OK,
    //        //    icon: Ext.MessageBox.WARNING
    //        //});
    //        return;
    //    }

    //    // Submit the Ajax request and handle the response
    //    form.submit({
    //        url: 'api/tislot',
    //        waitMsg: 'Saving..',
    //        clientValidation: true,
    //        submitEmptyText: true,
    //        success: function (frm, action) {

    //            var model = Ext.create('TiSlots.model.TiSlot');
    //            var resp = Ext.decode(action.response.responseText);

    //            if (resp.data[0]) {
    //                model.set(resp.data[0]);
    //                form.loadRecord(model);
    //            }

    //            Ext.Msg.alert('Status', 'Record created successfully.', function (btn) {
    //                var win = frm.owner.ownerCt;

    //                var grid = Ext.getCmp('tislotgrid');

    //                grid.store.reload();
    //                //win.grid.store.reload();
    //                win.close();
    //            });
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
    //},

    //onReadClick: function (win, record) {
    //    var form = win.down('form').form;//this.getView().getForm();

    //    //result should contain success=true and data property otherwise it will go to failure even if there is no failure
    //    form.load({
    //        waitMsg: 'Loading...',
    //        method: 'GET',
    //        //url: 'api/tislot/' + record.TiSlotId,
    //        url: 'api/tislot/GetTiSlotById/' + record.TiSlotId,
    //        success: function (frm, action) {
    //            try {
    //                var resp = Ext.decode(action.response.responseText);

    //                if (resp.data.length > 0) {
    //                    var model = Ext.create('TiSlots.model.TiSlot');
    //                    model.set(resp.data);
    //                }
    //            }
    //            catch (ex) {
    //                Ext.Msg.alert('Status', 'Exception: ' + ex.Message);

    //            }
    //        },
    //        failure: function (frm, action) {
    //            Ext.Msg.alert("Load failed", action.result.errorMessage);
    //        }
    //    });
    //},

    ////save button for an edited ti-slot
    //onUpdateClick: function (sender, record) {
    //    var form = this.getView().getForm();

    //    if (!form.isDirty()) {
    //        Ext.Msg.alert('Status', 'No pending changes to save.');
    //        return;
    //    }
    //    else if (!form.isValid()) {
    //        Ext.Msg.alert('Save not allowed', 'Some required fields are empty.');
    //        return;
    //    }

    //    form.submit({
    //        url: 'api/tislot/',
    //        waitMsg: 'Updating..',
    //        method: 'PUT',
    //        clientValidation: true,
    //        success: function (frm, action) {
    //            try {
    //                var model = Ext.create('TiSlots.model.TiSlot');
    //                var resp = Ext.decode(action.response.responseText);

    //                if (resp.data.length > 0) {
    //                    model.set(resp.data[0]);
    //                    form.loadRecord(model);
    //                }

    //                Ext.Msg.alert('Status', 'Record updated successfully.', function (btn) {
    //                    var win = form.owner.ownerCt;

    //                    var grid = Ext.getCmp('tislotgrid');

    //                    grid.store.reload();

    //                    win.close();
    //                });


    //            }
    //            catch (ex) {
    //                Ext.Msg.alert('Status', 'Exception: ' + ex.Message);

    //            }
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
    //},

    ////double click on a record
    //onDoubleClick: function (e, record) {
    //    this.selectedRecord = record.data;

    //    //console.log(record.data)

    //    var win = Ext.create('Ext.window.Window', {
    //        layout: 'fit',
    //        xtype: 'form',
    //        width: '75%',
    //        height: '75%',
    //        id: 'tislotFormWindow',
    //        items: {
    //            xtype: 'tislotform'
    //        },
    //        listeners: {
    //            beforeshow: function (form, options) {
    //                //set the ETA date and time controls
    //                estimatedTimeArrival = record.data.EstimatedTimeArrival;
    //                Ext.getCmp('EtaDate').setValue(dateTimePart(estimatedTimeArrival, 1));
    //                Ext.getCmp('EtaTime').setValue(dateTimePart(estimatedTimeArrival, 2));

    //                //display the badge times after being converted from UTC
    //                badgeEnterSite = record.data.BadgeEnterSite;
    //                if (badgeEnterSite != '0001-01-01T00:00:00') {
    //                    Ext.getCmp('BadgeEnterSiteDisplay').setValue(formatDate(record.data.BadgeEnterSite, 1))
    //                }

    //                //display the badge times after being converted from UTC
    //                badgeExitSite = record.data.BadgeExitSite;
    //                if (badgeExitSite != '0001-01-01T00:00:00') {
    //                    Ext.getCmp('BadgeExitSiteDisplay').setValue(formatDate(record.data.BadgeExitSite, 1))
    //                }

    //                //if there is a check in time in the database, display it and lock it for editing
    //                tiSlotCheckIn = record.data.TiSlotCheckIn;
    //                if (tiSlotCheckIn != '0001-01-01T00:00:00') {
    //                    //convert tiSlotCheckIn from UTC in the database to local for display
    //                    var tiSlotCheckInLocal = formatDate(tiSlotCheckIn, 1);
    //                    Ext.getCmp('TiSlotCheckInDate').setValue(dateTimePart(tiSlotCheckInLocal, 1));
    //                    Ext.getCmp('TiSlotCheckInTime').setValue(dateTimePart(tiSlotCheckInLocal, 2));
    //                    Ext.getCmp('TiSlotCheckInDate').setConfig('readOnly', true);
    //                    Ext.getCmp('TiSlotCheckInTime').setConfig('readOnly', true);
    //                }

    //                //set the displayed values in the remote combo boxes
    //                var amnsContactName = record.data.AmnsContactName;
    //                Ext.ComponentQuery.query('#AmnsContactNameCombo')[0].setValue(amnsContactName);
    //                var forCompanyRecord = record.data.ForCompany;
    //                Ext.ComponentQuery.query('#ForCompanyCombo')[0].setValue(forCompanyRecord);
    //                var vendorCarrierRecord = record.data.VendorCarrierName;
    //                Ext.ComponentQuery.query('#VendorCarrierCombo')[0].setValue(vendorCarrierRecord);
    //                var driverNameRecord = record.data.DriverName;
    //                Ext.ComponentQuery.query('#DriverNameCombo')[0].setValue(driverNameRecord);
    //                var departmentRecord = record.data.AmnsDepartment;
    //                Ext.ComponentQuery.query('#DepartmentCombo')[0].setValue(departmentRecord);
    //                //load the sub-department dropdown options based on the department identifier of the record
    //                Ext.getCmp('SubDepartmentCombo').getStore().load(
    //                    {
    //                        params:
    //                        {
    //                            command: 2,
    //                            departmentIdentifier: record.data.AmnsDepartmentIdentifier
    //                        }
    //                    });
    //                var subDepartmentRecord = record.data.AmnsSubDepartment;
    //                Ext.ComponentQuery.query('#SubDepartmentCombo')[0].setValue(subDepartmentRecord);
    //            }
    //        }
    //    }).show()

    //    win.isUpdate = true;
    //    win.record = record.data;
    //    win.grid = this.getView();

    //    this.onReadClick(win, record.data);
    //},

    ////click the add button
    //onAddClick: function (button, e, options) {
    //    var win = Ext.create('Ext.window.Window', {
    //        layout: 'fit',
    //        xtype: 'form',
    //        width: '75%',
    //        height: '75%',
    //        id: 'tislotFormWindow',
    //        items: {
    //            xtype: 'tislotform'
    //        },
    //        listeners: {
    //            beforeshow: function (form, options) {
    //                //set some default values for date and time
    //                Ext.getCmp('EtaDate').setValue(new Date());
    //                Ext.getCmp('EtaTime').setValue('7:00 AM');
    //                var dateData = Ext.getCmp('EtaDate').getSubmitValue();
    //                var timeData = Ext.getCmp('EtaTime').getSubmitValue();
    //                var dateTime = dateData + ' ' + timeData;
    //                //set the default date time to UTC
    //                dateTime = formatDate(dateTime, 2)

    //                Ext.getCmp('EstimatedTimeArrival').setValue(dateTime);

    //                //set the user key
    //                var userSessionKeyOnLoad = Ext.getCmp('UserSessionKeyOnLoad').getSubmitValue();
    //                Ext.getCmp('UserSessionKey').setValue(userSessionKeyOnLoad);
    //            }
    //        },
    //    }).show()

    //    Ext.Ajax.request({
    //        url: 'api/tislotnumber/',
    //        method: 'POST',
    //        success: function (response, opts) {
    //            var resp = Ext.decode(response.responseText);
    //            var data = resp.data;
    //            Ext.getCmp('tislotnumber').setValue(data.TiSlotNumberId);
    //        },
    //        failure: function (response, opts) {
    //        }
    //    });

    //    var store = Ext.getStore('tislot');
    //    win.isUpdate = false;
    //    //store.proxy.extraParams = { type: 'Base' };
    //},

    //onSaveClick: function (sender, record, e) {
    //    var view = Ext.getCmp('tislotFormWindow');
    //    if (view.isUpdate) {
    //        this.onUpdateClick(sender, view.record);
    //    } else {
    //        this.onCreateClick(sender, view.record);
    //    }
    //},

    ////cancel button for the new ti-slot and edit ti-slot pages
    //onCancelClick: function (sender, record, e) {
    //    this.getView().ownerCt.close();
    //},

    ////click an individual record
    //onItemSelected: function (e, record) {
    //    this.selectedRecord = record.data;
    //},
});
