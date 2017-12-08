namespace SWD2Randomizer
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.gamepath = new System.Windows.Forms.TextBox();
            this.gamepathBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gamepathBrowseBtn = new System.Windows.Forms.Button();
            this.gamepathLabel = new System.Windows.Forms.Label();
            this.randomizeBtn = new System.Windows.Forms.Button();
            this.pathValid = new System.Windows.Forms.Label();
            this.restoreBtn = new System.Windows.Forms.Button();
            this.listDifficulty = new System.Windows.Forms.ListBox();
            this.textSeed = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.logText = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gamepath
            // 
            this.gamepath.Location = new System.Drawing.Point(75, 13);
            this.gamepath.Name = "gamepath";
            this.gamepath.ReadOnly = true;
            this.gamepath.ShortcutsEnabled = false;
            this.gamepath.Size = new System.Drawing.Size(395, 20);
            this.gamepath.TabIndex = 0;
            this.gamepath.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\SteamWorld Dig 2";
            // 
            // gamepathBrowserDialog
            // 
            this.gamepathBrowserDialog.Description = "Locate SWD2 base directory";
            this.gamepathBrowserDialog.ShowNewFolderButton = false;
            // 
            // gamepathBrowseBtn
            // 
            this.gamepathBrowseBtn.Location = new System.Drawing.Point(509, 13);
            this.gamepathBrowseBtn.Name = "gamepathBrowseBtn";
            this.gamepathBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.gamepathBrowseBtn.TabIndex = 1;
            this.gamepathBrowseBtn.Text = "Browse...";
            this.gamepathBrowseBtn.UseVisualStyleBackColor = true;
            this.gamepathBrowseBtn.Click += new System.EventHandler(this.browse_Click);
            // 
            // gamepathLabel
            // 
            this.gamepathLabel.AutoSize = true;
            this.gamepathLabel.Location = new System.Drawing.Point(6, 16);
            this.gamepathLabel.Name = "gamepathLabel";
            this.gamepathLabel.Size = new System.Drawing.Size(63, 13);
            this.gamepathLabel.TabIndex = 2;
            this.gamepathLabel.Text = "Game Path:";
            // 
            // randomizeBtn
            // 
            this.randomizeBtn.Location = new System.Drawing.Point(9, 45);
            this.randomizeBtn.Name = "randomizeBtn";
            this.randomizeBtn.Size = new System.Drawing.Size(75, 23);
            this.randomizeBtn.TabIndex = 3;
            this.randomizeBtn.Text = "Randomize";
            this.randomizeBtn.UseVisualStyleBackColor = true;
            this.randomizeBtn.Click += new System.EventHandler(this.randomizeBtn_Click);
            // 
            // pathValid
            // 
            this.pathValid.AutoSize = true;
            this.pathValid.Location = new System.Drawing.Point(6, 38);
            this.pathValid.Name = "pathValid";
            this.pathValid.Size = new System.Drawing.Size(64, 13);
            this.pathValid.TabIndex = 4;
            this.pathValid.Text = "Is path valid";
            // 
            // restoreBtn
            // 
            this.restoreBtn.Location = new System.Drawing.Point(90, 45);
            this.restoreBtn.Name = "restoreBtn";
            this.restoreBtn.Size = new System.Drawing.Size(75, 23);
            this.restoreBtn.TabIndex = 5;
            this.restoreBtn.Text = "Restore";
            this.restoreBtn.UseVisualStyleBackColor = true;
            this.restoreBtn.Click += new System.EventHandler(this.restoreBtn_Click);
            // 
            // listDifficulty
            // 
            this.listDifficulty.FormattingEnabled = true;
            this.listDifficulty.Items.AddRange(new object[] {
            "Casual",
            "Speedrunner"});
            this.listDifficulty.Location = new System.Drawing.Point(507, 13);
            this.listDifficulty.Name = "listDifficulty";
            this.listDifficulty.Size = new System.Drawing.Size(77, 30);
            this.listDifficulty.TabIndex = 6;
            // 
            // textSeed
            // 
            this.textSeed.Location = new System.Drawing.Point(238, 13);
            this.textSeed.MaxLength = 8;
            this.textSeed.Name = "textSeed";
            this.textSeed.Size = new System.Drawing.Size(100, 20);
            this.textSeed.TabIndex = 7;
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point(6, 16);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(226, 13);
            this.seedLabel.TabIndex = 8;
            this.seedLabel.Text = "Seed (leave blank to generate a random seed)";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(454, 16);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(47, 13);
            this.difficultyLabel.TabIndex = 9;
            this.difficultyLabel.Text = "Difficulty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gamepathLabel);
            this.groupBox1.Controls.Add(this.gamepath);
            this.groupBox1.Controls.Add(this.gamepathBrowseBtn);
            this.groupBox1.Controls.Add(this.pathValid);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 66);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.logText);
            this.groupBox2.Controls.Add(this.seedLabel);
            this.groupBox2.Controls.Add(this.textSeed);
            this.groupBox2.Controls.Add(this.randomizeBtn);
            this.groupBox2.Controls.Add(this.restoreBtn);
            this.groupBox2.Controls.Add(this.difficultyLabel);
            this.groupBox2.Controls.Add(this.listDifficulty);
            this.groupBox2.Location = new System.Drawing.Point(12, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(590, 253);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // logText
            // 
            this.logText.Location = new System.Drawing.Point(9, 79);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.Size = new System.Drawing.Size(575, 168);
            this.logText.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 349);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "SWD2 Randomizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox gamepath;
        private System.Windows.Forms.FolderBrowserDialog gamepathBrowserDialog;
        private System.Windows.Forms.Button gamepathBrowseBtn;
        private System.Windows.Forms.Label gamepathLabel;
        private System.Windows.Forms.Button randomizeBtn;
        private System.Windows.Forms.Label pathValid;
        private System.Windows.Forms.Button restoreBtn;
        private System.Windows.Forms.ListBox listDifficulty;
        private System.Windows.Forms.TextBox textSeed;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox logText;
    }
}

