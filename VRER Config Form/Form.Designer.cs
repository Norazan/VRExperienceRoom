namespace VRExperienceRoom
{
    partial class Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.button1 = new System.Windows.Forms.Button();
            this.TabWindow = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TimerMS = new System.Windows.Forms.Label();
            this.FileLabel = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.TimerLabel = new System.Windows.Forms.Label();
            this.Countdown = new System.Windows.Forms.CheckBox();
            this.TimerStatus = new System.Windows.Forms.Label();
            this.TimerButton = new System.Windows.Forms.Button();
            this.ConsoleWindowRun = new System.Windows.Forms.RichTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.HoursInput = new System.Windows.Forms.TextBox();
            this.ScentInput = new System.Windows.Forms.CheckBox();
            this.HeatInput = new System.Windows.Forms.CheckBox();
            this.WindRPMInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.WindRPMScrollbar = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SecondsInput = new System.Windows.Forms.TextBox();
            this.MinutesInput = new System.Windows.Forms.TextBox();
            this.ConsoleWindowCreate = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SettingsListCreate = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ProgramTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanForArduinosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PortSelector = new System.Windows.Forms.ComboBox();
            this.Port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SettingsListRun = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TabWindow.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WindRPMScrollbar)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddSettingButton_Click);
            // 
            // tabControl1
            // 
            this.TabWindow.Controls.Add(this.tabPage1);
            this.TabWindow.Controls.Add(this.tabPage3);
            this.TabWindow.Location = new System.Drawing.Point(12, 27);
            this.TabWindow.Name = "tabControl1";
            this.TabWindow.SelectedIndex = 0;
            this.TabWindow.Size = new System.Drawing.Size(711, 478);
            this.TabWindow.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SettingsListRun);
            this.tabPage3.Controls.Add(this.TimerMS);
            this.tabPage3.Controls.Add(this.FileLabel);
            this.tabPage3.Controls.Add(this.button8);
            this.tabPage3.Controls.Add(this.TimerLabel);
            this.tabPage3.Controls.Add(this.Countdown);
            this.tabPage3.Controls.Add(this.TimerStatus);
            this.tabPage3.Controls.Add(this.TimerButton);
            this.tabPage3.Controls.Add(this.ConsoleWindowRun);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(703, 452);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Run";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.TimerMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimerMS.Location = new System.Drawing.Point(538, 321);
            this.TimerMS.Name = "label3";
            this.TimerMS.Size = new System.Drawing.Size(33, 15);
            this.TimerMS.TabIndex = 25;
            this.TimerMS.Text = "000";
            this.TimerMS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.FileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileLabel.Location = new System.Drawing.Point(399, 14);
            this.FileLabel.Name = "label8";
            this.FileLabel.Size = new System.Drawing.Size(296, 77);
            this.FileLabel.TabIndex = 24;
            this.FileLabel.Text = "Select config file...";
            this.FileLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(482, 94);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(128, 36);
            this.button8.TabIndex = 23;
            this.button8.Text = "Browse...";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // label5
            // 
            this.TimerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimerLabel.Location = new System.Drawing.Point(463, 297);
            this.TimerLabel.Name = "label5";
            this.TimerLabel.Size = new System.Drawing.Size(97, 34);
            this.TimerLabel.TabIndex = 21;
            this.TimerLabel.Text = "00:00:00";
            this.TimerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox5
            // 
            this.Countdown.AutoSize = true;
            this.Countdown.Enabled = false;
            this.Countdown.Location = new System.Drawing.Point(119, 314);
            this.Countdown.Name = "checkBox5";
            this.Countdown.Size = new System.Drawing.Size(80, 17);
            this.Countdown.TabIndex = 20;
            this.Countdown.Text = "Countdown";
            this.Countdown.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.TimerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimerStatus.ForeColor = System.Drawing.Color.Red;
            this.TimerStatus.Location = new System.Drawing.Point(406, 264);
            this.TimerStatus.Name = "label4";
            this.TimerStatus.Size = new System.Drawing.Size(204, 34);
            this.TimerStatus.TabIndex = 19;
            this.TimerStatus.Text = "Stopped";
            this.TimerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button7
            // 
            this.TimerButton.Enabled = false;
            this.TimerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimerButton.Location = new System.Drawing.Point(118, 264);
            this.TimerButton.Name = "button7";
            this.TimerButton.Size = new System.Drawing.Size(163, 49);
            this.TimerButton.TabIndex = 17;
            this.TimerButton.Text = "Start";
            this.TimerButton.UseVisualStyleBackColor = true;
            this.TimerButton.Click += new System.EventHandler(this.TimerButton_Click);
            // 
            // richTextBox3
            // 
            this.ConsoleWindowRun.Location = new System.Drawing.Point(80, 350);
            this.ConsoleWindowRun.Name = "richTextBox3";
            this.ConsoleWindowRun.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ConsoleWindowRun.Size = new System.Drawing.Size(525, 96);
            this.ConsoleWindowRun.TabIndex = 16;
            this.ConsoleWindowRun.Text = "";
            this.ConsoleWindowRun.TextChanged += new System.EventHandler(this.ConsoleWindow_TextChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PortSelector);
            this.tabPage1.Controls.Add(this.HoursInput);
            this.tabPage1.Controls.Add(this.ScentInput);
            this.tabPage1.Controls.Add(this.HeatInput);
            this.tabPage1.Controls.Add(this.WindRPMInput);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.WindRPMScrollbar);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.SecondsInput);
            this.tabPage1.Controls.Add(this.MinutesInput);
            this.tabPage1.Controls.Add(this.ConsoleWindowCreate);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.SettingsListCreate);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(703, 452);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Create";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.HoursInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoursInput.ForeColor = System.Drawing.Color.Gray;
            this.HoursInput.Location = new System.Drawing.Point(485, 78);
            this.HoursInput.MaxLength = 2;
            this.HoursInput.Name = "textBox1";
            this.HoursInput.Size = new System.Drawing.Size(27, 29);
            this.HoursInput.TabIndex = 25;
            this.HoursInput.Text = "H";
            this.HoursInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HoursInput.TextChanged += new System.EventHandler(this.HoursInput_TextChanged);
            this.HoursInput.Enter += new System.EventHandler(this.HoursInput_Enter);
            this.HoursInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Validate_KeyDown);
            this.HoursInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Validate_KeyPress);
            this.HoursInput.Leave += new System.EventHandler(this.HoursInput_Leave);
            // 
            // checkBox2
            // 
            this.ScentInput.AutoSize = true;
            this.ScentInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScentInput.Location = new System.Drawing.Point(608, 210);
            this.ScentInput.Name = "checkBox2";
            this.ScentInput.Size = new System.Drawing.Size(70, 24);
            this.ScentInput.TabIndex = 24;
            this.ScentInput.Text = "Scent";
            this.ScentInput.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.HeatInput.AutoSize = true;
            this.HeatInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeatInput.Location = new System.Drawing.Point(417, 210);
            this.HeatInput.Name = "checkBox1";
            this.HeatInput.Size = new System.Drawing.Size(63, 24);
            this.HeatInput.TabIndex = 23;
            this.HeatInput.Text = "Heat";
            this.HeatInput.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.WindRPMInput.BackColor = System.Drawing.SystemColors.Control;
            this.WindRPMInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.WindRPMInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindRPMInput.Location = new System.Drawing.Point(498, 176);
            this.WindRPMInput.MaxLength = 4;
            this.WindRPMInput.Name = "textBox4";
            this.WindRPMInput.Size = new System.Drawing.Size(100, 19);
            this.WindRPMInput.TabIndex = 22;
            this.WindRPMInput.Text = "0";
            this.WindRPMInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.WindRPMInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Validate_KeyDown);
            this.WindRPMInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Validate_KeyPress);
            this.WindRPMInput.Leave += new System.EventHandler(this.WindRPMInput_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(494, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "Wind (RPM)";
            // 
            // trackBar1
            // 
            this.WindRPMScrollbar.BackColor = System.Drawing.SystemColors.Control;
            this.WindRPMScrollbar.Location = new System.Drawing.Point(417, 150);
            this.WindRPMScrollbar.Maximum = 5200;
            this.WindRPMScrollbar.Name = "trackBar1";
            this.WindRPMScrollbar.Size = new System.Drawing.Size(261, 45);
            this.WindRPMScrollbar.TabIndex = 20;
            this.WindRPMScrollbar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.WindRPMScrollbar.Scroll += new System.EventHandler(this.WindRPMScrollbar_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(561, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 24);
            this.label7.TabIndex = 19;
            this.label7.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(515, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = ":";
            // 
            // textBox3
            // 
            this.SecondsInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondsInput.ForeColor = System.Drawing.Color.Gray;
            this.SecondsInput.Location = new System.Drawing.Point(578, 78);
            this.SecondsInput.MaxLength = 2;
            this.SecondsInput.Name = "textBox3";
            this.SecondsInput.Size = new System.Drawing.Size(27, 29);
            this.SecondsInput.TabIndex = 4;
            this.SecondsInput.Text = "S";
            this.SecondsInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SecondsInput.Enter += new System.EventHandler(this.SecondsInput_Enter);
            this.SecondsInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Validate_KeyDown);
            this.SecondsInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Validate_KeyPress);
            this.SecondsInput.Leave += new System.EventHandler(this.SecondsInput_Leave);
            // 
            // textBox2
            // 
            this.MinutesInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinutesInput.ForeColor = System.Drawing.Color.Gray;
            this.MinutesInput.Location = new System.Drawing.Point(532, 78);
            this.MinutesInput.MaxLength = 2;
            this.MinutesInput.Name = "textBox2";
            this.MinutesInput.Size = new System.Drawing.Size(27, 29);
            this.MinutesInput.TabIndex = 3;
            this.MinutesInput.Text = "M";
            this.MinutesInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MinutesInput.TextChanged += new System.EventHandler(this.MinutesInput_TextChanged);
            this.MinutesInput.Enter += new System.EventHandler(this.MinutesInput_Enter);
            this.MinutesInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Validate_KeyDown);
            this.MinutesInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Validate_KeyPress);
            this.MinutesInput.Leave += new System.EventHandler(this.MinutesInput_Leave);
            // 
            // richTextBox2
            // 
            this.ConsoleWindowCreate.Location = new System.Drawing.Point(80, 350);
            this.ConsoleWindowCreate.Name = "richTextBox2";
            this.ConsoleWindowCreate.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ConsoleWindowCreate.Size = new System.Drawing.Size(525, 96);
            this.ConsoleWindowCreate.TabIndex = 15;
            this.ConsoleWindowCreate.Text = "";
            this.ConsoleWindowCreate.TextChanged += new System.EventHandler(this.ConsoleWindow_TextChanged);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(118, 295);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(163, 49);
            this.button4.TabIndex = 12;
            this.button4.Text = "Export";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(463, 295);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(163, 49);
            this.button3.TabIndex = 11;
            this.button3.Text = "Test Current Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.TestSettingsButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(559, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 29);
            this.button2.TabIndex = 10;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.RemoveSettingButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(501, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Timestamp";
            // 
            // listView1
            // 
            this.SettingsListCreate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Port,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.SettingsListCreate.FullRowSelect = true;
            this.SettingsListCreate.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SettingsListCreate.HideSelection = false;
            this.SettingsListCreate.Location = new System.Drawing.Point(15, 14);
            this.SettingsListCreate.Name = "listView1";
            this.SettingsListCreate.Size = new System.Drawing.Size(378, 264);
            this.SettingsListCreate.TabIndex = 8;
            this.SettingsListCreate.UseCompatibleStateImageBehavior = false;
            this.SettingsListCreate.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Timestamp";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Wind (RPM)";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Heat";
            this.columnHeader3.Width = 47;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Mist";
            this.columnHeader4.Width = 47;
            // 
            // timer1
            // 
            this.ProgramTimer.Tick += new System.EventHandler(this.ProgramTimer_Tick_UI);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(735, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanForArduinosToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // scanForArduinosToolStripMenuItem
            // 
            this.scanForArduinosToolStripMenuItem.Name = "scanForArduinosToolStripMenuItem";
            this.scanForArduinosToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.scanForArduinosToolStripMenuItem.Text = "Scan for Arduino\'s";
            this.scanForArduinosToolStripMenuItem.Click += new System.EventHandler(this.ScanForArduinosMenuItem_Click);
            // 
            // comboBox1
            // 
            this.PortSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortSelector.FormattingEnabled = true;
            this.PortSelector.Items.AddRange(new object[] {
            "Test1"});
            this.PortSelector.Location = new System.Drawing.Point(417, 14);
            this.PortSelector.Name = "comboBox1";
            this.PortSelector.Size = new System.Drawing.Size(261, 21);
            this.PortSelector.TabIndex = 26;
            // 
            // Port
            // 
            this.Port.Text = "Port";
            this.Port.Width = 100;
            // 
            // listView2
            // 
            this.SettingsListRun.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.SettingsListRun.FullRowSelect = true;
            this.SettingsListRun.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SettingsListRun.HideSelection = false;
            this.SettingsListRun.Location = new System.Drawing.Point(15, 14);
            this.SettingsListRun.Name = "listView2";
            this.SettingsListRun.Size = new System.Drawing.Size(378, 226);
            this.SettingsListRun.TabIndex = 26;
            this.SettingsListRun.UseCompatibleStateImageBehavior = false;
            this.SettingsListRun.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Port";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Timestamp";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Wind (RPM)";
            this.columnHeader7.Width = 80;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Heat";
            this.columnHeader8.Width = 47;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Mist";
            this.columnHeader9.Width = 47;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 517);
            this.Controls.Add(this.TabWindow);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form";
            this.Text = "VR Experience Room";
            this.TabWindow.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WindRPMScrollbar)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TextBox1_Enter(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl TabWindow;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView SettingsListCreate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox ConsoleWindowCreate;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox Countdown;
        private System.Windows.Forms.Label TimerStatus;
        private System.Windows.Forms.Button TimerButton;
        private System.Windows.Forms.RichTextBox ConsoleWindowRun;
        private System.Windows.Forms.Label TimerLabel;
        private System.Windows.Forms.Timer ProgramTimer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SecondsInput;
        private System.Windows.Forms.TextBox MinutesInput;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TrackBar WindRPMScrollbar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WindRPMInput;
        private System.Windows.Forms.CheckBox ScentInput;
        private System.Windows.Forms.CheckBox HeatInput;
        private System.Windows.Forms.TextBox HoursInput;
        private System.Windows.Forms.Label TimerMS;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanForArduinosToolStripMenuItem;
        private System.Windows.Forms.ComboBox PortSelector;
        private System.Windows.Forms.ColumnHeader Port;
        private System.Windows.Forms.ListView SettingsListRun;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
    }
}

