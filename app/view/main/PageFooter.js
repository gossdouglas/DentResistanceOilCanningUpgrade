var str5 = '<p><center>&copy;Arcelor Mittal<br>' +
    'Problems or Concerns, please contact the <a href="#"> <b>Support Team</b></a><br>Last Modified: 3 December 2021</center>'
var str6 = '<p><center>For questions or comments, please contact<br>' +
    'Sriram Sadagopan, ArcelorMittal R & D<br>' +
    'Phone: (219) 256-7636<br>' +
    'Email: <a href="mailto:mailto:Sriram.Sadagopan@arcelormittal.com"><b>Sriram.Sadagopan@arcelormittal.com</b></a></center>'

Ext.define('DentResistanceOilCanningUpgrade.view.main.PageFooter', {
    extend: 'Ext.form.Panel',
    xtype: 'page-footer',
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
                            width: '50%',
                            html: str5
                        },
                        {
                            xtype: 'panel',
                            width: '50%',
                            html: str6
                        }
                    ]

            },
        ],
});