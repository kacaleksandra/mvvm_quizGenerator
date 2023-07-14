using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuizGen.ValueObjects
{
    internal class Question : ObservableObject
    {
        public string Value { get; set; }

        public Question(string value)
        { 
           Value = value;
           OnPropertyChanged(nameof(Question));
        }

        public static implicit operator string(Question question)
            => question.Value;
        
        public static implicit operator Question(string value)
            => new(value);

        public override string ToString()
        {
            return Value;
        }

    }
}
