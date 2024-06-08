namespace debrick
{
    partial class Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            labelVersion = new Label();
            label = new Label();
            button1 = new Button();
            checkedListBox = new CheckedListBox();
            button2 = new Button();
            button0 = new Button();
            progressBar = new ProgressBar();
            textBoxIP = new TextBox();
            linkLabel = new LinkLabel();
            SuspendLayout();
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            labelVersion.ForeColor = Color.Aqua;
            labelVersion.Location = new Point(12, 9);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(73, 32);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "v.2.1";
            // 
            // label
            // 
            label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label.ForeColor = Color.White;
            label.Location = new Point(100, 104);
            label.Name = "label";
            label.Size = new Size(600, 306);
            label.TabIndex = 3;
            label.Text = resources.GetString("label.Text");
            label.TextAlign = ContentAlignment.TopCenter;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = Color.Transparent;
            button1.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            button1.ForeColor = SystemColors.InfoText;
            button1.Location = new Point(300, 430);
            button1.Name = "button1";
            button1.Size = new Size(200, 50);
            button1.TabIndex = 4;
            button1.TabStop = false;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = false;
            button1.Visible = false;
            button1.Click += Button_Click;
            // 
            // checkedListBox
            // 
            checkedListBox.BackColor = Color.Black;
            checkedListBox.BorderStyle = BorderStyle.FixedSingle;
            checkedListBox.CheckOnClick = true;
            checkedListBox.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            checkedListBox.ForeColor = Color.White;
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new Point(100, 150);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new Size(600, 248);
            checkedListBox.TabIndex = 5;
            checkedListBox.Visible = false;
            checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.BackColor = Color.Transparent;
            button2.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            button2.ForeColor = SystemColors.InfoText;
            button2.Location = new Point(500, 430);
            button2.Name = "button2";
            button2.Size = new Size(200, 50);
            button2.TabIndex = 6;
            button2.TabStop = false;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            button2.Click += Button_Click;
            // 
            // button0
            // 
            button0.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button0.BackColor = Color.Transparent;
            button0.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            button0.ForeColor = SystemColors.InfoText;
            button0.Location = new Point(100, 430);
            button0.Name = "button0";
            button0.Size = new Size(200, 50);
            button0.TabIndex = 7;
            button0.TabStop = false;
            button0.Text = "button0";
            button0.UseVisualStyleBackColor = false;
            button0.Visible = false;
            button0.Click += Button_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(100, 250);
            progressBar.MarqueeAnimationSpeed = 10;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(600, 31);
            progressBar.Step = 1;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 8;
            progressBar.Visible = false;
            // 
            // textBoxIP
            // 
            textBoxIP.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxIP.Location = new Point(250, 250);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new Size(300, 44);
            textBoxIP.TabIndex = 9;
            textBoxIP.Text = "192.168.1.123";
            textBoxIP.TextAlign = HorizontalAlignment.Center;
            textBoxIP.Visible = false;
            // 
            // linkLabel
            // 
            linkLabel.AutoSize = true;
            linkLabel.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabel.LinkColor = Color.Aqua;
            linkLabel.Location = new Point(475, 9);
            linkLabel.Name = "linkLabel";
            linkLabel.Size = new Size(291, 32);
            linkLabel.TabIndex = 10;
            linkLabel.TabStop = true;
            linkLabel.Text = "https://github.com/OpenIPC/debrick";
            linkLabel.LinkClicked += LinkLabel_LinkClicked;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(778, 544);
            Controls.Add(linkLabel);
            Controls.Add(textBoxIP);
            Controls.Add(progressBar);
            Controls.Add(button0);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(labelVersion);
            Controls.Add(checkedListBox);
            Controls.Add(label);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(800, 600);
            MinimizeBox = false;
            MinimumSize = new Size(800, 600);
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "debrick";
            FormClosing += Form_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelVersion;
        private Label label;
        private Button button1;
        private CheckedListBox checkedListBox;
        private Button button2;
        private Button button0;
        private ProgressBar progressBar;
        private TextBox textBoxIP;
        private LinkLabel linkLabel;
    }
}
