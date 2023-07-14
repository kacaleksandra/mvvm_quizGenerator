using System.Linq;
using QuizGen.DAL;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using quiz_generator;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;
using QuizGen.Models;

namespace QuizGen.ViewModel
{
    internal class MainWindow : ObservableObject
    {
        //public CommunityToolkit.Mvvm.Input.RelayCommand AddQuizCommand { get; private set; }
        public ObservableCollection<Quiz> Quizzes { get; private set; }
        public Quiz SelectedQuiz { get; set; }


        public ICommand AddQuizCommand { get; private set; }
        public ICommand EditQuizCommand { get; private set; }

        public MainWindow()
        {
            //AddQuizCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(this.AddQuiz);
            AddQuizCommand = new RelayCommand(AddQuiz);
            EditQuizCommand = new RelayCommand(EditQuiz);


            var context = QuizGenDbContext.GetInstance();
            var quizzes = context.Quizzes.ToList();
            Quizzes = new ObservableCollection<Quiz>(quizzes);

        }

        private void AddQuiz(object p)
        {
            var addQuiz = new quiz_generator.AddQuiz();
            var window = App.Current.MainWindow;
            window.Content = addQuiz;
            window.Show();
        }

        private void EditQuiz(object p)
        {
            if (SelectedQuiz is null) return;
            var editQuiz = new quiz_generator.EditQuiz(SelectedQuiz);
            Application.Current.MainWindow.Content = editQuiz;
        }
    }
}