using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGen.Services
{
    public class Encryption
    {
            public static string Decrypt(string value)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in value)
                {
                    if (!char.IsLetter(c))
                    {
                        sb.Append(c);
                        continue;
                    }

                    char shiftedChar = (char)(c - 4);

                    if (char.IsUpper(c))
                    {
                        shiftedChar = shiftedChar < 'A' ? (char)(shiftedChar + 26) : shiftedChar;
                    }
                    else
                    {
                        shiftedChar = shiftedChar < 'a' ? (char)(shiftedChar + 26) : shiftedChar;
                    }

                    sb.Append(shiftedChar);
                }

                return sb.ToString();
            }

            public static string Encrypt(string value)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in value)
                {
                    if (!char.IsLetter(c))
                    {
                        sb.Append(c);
                        continue;
                    }

                    char shiftedChar = (char)(c + 4);

                    if (char.IsUpper(c))
                    {
                        shiftedChar = shiftedChar > 'Z' ? (char)(shiftedChar - 26) : shiftedChar;
                    }
                    else
                    {
                        shiftedChar = shiftedChar > 'z' ? (char)(shiftedChar - 26) : shiftedChar;
                    }

                    sb.Append(shiftedChar);
                }

                return sb.ToString();
            }
        }
    }

