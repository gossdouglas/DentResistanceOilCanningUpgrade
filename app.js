/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'DentResistanceOilCanningUpgrade.Application',

    name: 'DentResistanceOilCanningUpgrade',

    requires: [
        // This will automatically load all classes in the DentResistanceOilCanningUpgrade namespace
        // so that application classes do not need to require each other.
        'DentResistanceOilCanningUpgrade.*'
    ],

    // The name of the initial view to create.
    mainView: 'DentResistanceOilCanningUpgrade.view.main.Main'
});
