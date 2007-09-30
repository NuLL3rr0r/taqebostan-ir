namespace taqebostan
{
    partial class frmTinyInputBox
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
            this.txtNode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtNode
            // 
            this.txtNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtNode.Location = new System.Drawing.Point(0, 0);
            this.txtNode.MaxLength = 35;
            this.txtNode.Name = "txtNode";
            this.txtNode.Size = new System.Drawing.Size(127, 21);
            this.txtNode.TabIndex = 0;
            this.txtNode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNode_KeyPress);
            // 
            // frmTinyInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(127, 20);
            this.Controls.Add(this.txtNode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTinyInputBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.frmNodeManager_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNode;
    }
}