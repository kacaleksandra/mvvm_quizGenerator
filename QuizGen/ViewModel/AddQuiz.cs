using QuizGen.DAL;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using QuizGen.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using quiz_generator;
using System.Windows;


namespace QuizGen.ViewModel
{
    internal class AddQuiz : ObservableObject
    {

        public string Question { get; set; }


        public string Name { get; set; }

        public Question SelectedQuestion { get; set; }

        public ObservableCollection<string> Answers { get; set; }
        public ObservableCollection<Question> Questions { get; set; }

        public ICommand AddQuestionCommand { get; private set; }
        public ICommand SaveQuizCommand { get; private set; }
        public ICommand RemoveQuestionCommand { get; private set; }

        public bool[] CorrectAnswers { get; set; } = new bool[4];


        public AddQuiz()
        {
            AddQuestionCommand = new RelayCommand(AddQuestion);
            SaveQuizCommand = new RelayCommand(SaveQuiz);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
            Answers = new ObservableCollection<string>(new string[4]);
            Questions = new ObservableCollection<Question>(new List<Question>());
        }

        private void AddQuestion(object p)
        {
            if (string.IsNullOrWhiteSpace(Question) || Answers.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Pola nie mogą być puste.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!CorrectAnswers.Any(a => a))
            {
                MessageBox.Show("Przynajmniej jedna odpowiedź musi być poprawna.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var question = new Question
            {
                Id = System.Guid.NewGuid(),
                Body = Question,
            };

            var answers = new List<Answer>();
            for (int i = 0; i < Answers.Count; i++)
            {
                var answer = new Answer
                {
                    Value = Answers[i],
                    QuestionId = question.Id,
                    IsCorrect = CorrectAnswers[i]

                };
                answers.Add(answer);
            }

            question.Answers = answers;

            Questions.Add(question);
        }

        void RemoveQuestion(object p)
        {
            if (SelectedQuestion is null) return;
            Questions.Remove(SelectedQuestion);
        }

        private void SaveQuiz(object p)
        {
            var context = QuizGenDbContext.GetInstance();

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = Name
            };

            if (Name is null) return;

            try
            {
                context.Database.BeginTransaction();
                context.Quizzes.Add(quiz);
                var questions = Questions.ToList();
                questions.ForEach(x =>
                {
                    x.QuizId = quiz.Id;
                    context.Questions.Add(x);
                });
                context.SaveChanges();
                context.Database.CommitTransaction();
            }
            catch (Exception)
            {
                context.Database.RollbackTransaction();
            }

            var mainWindow = new quiz_generator.MainWindow();
            Application.Current.MainWindow.Content = mainWindow.Content;
        }

    }
}
