using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace SWD2Randomizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listDifficulty.SelectedItem = "Speedrunner";
            this.gamepath.Text = Properties.Settings.Default.gamePath;
            checkPath();
            checkOrigFile();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            gamepathBrowserDialog.SelectedPath = this.gamepath.Text;
            if (gamepathBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                gamepath.Text = gamepathBrowserDialog.SelectedPath;
                Properties.Settings.Default.gamePath = this.gamepath.Text;
                Properties.Settings.Default.Save();
                checkPath();
                checkOrigFile();
            }
        }

        private void checkPath()
        {
            if (File.Exists(Path.Combine(Path.Combine(gamepath.Text, "Bundle"),"data01.impak")))
            {
                pathValid.Text = "Game path is valid!";
                pathValid.ForeColor = Color.Green;
                randomizeBtn.Enabled = true;
                restoreBtn.Enabled = true;
            }
            else
            {
                pathValid.Text = "Game path is not valid!";
                pathValid.ForeColor = Color.Red;
                randomizeBtn.Enabled = false;
                restoreBtn.Enabled = false;
            }
        }

        private void checkOrigFile()
        {
            if (File.Exists(Path.Combine(Path.Combine(gamepath.Text, "Bundle"), "data01.impak.orig")))
            {
                restoreBtn.Enabled = true;
                randomizeBtn.Enabled = false;
            }
            else
            {
                restoreBtn.Enabled = false;
                randomizeBtn.Enabled = true;
            }
        }

        private void restoreBtn_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            restoreBtn.Text = "Restoring...";
            restoreBtn.Enabled = false;

            logText.Clear();

            string basedir = Path.Combine(gamepath.Text, "Bundle");
            try
            {
                File.Copy(Path.Combine(basedir, "data01.impak.orig"), Path.Combine(basedir, "data01.impak"), true);
                File.Delete(Path.Combine(basedir, "data01.impak.orig"));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Does not have access to the game directory", "Access error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            logText.Text = "Restoring game data finished!";
            logText.Update();

            restoreBtn.Text = "Restore";
            this.UseWaitCursor = false;

            checkOrigFile();
        }

        private void randomizeBtn_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            randomizeBtn.Text = "Randomizing...";
            randomizeBtn.Enabled = false;

            logText.Clear();

            string baseDir = Path.Combine(gamepath.Text, "Bundle");
            string zipFile = Path.Combine(Directory.GetCurrentDirectory(), "data01.impak.zip");
            string extractedDir = Path.Combine(Directory.GetCurrentDirectory(), "data01");

            StringBuilder logsb = new StringBuilder();

            /* Generate a new seed if blank */
            if (string.IsNullOrWhiteSpace(textSeed.Text))
            {
                SetSeedBasedOnDifficulty();
            }

            string difficulty = GetRandomizerDifficulty();

            logsb.Append("Selecting difficulty is ").AppendLine(difficulty);
            logText.Text = logsb.ToString();
            logText.Update();

            string seed = GetSeed();
            if (string.IsNullOrWhiteSpace(seed))
            {
                randomizeBtn.Text = "Randomize";
                randomizeBtn.Enabled = true;
                this.UseWaitCursor = false;
                return;
            }

            int parsedSeed;
            if (!int.TryParse(seed, out parsedSeed))
            {
                MessageBox.Show("Seed must be numeric or blank.", "Seed Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logsb.AppendLine("Seed must be numeric or blank.");
                logText.Text = logsb.ToString();
                logText.Update();
                randomizeBtn.Text = "Randomize";
                randomizeBtn.Enabled = true;
                this.UseWaitCursor = false;
                return;
            }

            logsb.Append("Seed is ").AppendLine(textSeed.Text);
            logText.Text = logsb.ToString();
            logText.Update();

            try
            {
                File.Copy(Path.Combine(baseDir, "data01.impak"), Path.Combine(baseDir, "data01.impak.orig"), true);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Does not have access to the game directory", "Access error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                randomizeBtn.Text = "Randomize";
                randomizeBtn.Enabled = true;
                this.UseWaitCursor = false;
                return;
            }

            /* Check if previous zipfile exists and delete it */
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }

            /* Check if previous extracted directory exists and delete it */
            if (Directory.Exists(extractedDir))
            {
                Directory.Delete(extractedDir, true);
            }

            try
            {
                File.Move(Path.Combine(baseDir, "data01.impak"), zipFile);
            }
            catch (IOException)
            {
                MessageBox.Show("Cannot move the game file, you need to close the game", "Access error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                randomizeBtn.Text = "Randomize";
                this.UseWaitCursor = false;
                checkOrigFile();
                return;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Does not have access to the current directory", "Access error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                randomizeBtn.Text = "Randomize";
                this.UseWaitCursor = false;
                checkOrigFile();
                return;
            }

            logsb.AppendLine("Extracting game data...");
            logText.Text = logsb.ToString();
            logText.Update();

            ZipFile.ExtractToDirectory(zipFile, extractedDir);

            List<Location> locations;
            if (difficulty == "Speedrunner")
            {
                locations = new LocationsSpeedrunner().Locations;
            }
            else
            {
                locations = new LocationsCasual().Locations;
            }

            Randomizer randomizer = new Randomizer(extractedDir, parsedSeed, locations);

            logsb.Append("Randomize");
            logText.Text = logsb.ToString();
            logText.Update();

            int ret = 0;
            for (int i = 0; i < 1000; i++)
            {
                ret = randomizer.Randomize();
                if (ret == -1)
                {
                    logsb.AppendLine("").AppendLine("Error occured: mismatch number of upgrades");
                    logText.Text = logsb.ToString();
                    logText.Update();
                    break;
                }
                if (ret == 1)
                {
                    logsb.AppendLine("").AppendLine("Successfully found a correct case!");
                    logText.Text = logsb.ToString();
                    logText.Update();

                    /* Save upgrade locations into a file */
                    string cheatFile = Path.Combine(Directory.GetCurrentDirectory(), textSeed.Text + ".txt");
                    using (StreamWriter outputFile = new StreamWriter(cheatFile))
                    {
                        foreach (var location in locations)
                        {
                            if (location.Type == SWD2Randomizer.Location.RandomizeType.Upgrade)
                            {
                                outputFile.WriteLine(location.Name + " location contains " + location.Grant);
                            }
                        }
                        foreach (var location in locations)
                        {
                            if (location.Type == SWD2Randomizer.Location.RandomizeType.Area)
                            {
                                outputFile.WriteLine("Door to " + location.Name + " contains " + location.Grant);
                            }
                        }
                    }

                    logsb.AppendLine("Created cheat file at " + cheatFile);
                    logText.Text = logsb.ToString();
                    logText.Update();

                    break;
                }
                logsb.Append(".");
                logText.Text = logsb.ToString();
                logText.Update();
            }

            if (ret == 0)
            {
                logsb.AppendLine("").AppendLine("Could not find any correct case...");
                logText.Text = logsb.ToString();
            }

            logsb.AppendLine("Compress into game data...");
            logText.Text = logsb.ToString();
            logText.Update();

            ZipFile.CreateFromDirectory(extractedDir, Path.Combine(baseDir, "data01.impak"));

            logsb.AppendLine("Done!");
            logText.Text = logsb.ToString();
            logText.Update();

            randomizeBtn.Text = "Randomize";
            checkOrigFile();
            UseWaitCursor = false;

        }

        private void SetSeedBasedOnDifficulty()
        {
            switch (listDifficulty.SelectedItem.ToString())
            {
                case "Casual":
                    textSeed.Text = string.Format("C{0:0000000}", (new SeedRandom()).Next(10000000));
                    break;
                default:
                    textSeed.Text = string.Format("S{0:0000000}", (new SeedRandom()).Next(10000000));
                    break;
            }
        }

        private string GetRandomizerDifficulty()
        {
            string difficulty = "Speedrunner";

            if (textSeed.Text.ToUpper().Contains("C"))
            {
                difficulty = "Casual";
            }
            else if (textSeed.Text.ToUpper().Contains("S"))
            {
                difficulty = "Speedrunner";
            }

            listDifficulty.SelectedItem = difficulty;

            return difficulty;
        }

        private string GetSeed()
        {
            if (textSeed.Text.ToUpper().Contains("C"))
            {
                return textSeed.Text.ToUpper().Replace("C", "");
            }
            else if (textSeed.Text.ToUpper().Contains("S"))
            {
                return textSeed.Text.ToUpper().Replace("S", "");
            }
            MessageBox.Show("The seed string is unrecognized.", "Seed Difficulty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return "";
        }
    }
}
