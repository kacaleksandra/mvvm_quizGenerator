using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizGen.ValueObjects;

namespace QuizGen.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public AnswerValue Value { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public Answer()
        {
        }
    }
}
