using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string link = textBox2.Text;
                Console.WriteLine(link);

                // Create FolderBrowserDialog
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select Download Location";

                    // Show the dialog and get result
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected path
                        string downloadPath = folderDialog.SelectedPath;

                        var youtube = new YoutubeClient();
                        var videoUrl = link;  // Using the link from richTextBox1
                        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
                        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                        var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                        // Combine the selected path with the filename
                        string fullPath = Path.Combine(downloadPath, $"video.{streamInfo.Container}");

                        // Download to the selected location
                        await youtube.Videos.Streams.DownloadAsync(streamInfo, fullPath);

                        MessageBox.Show("Download completed successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
