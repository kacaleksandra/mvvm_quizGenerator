using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGen.ValueObjects
{
    public class QuestionBody
    {
        public string Value { get; }

        public QuestionBody(string value)
        {
            Value = value;
        }

        public static implicit operator string(QuestionBody questionBody)
            => questionBody.Value;

        public static implicit operator QuestionBody(string value)
            => new(value);

        public override string ToString()
        {
            return Value;
        }

    }
}
