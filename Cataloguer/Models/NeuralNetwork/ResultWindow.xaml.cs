using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace lab2
{
    public partial class ResultWindow : Window
    {
        public int CorrectDigit => Regex.IsMatch(CorrectAnswerTextBox.Text, @"^\d*$") ? Int32.Parse(CorrectAnswerTextBox.Text) : -1;

        public ResultWindow(int assumptiveDigit)
        {
            InitializeComponent();
            NetworkAnswerLabel.Content = $"The neural network thinks the digit is {assumptiveDigit}";
            CorrectAnswerTextBox.Text = assumptiveDigit.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
