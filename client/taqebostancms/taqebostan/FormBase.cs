using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{    

    public class FormBase : Form
    {

        #region Declarations
        private bool drag = false;
        private Point start_point = new Point(0, 0);
        private bool draggable = true;
        private string exclude_list = "";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #endregion

        #region Constructor , Dispose

        public FormBase()
        {
            InitializeComponent();

            //
			//Adding Mouse Event Handlers for the Form
			//
			this.MouseDown += new MouseEventHandler(Form_MouseDown);
            this.MouseUp += new MouseEventHandler(Form_MouseUp);
            this.MouseMove += new MouseEventHandler(Form_MouseMove);

        }     

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

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			// 
			// FormBase
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(369, 182);
			this.Name = "FormBase";
			this.Text = "AlerterForm";

		}

        #endregion

        #region Overriden Functions

        protected override void OnControlAdded(ControlEventArgs e)
        {
            //
			//Add Mouse Event Handlers for each control added into the form,
			//if Draggable property of the form is set to true and the control
			//name is not in the ExcludeList.Exclude list is the comma seperated
			//list of the Controls for which you do not require the mouse handler 
			//to be added. For Example a button.  
			//
			if (this.Draggable && (this.ExcludeList.IndexOf(e.Control.Name) == -1))
            {
                e.Control.MouseDown += new MouseEventHandler(Form_MouseDown);
                e.Control.MouseUp += new MouseEventHandler(Form_MouseUp);
                e.Control.MouseMove += new MouseEventHandler(Form_MouseMove);
            }
            base.OnControlAdded(e);
        }

        #endregion

        #region Event Handlers

		void Form_MouseDown(object sender, MouseEventArgs e)
		{          
			//
			//On Mouse Down set the flag drag=true and 
			//Store the clicked point to the start_point variable
			//
			this.drag = true;            
			this.start_point = new Point(e.X, e.Y);
		}

		void Form_MouseUp(object sender, MouseEventArgs e)
		{
			//
			//Set the drag flag = false;
			//
			this.drag = false;
		}

        void Form_MouseMove(object sender, MouseEventArgs e)
        {
            //
			//If drag = true, drag the form
			//
			if (this.drag)
            {
                Point p1 = new Point(e.X, e.Y);
                Point p2 = this.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.start_point.X, p2.Y - this.start_point.Y);
                this.Location = p3;
            }
        }

        

        

        #endregion

        #region Properties

        public string ExcludeList
        {
            set
            {
                this.exclude_list = value;
            }
            get
            {
                return this.exclude_list.Trim();
            }
        }

        public bool Draggable
        {
            set
            {
                this.draggable = value;
            }
            get
            {
                return this.draggable;
            }
        }

        #endregion

    }
}