﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizGen.Models;

namespace quiz_generator
{
    /// <summary>
    /// Logika interakcji dla klasy EditQuiz.xaml
    /// </summary>
    public partial class EditQuiz : Page
    {
        public EditQuiz(Quiz quiz)
        {
            InitializeComponent();
            DataContext = new QuizGen.ViewModel.EditQuiz(quiz);           
        }

        private void TextBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
