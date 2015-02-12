namespace MieMieUpdata
{
    partial class MieMieUpdata
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MieMieUpdata));
            this.labelChooseLine = new System.Windows.Forms.Label();
            this.labelChooseGame = new System.Windows.Forms.Label();
            this.comboBoxLine = new System.Windows.Forms.ComboBox();
            this.comboBoxGame = new System.Windows.Forms.ComboBox();
            this.labelJavaPath = new System.Windows.Forms.Label();
            this.labelPlayerName = new System.Windows.Forms.Label();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.textBoxJavaPath = new System.Windows.Forms.TextBox();
            this.labelMemory = new System.Windows.Forms.Label();
            this.textBoxMemory = new System.Windows.Forms.TextBox();
            this.buttonSimpleUpdata = new System.Windows.Forms.Button();
            this.buttonFullUpdata = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonJavaPath = new System.Windows.Forms.Button();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            this.buttonDeleteGame = new System.Windows.Forms.Button();
            this.labelMB = new System.Windows.Forms.Label();
            this.checkBoxX64 = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonUpdataCloak = new System.Windows.Forms.Button();
            this.buttonUpdataSkin = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorkerFullDownLoad = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerLog = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerSimpleDownLoad = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerSkinOrCape = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerDeleteSkinOrCape = new System.ComponentModel.BackgroundWorker();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChooseLine
            // 
            this.labelChooseLine.AutoSize = true;
            this.labelChooseLine.Location = new System.Drawing.Point(190, 326);
            this.labelChooseLine.Name = "labelChooseLine";
            this.labelChooseLine.Size = new System.Drawing.Size(65, 12);
            this.labelChooseLine.TabIndex = 0;
            this.labelChooseLine.Text = "更新线路：";
            this.labelChooseLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelChooseGame
            // 
            this.labelChooseGame.AutoSize = true;
            this.labelChooseGame.Location = new System.Drawing.Point(190, 300);
            this.labelChooseGame.Name = "labelChooseGame";
            this.labelChooseGame.Size = new System.Drawing.Size(65, 12);
            this.labelChooseGame.TabIndex = 1;
            this.labelChooseGame.Text = "游戏选择：";
            this.labelChooseGame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxLine
            // 
            this.comboBoxLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLine.FormattingEnabled = true;
            this.comboBoxLine.Items.AddRange(new object[] {
            "网通线路",
            "电信线路"});
            this.comboBoxLine.Location = new System.Drawing.Point(255, 323);
            this.comboBoxLine.Name = "comboBoxLine";
            this.comboBoxLine.Size = new System.Drawing.Size(115, 20);
            this.comboBoxLine.TabIndex = 6;
            this.comboBoxLine.SelectedIndexChanged += new System.EventHandler(this.comboBoxLine_SelectedIndexChanged);
            // 
            // comboBoxGame
            // 
            this.comboBoxGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGame.FormattingEnabled = true;
            this.comboBoxGame.Location = new System.Drawing.Point(255, 297);
            this.comboBoxGame.Name = "comboBoxGame";
            this.comboBoxGame.Size = new System.Drawing.Size(115, 20);
            this.comboBoxGame.TabIndex = 7;
            this.comboBoxGame.SelectedIndexChanged += new System.EventHandler(this.comboBoxGame_SelectedIndexChanged);
            // 
            // labelJavaPath
            // 
            this.labelJavaPath.AutoSize = true;
            this.labelJavaPath.Location = new System.Drawing.Point(20, 275);
            this.labelJavaPath.Name = "labelJavaPath";
            this.labelJavaPath.Size = new System.Drawing.Size(65, 12);
            this.labelJavaPath.TabIndex = 5;
            this.labelJavaPath.Text = "JAVA路径：";
            this.labelJavaPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPlayerName
            // 
            this.labelPlayerName.AutoSize = true;
            this.labelPlayerName.Location = new System.Drawing.Point(20, 300);
            this.labelPlayerName.Name = "labelPlayerName";
            this.labelPlayerName.Size = new System.Drawing.Size(65, 12);
            this.labelPlayerName.TabIndex = 4;
            this.labelPlayerName.Text = "玩家名称：";
            this.labelPlayerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(80, 297);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(100, 21);
            this.textBoxPlayerName.TabIndex = 9;
            // 
            // textBoxJavaPath
            // 
            this.textBoxJavaPath.Location = new System.Drawing.Point(80, 272);
            this.textBoxJavaPath.Name = "textBoxJavaPath";
            this.textBoxJavaPath.Size = new System.Drawing.Size(369, 21);
            this.textBoxJavaPath.TabIndex = 8;
            // 
            // labelMemory
            // 
            this.labelMemory.AutoSize = true;
            this.labelMemory.Location = new System.Drawing.Point(20, 325);
            this.labelMemory.Name = "labelMemory";
            this.labelMemory.Size = new System.Drawing.Size(65, 12);
            this.labelMemory.TabIndex = 8;
            this.labelMemory.Text = "内存大小：";
            this.labelMemory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxMemory
            // 
            this.textBoxMemory.Location = new System.Drawing.Point(80, 322);
            this.textBoxMemory.Name = "textBoxMemory";
            this.textBoxMemory.Size = new System.Drawing.Size(75, 21);
            this.textBoxMemory.TabIndex = 10;
            // 
            // buttonSimpleUpdata
            // 
            this.buttonSimpleUpdata.Location = new System.Drawing.Point(455, 296);
            this.buttonSimpleUpdata.Name = "buttonSimpleUpdata";
            this.buttonSimpleUpdata.Size = new System.Drawing.Size(75, 23);
            this.buttonSimpleUpdata.TabIndex = 3;
            this.buttonSimpleUpdata.Text = "部分更新";
            this.buttonSimpleUpdata.UseVisualStyleBackColor = true;
            this.buttonSimpleUpdata.Click += new System.EventHandler(this.buttonSimpleUpdata_Click);
            // 
            // buttonFullUpdata
            // 
            this.buttonFullUpdata.Location = new System.Drawing.Point(535, 296);
            this.buttonFullUpdata.Name = "buttonFullUpdata";
            this.buttonFullUpdata.Size = new System.Drawing.Size(75, 23);
            this.buttonFullUpdata.TabIndex = 4;
            this.buttonFullUpdata.Text = "完整修复";
            this.buttonFullUpdata.UseVisualStyleBackColor = true;
            this.buttonFullUpdata.Click += new System.EventHandler(this.buttonFullUpdata_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(535, 321);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "结束游戏";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(455, 321);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "启动游戏";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonJavaPath
            // 
            this.buttonJavaPath.Location = new System.Drawing.Point(535, 272);
            this.buttonJavaPath.Name = "buttonJavaPath";
            this.buttonJavaPath.Size = new System.Drawing.Size(75, 23);
            this.buttonJavaPath.TabIndex = 5;
            this.buttonJavaPath.Text = "选择路径";
            this.buttonJavaPath.UseVisualStyleBackColor = true;
            this.buttonJavaPath.Click += new System.EventHandler(this.buttonJavaPath_Click);
            // 
            // progressBarMain
            // 
            this.progressBarMain.Location = new System.Drawing.Point(20, 240);
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(590, 23);
            this.progressBarMain.Step = 1;
            this.progressBarMain.TabIndex = 11;
            // 
            // buttonDeleteGame
            // 
            this.buttonDeleteGame.Location = new System.Drawing.Point(375, 296);
            this.buttonDeleteGame.Name = "buttonDeleteGame";
            this.buttonDeleteGame.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteGame.TabIndex = 16;
            this.buttonDeleteGame.Text = "删除版本";
            this.buttonDeleteGame.UseVisualStyleBackColor = true;
            this.buttonDeleteGame.Click += new System.EventHandler(this.buttonDeleteGame_Click);
            // 
            // labelMB
            // 
            this.labelMB.AutoSize = true;
            this.labelMB.Location = new System.Drawing.Point(163, 326);
            this.labelMB.Name = "labelMB";
            this.labelMB.Size = new System.Drawing.Size(17, 12);
            this.labelMB.TabIndex = 17;
            this.labelMB.Text = "MB";
            this.labelMB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxX64
            // 
            this.checkBoxX64.AutoSize = true;
            this.checkBoxX64.Location = new System.Drawing.Point(380, 326);
            this.checkBoxX64.Name = "checkBoxX64";
            this.checkBoxX64.Size = new System.Drawing.Size(66, 16);
            this.checkBoxX64.TabIndex = 18;
            this.checkBoxX64.Text = "X64模式";
            this.checkBoxX64.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(610, 220);
            this.richTextBox1.TabIndex = 19;
            this.richTextBox1.Text = "";
            // 
            // buttonUpdataCloak
            // 
            this.buttonUpdataCloak.Location = new System.Drawing.Point(535, 272);
            this.buttonUpdataCloak.Name = "buttonUpdataCloak";
            this.buttonUpdataCloak.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdataCloak.TabIndex = 20;
            this.buttonUpdataCloak.Text = "管理披风";
            this.buttonUpdataCloak.UseVisualStyleBackColor = true;
            this.buttonUpdataCloak.Click += new System.EventHandler(this.buttonUpdataCloak_Click);
            // 
            // buttonUpdataSkin
            // 
            this.buttonUpdataSkin.Location = new System.Drawing.Point(455, 272);
            this.buttonUpdataSkin.Name = "buttonUpdataSkin";
            this.buttonUpdataSkin.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdataSkin.TabIndex = 21;
            this.buttonUpdataSkin.Text = "管理皮肤";
            this.buttonUpdataSkin.UseVisualStyleBackColor = true;
            this.buttonUpdataSkin.Click += new System.EventHandler(this.buttonUpdataSkin_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 354);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(634, 22);
            this.statusStrip.TabIndex = 23;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(619, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backgroundWorkerFullDownLoad
            // 
            this.backgroundWorkerFullDownLoad.WorkerReportsProgress = true;
            this.backgroundWorkerFullDownLoad.WorkerSupportsCancellation = true;
            this.backgroundWorkerFullDownLoad.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFullDownLoad_DoWork);
            this.backgroundWorkerFullDownLoad.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerFullDownLoad_ProgressChanged);
            this.backgroundWorkerFullDownLoad.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerFullDownLoad_RunWorkerCompleted);
            // 
            // backgroundWorkerLog
            // 
            this.backgroundWorkerLog.WorkerReportsProgress = true;
            this.backgroundWorkerLog.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLog_DoWork);
            this.backgroundWorkerLog.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLog_ProgressChanged);
            // 
            // backgroundWorkerSimpleDownLoad
            // 
            this.backgroundWorkerSimpleDownLoad.WorkerReportsProgress = true;
            this.backgroundWorkerSimpleDownLoad.WorkerSupportsCancellation = true;
            this.backgroundWorkerSimpleDownLoad.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSimpleDownLoad_DoWork);
            this.backgroundWorkerSimpleDownLoad.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerSimpleDownLoad_ProgressChanged);
            this.backgroundWorkerSimpleDownLoad.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSimpleDownLoad_RunWorkerCompleted);
            // 
            // backgroundWorkerSkinOrCape
            // 
            this.backgroundWorkerSkinOrCape.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSkinOrCape_DoWork);
            this.backgroundWorkerSkinOrCape.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSkinOrCape_RunWorkerCompleted);
            // 
            // backgroundWorkerDeleteSkinOrCape
            // 
            this.backgroundWorkerDeleteSkinOrCape.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerDeleteSkinOrCape_DoWork);
            this.backgroundWorkerDeleteSkinOrCape.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerDeleteSkinOrCape_RunWorkerCompleted);
            // 
            // MieMieUpdata
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(634, 376);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.buttonUpdataSkin);
            this.Controls.Add(this.buttonUpdataCloak);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.checkBoxX64);
            this.Controls.Add(this.labelMB);
            this.Controls.Add(this.buttonDeleteGame);
            this.Controls.Add(this.progressBarMain);
            this.Controls.Add(this.buttonJavaPath);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonFullUpdata);
            this.Controls.Add(this.buttonSimpleUpdata);
            this.Controls.Add(this.textBoxMemory);
            this.Controls.Add(this.labelMemory);
            this.Controls.Add(this.textBoxJavaPath);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.labelJavaPath);
            this.Controls.Add(this.labelPlayerName);
            this.Controls.Add(this.comboBoxGame);
            this.Controls.Add(this.comboBoxLine);
            this.Controls.Add(this.labelChooseGame);
            this.Controls.Add(this.labelChooseLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MieMieUpdata";
            this.Text = "咩咩服务器更新程序 V0.0.5 ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MieMieUpdata_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelChooseLine;
        private System.Windows.Forms.Label labelChooseGame;
        private System.Windows.Forms.ComboBox comboBoxLine;
        private System.Windows.Forms.ComboBox comboBoxGame;
        private System.Windows.Forms.Label labelJavaPath;
        private System.Windows.Forms.Label labelPlayerName;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.TextBox textBoxJavaPath;
        private System.Windows.Forms.Label labelMemory;
        private System.Windows.Forms.TextBox textBoxMemory;
        private System.Windows.Forms.Button buttonSimpleUpdata;
        private System.Windows.Forms.Button buttonFullUpdata;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonJavaPath;
        private System.Windows.Forms.ProgressBar progressBarMain;
        private System.Windows.Forms.Button buttonDeleteGame;
        private System.Windows.Forms.Label labelMB;
        private System.Windows.Forms.CheckBox checkBoxX64;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonUpdataCloak;
        private System.Windows.Forms.Button buttonUpdataSkin;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFullDownLoad;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLog;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSimpleDownLoad;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSkinOrCape;
        private System.ComponentModel.BackgroundWorker backgroundWorkerDeleteSkinOrCape;



    }
}

