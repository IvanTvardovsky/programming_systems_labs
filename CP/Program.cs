using System;

public class Program
{
    public static void Main()
    {

        string source = "if a > 10:\n\tprint(\"AAA\")\nif z > 20:\n\t\tprint(\"ZZZ\")\n\telse:\n\t\tprint(\"!Z\")\nelse:\n\tprint(\"!A\")";

        Console.WriteLine(source);

        Console.WriteLine("********************************************************");

        string[] lines = SplitString(source);
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine("********************************************************");

        string ans = "";
        int linesIterator = 0;
        int mCounter = 1;
        POLIZ(ref ans, lines, ref linesIterator, ref mCounter);

        Console.WriteLine(ans);

    }

    public static void POLIZ(ref string ans, in string[] lines, ref int linesIterator, ref int mCounter)
    {
        if (lines[linesIterator].StartsWith("if"))
        {
            ans += " " + MakePostfix(TrimIf(lines[linesIterator]));
            ans += " m";
            ans += mCounter.ToString();
            int prevCounter = mCounter;
            ++mCounter;
            ans += " BF";
            ++linesIterator;

            bool isInIf = true;

            while (linesIterator < lines.Length)
            {
                if (lines[linesIterator].StartsWith("if"))
                {
                    POLIZ(ref ans, lines, ref linesIterator, ref mCounter);
                }
                else if (lines[linesIterator].StartsWith("else"))
                {
                    isInIf = false;
                    ans += " m";
                    ans += mCounter.ToString();
                    //++mCounter; сломается если есть if в else
                    ans += " BRL";
                    ans += " m";
                    ans += prevCounter.ToString();
                    ans += " DEFL";

                }
                else
                {
                    if (isInIf)
                    {
                        ans += " " + lines[linesIterator];
                    }
                    else
                    {
                        if (linesIterator < lines.Length - 1 && lines[linesIterator + 1].StartsWith("else:"))
                        {
                            ans += " " + lines[linesIterator];
                            ans += " m";
                            ans += mCounter.ToString();
                            ans += " DEFL";
                            ++mCounter;
                            return;
                        }
                        else
                        {
                            ans += " " + lines[linesIterator];
                        }

                        if (linesIterator == lines.Length - 1)
                        {
                            ans += " m";
                            ans += mCounter.ToString();
                            ans += " DEFL";
                            ++mCounter;
                        }
                    }
                }

                if (linesIterator == lines.Length - 1)
                {
                    return;
                }

                ++linesIterator;
            }
            

        } else
        {
            throw new ArgumentException("Invalid input format. Expected format: 'if ...:'");
        }
    }

    public static string[] SplitString(string input)
    {
        string[] lines = input.Split('\n'); // Разбиваем строку на массив строк по символу '\n'

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim(); // Удаляем лишние пробелы в начале и конце каждой строки

            if (lines[i].StartsWith("\t")) // Удаляем символы '\t' в начале каждой строки
            {
                lines[i] = lines[i].Substring(1);
            }
        }

        return lines;
    }

    public static string MakePostfix(string input)
    {
        string[] parts = input.Split(' '); // Разбиваем строку на массив подстрок по пробелам

        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid input format. Expected format: 'A лог. выражение B'");
        }

        string a = parts[0];
        string oper = parts[1];
        string b = parts[2];

        return string.Join(" ", a, b, oper); // Соединяем подстроки с пробелами между ними
    }

    public static string TrimIf(string input)
    {
        int startIndex = input.IndexOf("if ") + 3; // Находим начальный индекс выражения
        int endIndex = input.LastIndexOf(":"); // Находим конечный индекс выражения

        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        {
            string expression = input.Substring(startIndex, endIndex - startIndex).Trim(); // Извлекаем выражение и обрезаем лишние пробелы
            return expression;
        }

        throw new ArgumentException("Invalid input format. Expected format: 'if <выражение>:'");
    }

}
