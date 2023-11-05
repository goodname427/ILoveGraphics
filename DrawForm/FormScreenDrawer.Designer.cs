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
            TBox_Scale = new TextBox();
            Lbl_1 = new Label();
            TBox_Message = new TextBox();
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
            // TBox_Scale
            // 
            TBox_Scale.Location = new Point(195, 12);
            TBox_Scale.Name = "TBox_Scale";
            TBox_Scale.Size = new Size(100, 23);
            TBox_Scale.TabIndex = 1;
            TBox_Scale.Text = "2";
            // 
            // Lbl_1
            // 
            Lbl_1.AutoSize = true;
            Lbl_1.Location = new Point(146, 15);
            Lbl_1.Name = "Lbl_1";
            Lbl_1.Size = new Size(32, 17);
            Lbl_1.TabIndex = 2;
            Lbl_1.Text = "缩放";
            // 
            // TBox_Message
            // 
            TBox_Message.BackColor = Color.White;
            TBox_Message.BorderStyle = BorderStyle.None;
            TBox_Message.Location = new Point(12, 12);
            TBox_Message.Multiline = true;
            TBox_Message.Name = "TBox_Message";
            TBox_Message.ReadOnly = true;
            TBox_Message.Size = new Size(177, 70);
            TBox_Message.TabIndex = 3;
            TBox_Message.Visible = false;
            // 
            // FormScreenDrawer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 661);
            Controls.Add(TBox_Message);
            Controls.Add(Lbl_1);
            Controls.Add(TBox_Scale);
            Controls.Add(Btn_Start);
            Name = "FormScreenDrawer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DrawForm";
            KeyPress += FormScreenDrawer_KeyPress;
            MouseClick += FormScreenDrawer_MouseClick;
            MouseMove += FormScreenDrawer_MouseMove;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Btn_Start;
        private System.Windows.Forms.Timer Tmr_Update;
        private TextBox TBox_Scale;
        private Label Lbl_1;
        private TextBox TBox_Message;
    }
}