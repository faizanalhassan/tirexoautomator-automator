namespace VideoDownloader
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchBoxLbl = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.movieResCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hostsComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.providersComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(177, 20);
            this.searchBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(589, 26);
            this.searchBox.TabIndex = 0;
            // 
            // searchBoxLbl
            // 
            this.searchBoxLbl.AutoSize = true;
            this.searchBoxLbl.Location = new System.Drawing.Point(13, 23);
            this.searchBoxLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.searchBoxLbl.Name = "searchBoxLbl";
            this.searchBoxLbl.Size = new System.Drawing.Size(144, 20);
            this.searchBoxLbl.TabIndex = 1;
            this.searchBoxLbl.Text = "Exact Movie Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(792, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 37);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // movieResCombo
            // 
            this.movieResCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.movieResCombo.FormattingEnabled = true;
            this.movieResCombo.Location = new System.Drawing.Point(177, 89);
            this.movieResCombo.Name = "movieResCombo";
            this.movieResCombo.Size = new System.Drawing.Size(589, 28);
            this.movieResCombo.TabIndex = 3;
            this.movieResCombo.SelectedIndexChanged += new System.EventHandler(this.MovieResCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Qualities:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 167);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Available Hosts:";
            // 
            // hostsComboBox
            // 
            this.hostsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hostsComboBox.FormattingEnabled = true;
            this.hostsComboBox.Location = new System.Drawing.Point(177, 164);
            this.hostsComboBox.Name = "hostsComboBox";
            this.hostsComboBox.Size = new System.Drawing.Size(589, 28);
            this.hostsComboBox.TabIndex = 3;
            this.hostsComboBox.SelectedIndexChanged += new System.EventHandler(this.HostsComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 238);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Available Provider:";
            // 
            // providersComboBox
            // 
            this.providersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.providersComboBox.FormattingEnabled = true;
            this.providersComboBox.Location = new System.Drawing.Point(177, 235);
            this.providersComboBox.Name = "providersComboBox";
            this.providersComboBox.Size = new System.Drawing.Size(589, 28);
            this.providersComboBox.TabIndex = 3;
            this.providersComboBox.SelectedIndexChanged += new System.EventHandler(this.HostsComboBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 692);
            this.Controls.Add(this.providersComboBox);
            this.Controls.Add(this.hostsComboBox);
            this.Controls.Add(this.movieResCombo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchBoxLbl);
            this.Controls.Add(this.searchBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Automator tirexo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Label searchBoxLbl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox movieResCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox hostsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox providersComboBox;
    }
}

