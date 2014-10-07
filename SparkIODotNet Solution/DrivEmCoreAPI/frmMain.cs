using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SparkIO.WebServices;

namespace DrivEmCoreAPI
{
    public partial class frmMain : Form
    {
        private Helper helper;

        public frmMain()
        {
            InitializeComponent();

            helper = new Helper();
        }
        
        private void btnBack_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.BACK);
        }

        private void btnBack_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnForward_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.FORWARD);
        }

        private void btnForward_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.LEFT);
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.RIGHT);
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnForwardLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.FORWARDLEFT);
        }

        private void btnForwardLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnForwardRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.FORWARDRIGHT);
        }

        private void btnForwardRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnBackLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.BACKLEFT);
        }

        private void btnBackLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }

        private void btnBackRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.BACKRIGHT);
        }

        private void btnBackRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.STOP);
        }
    }
}
