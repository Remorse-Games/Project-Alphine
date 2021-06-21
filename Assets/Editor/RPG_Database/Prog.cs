using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using SFB;
using System;

namespace Calculator
{
    
    public class Token
    {
        double num1,num2;
        char op;

        public void initialize(double a, char b, double c) 
        {
            num1 = a; 
            op = b; 
            num2 = c; 
            return;
        }
        public double solve()
        {
            double result = 0;
            switch(op)
            {
                case '+' :
                    result = (num1 + num2);
                    break;
                case '-' :
                    result = (num1 - num2);
                    break;
                case '*' :
                    result = (num1 * num2);
                    break;
                case '/' :
                    result = (num1 / num2);
                    break;
            }

            return result;
        }
    }
    public class MainCalculation
    {

        [MenuItem("Extension/Calculator")]
        public static void main()
        {
            string expression = "70*(5+2)+100-(7+(2*5))";
            bool bracketFound = true;

            int openingBracketCount = 0, closingBracketCount = 0;
            expression = express(expression);
            for (int i = 0; i < expression.Length; i++)
            {
                if(expression[i] == '(')
                    openingBracketCount++;
                else if(expression[i]==')')
                    closingBracketCount++;
            }

            if(openingBracketCount!=closingBracketCount)
            {
                Debug.LogError("Expression Format Incorrect!");
                return;
            }
            else if(openingBracketCount == 0)
            {
                bracketFound = false;
            }
            while (bracketFound)
            {
                bracketFound = false;
                int lastClosingBracketIndex = expression.Length - 1;
                for (int i = expression.Length - 1; i >= 0; i--)
                {
                    if (expression[i] == ')')
                    {
                        lastClosingBracketIndex = i;
                    }
                    if (expression[i] == '(')
                    {
                        bracketFound = true;
                        string primaryCalculation = expression.Substring(i + 1, lastClosingBracketIndex - (i + 1));
                        List<double> numbers = new List<double>();
                        List<char> operators = new List<char>();
                        extract(express(primaryCalculation), ref numbers, ref operators);
                        multidiv(ref numbers, ref operators);
                        addsub(ref numbers, ref operators);

                        expression = expression.Remove(i, lastClosingBracketIndex - i + 1);
                        expression = expression.Insert(i, numbers[0].ToString());
                        Debug.Log(expression);
                    break;
                    }
                }
            }
            
            List<double> secNumber = new List<double>();
            List<char> secOperator = new List<char>();

            extract(express(expression), ref secNumber, ref secOperator);
            multidiv(ref secNumber, ref secOperator);
            addsub(ref secNumber, ref secOperator);
            
            //Result
            Debug.Log(secNumber[0]);
        }
        private static string express(string expressed) // Remove Space Found In String
        {
            for (int i = 0; i < expressed.Length; i++)
            {
                if (expressed[i] == 32)
                {
                    expressed = expressed.Remove(i, 1); i--;
                }
            }

            return expressed;
        }
        
        private static void extract(string extracted, ref List<double> numbers, ref List<char> operators) //Turns an expression into a list of numbers and operators.
        {
            List<int> digits = new List<int>();
            uint i,k; 
            int temp=0;

            for (i = 0; i <= extracted.Length; i++)
            {
                bool addNumber = false;
                if (i < extracted.Length)
                {
                    if(extracted[(int)i] >= '0' && extracted[(int)i] <= '9')
                        digits.Add(extracted[(int)i] - 48);
                    else
                        addNumber = true;
                }
                if(addNumber || i == extracted.Length)
                {
                    if (i < extracted.Length)
                    {
                        operators.Add(extracted[(int)i]);
                    }

                    for (k = 0; k < digits.Count; k++)
                    {
                        int count = 1;
                        for (int x = 0; x < digits.Count - 1 - k; x++)
                            count *= 10;
                        temp += digits[(int)k] * count;
                    }
                    numbers.Add(temp);
                    digits.Clear();
                    temp = 0;
                    addNumber = false;
                }
            }
            return;
        }

        private static void multidiv(ref List<double> numbers, ref List<char> operators) //Does multiplication and division.
        {
            Token tempp = new Token();
            double temp;
            uint i;
            for (i = 0; i < operators.Count; i++)
            {
                if (operators[(int)i] == '*' || operators[(int)i] == '/')
                {
                    tempp.initialize(numbers[(int)i], operators[(int)i], numbers[(int) i + 1]);
                    temp = tempp.solve();
                    if (operators.Count == 1)
                    {
                        operators.Clear();
                        numbers[0] = temp;
                        return;
                    }

                    else
                    {
                        operators.RemoveAt((int)i);
                        numbers.RemoveAt((int)i + 1);
                        numbers[(int)i] = temp;
                    }
                    --i;
                }
            }
        }
        private static void addsub(ref List<double> numbers, ref List<char> operators) //Does adding and subtracting.
        {
            Token tempp = new Token();
            double temp = numbers[0];
            while (true && operators.Count > 0)
            {
                if (operators[0] == '+' || operators[0] == '-')
                {
                    numbers.RemoveAt(0);
                    tempp.initialize(temp ,operators[0], numbers[0]);
                    temp = tempp.solve();

                    if (operators.Count == 1)
                    {
                        operators.Clear();
                        numbers[0] = temp;
                        return;
                    }

                    else
                        operators.RemoveAt(0);
                }
                else
                    break;
            }
        }

    }
}