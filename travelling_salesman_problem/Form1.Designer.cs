namespace travelling_salesman_problem
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            address_button = new Button();
            solve_button = new Button();
            addressBox = new TextBox();
            Address_lable = new Label();
            SuspendLayout();
            // 
            // gMapControl1
            // 
            gMapControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gMapControl1.Bearing = 0F;
            gMapControl1.CanDragMap = true;
            gMapControl1.EmptyTileColor = Color.Navy;
            gMapControl1.GrayScaleMode = false;
            gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            gMapControl1.LevelsKeepInMemmory = 5;
            gMapControl1.Location = new Point(336, 243);
            gMapControl1.MarkersEnabled = true;
            gMapControl1.MaxZoom = 2;
            gMapControl1.MinZoom = 2;
            gMapControl1.MouseWheelZoomEnabled = true;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.Name = "gMapControl1";
            gMapControl1.NegativeMode = false;
            gMapControl1.PolygonsEnabled = true;
            gMapControl1.RetryLoadTile = 0;
            gMapControl1.RoutesEnabled = true;
            gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            gMapControl1.SelectedAreaFillColor = Color.FromArgb(33, 65, 105, 225);
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.Size = new Size(1183, 703);
            gMapControl1.TabIndex = 0;
            gMapControl1.Zoom = 0D;
            gMapControl1.Load += gMapControl1_Load;
            // 
            // address_button
            // 
            address_button.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            address_button.Location = new Point(527, 157);
            address_button.Name = "address_button";
            address_button.Size = new Size(795, 58);
            address_button.TabIndex = 1;
            address_button.Text = "Add address";
            address_button.UseVisualStyleBackColor = true;
            address_button.Click += address_button_Click;
            // 
            // solve_button
            // 
            solve_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            solve_button.Location = new Point(527, 963);
            solve_button.Name = "solve_button";
            solve_button.Size = new Size(795, 66);
            solve_button.TabIndex = 2;
            solve_button.Text = "Solve";
            solve_button.UseVisualStyleBackColor = true;
            solve_button.Click += solve_button_Click;
            // 
            // addressBox
            // 
            addressBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            addressBox.Location = new Point(359, 25);
            addressBox.Name = "addressBox";
            addressBox.Size = new Size(1183, 47);
            addressBox.TabIndex = 3;
            // 
            // Address_lable
            // 
            Address_lable.AutoSize = true;
            Address_lable.Location = new Point(807, 100);
            Address_lable.Name = "Address_lable";
            Address_lable.Size = new Size(0, 41);
            Address_lable.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2034, 1072);
            Controls.Add(Address_lable);
            Controls.Add(addressBox);
            Controls.Add(solve_button);
            Controls.Add(address_button);
            Controls.Add(gMapControl1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private Button address_button;
        private Button button2;
        private TextBox addressBox;
        private Button solve_button;
        private Label Address_lable;
    }
}