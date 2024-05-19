using PasswordResetApp.Models;
using System;
using System.IO;
using System.Windows;

namespace PasswordResetApp
{
    public partial class MainWindow : Window
    {
        private string encryptedPassword;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click_1(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            encryptedPassword = PasswordManager.EncryptPassword(password);
            File.WriteAllText("encryptedPassword.txt", encryptedPassword);
            MessageBox.Show("Password encrypted and saved.");
        }

        private void BruteForceButton_Click_1(object sender, RoutedEventArgs e)
        {
            int maxThreads = int.TryParse(ThreadCountTextBox.Text, out var threads) ? threads : 4;
            var startTime = DateTime.Now;
            string foundPassword = BruteForce.BruteForceAttack(encryptedPassword, maxThreads);
            var elapsedTime = DateTime.Now - startTime;

            if (foundPassword != null)
            {
                ResultTextBlock.Text = $"Password found: {foundPassword} in {elapsedTime.TotalSeconds} seconds.";
            }
            else
            {
                ResultTextBlock.Text = "Password not found";
            }
        }
    }
}
