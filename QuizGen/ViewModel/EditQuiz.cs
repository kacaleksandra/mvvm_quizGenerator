using QuizGen.DAL;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using QuizGen.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuizGen;

namespace QuizGen.ViewModel
{
    internal class EditQuiz : ObservableObject
    {
        public EditQuiz(Quiz selectedQuiz)
        {
            var context = QuizGenDbContext.GetInstance();
            var quiz = context.Quizzes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == selectedQuiz.Id);
            if (quiz == null) return;
            _quiz = quiz;
            AddQuestionCommand = new RelayCommand(AddQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
            BackToMainWindowCommand = new RelayCommand(BackToMainWindow);
            EditQuestionCommand = new RelayCommand(EditQuestion);
            RemoveQuizCommand = new RelayCommand(RemoveQuiz);
            var firstQuestion = quiz.Questions?.FirstOrDefault();
            CorrectAnswers = firstQuestion?.Answers.Select(x => x.IsCorrect).ToArray() ?? new bool[4];
            //CorrectAnswer = new ObservableCollection<bool>(ResolveCorrectAnswer(firstQuestion?.Correct));
            Answers = new ObservableCollection<string>(firstQuestion?.Answers.Select(x => x.Value.ToString()).ToArray() ?? CreateAnswers());
            Questions = new ObservableCollection<Question>(_quiz.Questions ?? new List<Question>());
            Question = firstQuestion?.Body ?? string.Empty;
            Name = _quiz?.Name ?? string.Empty;
            //_correctAnswer = firstQuestion?.Correct ?? 0;

            SelectedQuestion = firstQuestion;

        }

        private string _question;
        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get
            {
                return _selectedQuestion;
            }
            set
            {
                ResolveQuestion(value);
                _selectedQuestion = value;
            }
        }
        public bool[] CorrectAnswers { get; set; } = new bool[4];
        public ObservableCollection<string> Answers { get; set; }
        public ObservableCollection<Question> Questions { get; set; }

        private readonly Quiz _quiz;

        public ICommand AddQuestionCommand { get; private set; }
        public ICommand EditQuestionCommand { get; private set; }
        public ICommand RemoveQuestionCommand { get; private set; }
        public ICommand RemoveQuizCommand { get; private set; }
        public ICommand BackToMainWindowCommand { get; private set; }


        private IEnumerable<string> CreateAnswers()
        {
            return new string[4];
        }

        private IEnumerable<bool> ResolveCorrectAnswer(int? correctIndex)
        {
            if (correctIndex == null) correctIndex = 1;
            var arr = new bool[4];
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == correctIndex - 1) arr[i] = true;
                else arr[i] = false;
            }
            return arr;

        }

        private void ResolveQuestion(Question question)
        {
            Answers.Clear();
            if (question?.Answers?.Count == 4)
            {
                question?.Answers.ForEach(answer => Answers.Add(answer.Value));
            } else
            {
                Answers = new ObservableCollection<string>(CreateAnswers());
                OnPropertyChanged(nameof(Answers));
            }
            Question = question?.Body ?? string.Empty;
            OnPropertyChanged(nameof(Question));
            CorrectAnswers = question?.Answers.Select(x => x.IsCorrect).ToArray() ?? new bool[4];
            OnPropertyChanged(nameof(CorrectAnswers));
        }

        private void BackToMainWindow(object p)
        {
            var mainWindow = new quiz_generator.MainWindow();
            Application.Current.MainWindow.Content = mainWindow.Content;
        }

        private void CleanupForm()
        {
            Answers = new ObservableCollection<string>(CreateAnswers());
            OnPropertyChanged(nameof(Answers));
            Question = string.Empty;
        }

        private IEnumerable<Answer> ResolveAnswers(Question question)
        {
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
            return answers;
        }

        private void AddQuestion(object p)
        {
            if (string.IsNullOrWhiteSpace(Question) || Answers.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Pola nie mogą być puste.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var question = new Question
            {
                Id = System.Guid.NewGuid(),
                Body = Question,
                QuizId = _quiz.Id
            };

            if (!CorrectAnswers.Any(a => a))
            {
                MessageBox.Show("Przynajmniej jedna odpowiedź musi być poprawna.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            question.Answers = ResolveAnswers(question).ToList();

            Questions.Add(question);
            
            var context = QuizGenDbContext.GetInstance();
            context.Questions.Add(question);
            context.SaveChanges();

            // Uncomment if needed - cleanup form behaviour
            //CleanupForm();
        }

        private void EditQuestion(object p)
        {
            if (SelectedQuestion is null) return;
            SelectedQuestion.Body = Question;
            //SelectedQuestion.Correct = _correctAnswer;
            for(int i = 0; i < Answers.Count; i++)
            {
                SelectedQuestion.Answers[i].Value = Answers[i];
                SelectedQuestion.Answers[i].IsCorrect = CorrectAnswers[i];
            }
            var context = QuizGenDbContext.GetInstance();
            context.Questions.Update(SelectedQuestion);
            context.SaveChanges();
            var question = Questions.SingleOrDefault(x => x.Id == SelectedQuestion.Id);

            // Required to force refresh table
            Questions = new ObservableCollection<Question>(Questions);
            OnPropertyChanged(nameof(Questions));
        }

        private void RemoveQuiz(object p)
        {
            var context = QuizGenDbContext.GetInstance();
            context.Quizzes.Remove(_quiz);
            context.SaveChanges();
            BackToMainWindow(null);
        }

        void RemoveQuestion(object p)
        {
            if (SelectedQuestion is null) return;
            var context = QuizGenDbContext.GetInstance();
            context.Questions.Remove(SelectedQuestion);
            context.SaveChanges();
            Questions.Remove(SelectedQuestion);
        }

    }
}
