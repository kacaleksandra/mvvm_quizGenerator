using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGen.ValueObjects
{
    public class AnswerValue
    {
        public string Value { get; }

        public AnswerValue(string value)
        {
            Value = value;
        }

        public static implicit operator string(AnswerValue answerValue)
            => answerValue.Value;

        public static implicit operator AnswerValue(string value)
            => new(value);

        public override string ToString()
        {
            return Value;
        }

    }
}
