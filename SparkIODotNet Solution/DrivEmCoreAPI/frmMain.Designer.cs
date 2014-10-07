namespace DrivEmCoreAPI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnForwardLeft = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnForwardRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnCenter = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnBackLeft = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnBackRight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnForwardLeft
            // 
            this.btnForwardLeft.Location = new System.Drawing.Point(-1, -1);
            this.btnForwardLeft.Name = "btnForwardLeft";
            this.btnForwardLeft.Size = new System.Drawing.Size(75, 75);
            this.btnForwardLeft.TabIndex = 0;
            this.btnForwardLeft.Text = "Forward Left";
            this.btnForwardLeft.UseVisualStyleBackColor = true;
            this.btnForwardLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForwardLeft_MouseDown);
            this.btnForwardLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForwardLeft_MouseUp);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(80, -1);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 75);
            this.btnForward.TabIndex = 1;
            this.btnForward.Text = "Forward";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseDown);
            this.btnForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseUp);
            // 
            // btnForwardRight
            // 
            this.btnForwardRight.Location = new System.Drawing.Point(161, -1);
            this.btnForwardRight.Name = "btnForwardRight";
            this.btnForwardRight.Size = new System.Drawing.Size(75, 74);
            this.btnForwardRight.TabIndex = 2;
            this.btnForwardRight.Text = "Forward Right";
            this.btnForwardRight.UseVisualStyleBackColor = true;
            this.btnForwardRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForwardRight_MouseDown);
            this.btnForwardRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForwardRight_MouseUp);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(-1, 80);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(75, 75);
            this.btnLeft.TabIndex = 3;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseUp);
            // 
            // btnCenter
            // 
            this.btnCenter.Enabled = false;
            this.btnCenter.Location = new System.Drawing.Point(81, 80);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Size = new System.Drawing.Size(75, 75);
            this.btnCenter.TabIndex = 4;
            this.btnCenter.UseVisualStyleBackColor = true;
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(161, 80);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(75, 75);
            this.btnRight.TabIndex = 5;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseUp);
            // 
            // btnBackLeft
            // 
            this.btnBackLeft.Location = new System.Drawing.Point(-1, 162);
            this.btnBackLeft.Name = "btnBackLeft";
            this.btnBackLeft.Size = new System.Drawing.Size(75, 75);
            this.btnBackLeft.TabIndex = 6;
            this.btnBackLeft.Text = "Back Left";
            this.btnBackLeft.UseVisualStyleBackColor = true;
            this.btnBackLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBackLeft_MouseDown);
            this.btnBackLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBackLeft_MouseUp);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(81, 162);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 75);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseDown);
            this.btnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseUp);
            // 
            // btnBackRight
            // 
            this.btnBackRight.Location = new System.Drawing.Point(161, 162);
            this.btnBackRight.Name = "btnBackRight";
            this.btnBackRight.Size = new System.Drawing.Size(75, 75);
            this.btnBackRight.TabIndex = 8;
            this.btnBackRight.Text = "Back Right";
            this.btnBackRight.UseVisualStyleBackColor = true;
            this.btnBackRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBackRight_MouseDown);
            this.btnBackRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBackRight_MouseUp);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 237);
            this.Controls.Add(this.btnBackRight);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnBackLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnCenter);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnForwardRight);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.btnForwardLeft);
            this.Name = "frmMain";
            this.Text = "DrivEm TinerAPI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnForwardLeft;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnForwardRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnCenter;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBackLeft;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnBackRight;
    }
}

