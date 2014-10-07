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

namespace DrivEmTinkerAPI
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
            helper.GiveCommand(Helper.DriveCommands.Rear);
        }

        private void btnBack_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnForward_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Forward);
        }

        private void btnForward_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Left);
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Right);
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnForwardLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.ForwardLeft);
        }

        private void btnForwardLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnForwardRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.ForwardRight);
        }

        private void btnForwardRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnBackLeft_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.RearLeft);
        }

        private void btnBackLeft_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }

        private void btnBackRight_MouseDown(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.RearRight);
        }

        private void btnBackRight_MouseUp(object sender, MouseEventArgs e)
        {
            helper.GiveCommand(Helper.DriveCommands.Stop);
        }
    }
}
