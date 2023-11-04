namespace DrawForm
{
    partial class FormScreenDrawer
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
            components = new System.ComponentModel.Container();
            Btn_Start = new Button();
            Tmr_Update = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // Btn_Start
            // 
            Btn_Start.Location = new Point(12, 12);
            Btn_Start.Name = "Btn_Start";
            Btn_Start.Size = new Size(120, 70);
            Btn_Start.TabIndex = 0;
            Btn_Start.Text = "开始";
            Btn_Start.UseVisualStyleBackColor = true;
            Btn_Start.Click += Btn_Start_Click;
            // 
            // Tmr_Update
            // 
            Tmr_Update.Interval = 1;
            Tmr_Update.Tick += Tmr_Update_Tick;
            // 
            // DrawForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 661);
            Controls.Add(Btn_Start);
            Name = "DrawForm";
            Text = "DrawForm";
            ResumeLayout(false);
        }

        #endregion

        private Button Btn_Start;
        private System.Windows.Forms.Timer Tmr_Update;
    }
}