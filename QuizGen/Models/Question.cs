using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizGen.ValueObjects;

namespace QuizGen.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public QuestionBody Body { get; set; }
        public List<Answer> Answers { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public Question()
        {
        }

    }
}
