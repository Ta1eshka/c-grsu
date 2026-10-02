using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {

        string inputFilePath = "input.txt";

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Файл '{inputFilePath}' не найден!");
            Console.WriteLine("Создаю файл с примером текста для первого запуска...");
            string defaultContent = "It is a cold morning, но день будет теплым.\n" +
                                   "What do you think about this и почему ты молчишь?\n" +
                                   "The cat спит on the sofa, а верный пес охраняет дом.";
            File.WriteAllText(inputFilePath, defaultContent, Encoding.UTF8);
        }

        string rawText = File.ReadAllText(inputFilePath, Encoding.UTF8);

        Text text = TextParser.Parse(rawText);
        var processor = new TextProcessor(text);

        processor.PrintSentencesByWordCount();

        processor.PrintSentencesByLength();

        Console.WriteLine("\n--- Слова длины 4 в вопросах (без дублей) ---");
        foreach (var w in processor.FindWordsInInterrogative(4))
        {
            Console.WriteLine(w);
        }

        processor.RemoveWordsStartingWithConsonant(4);
        Console.WriteLine("\n--- Текст после удаления слов длины 4 на согласную ---");
        foreach (var sentence in processor.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        processor.ReplaceWordsInSentence(0, 4, "[ЗАМЕНА]");
        Console.WriteLine("\n--- Предложение 0 после замены ---");
        Console.WriteLine(processor.TextData.Sentences[0]);

        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (File.Exists("stopwords_ru.txt"))
            foreach (var line in File.ReadAllLines("stopwords_ru.txt")) stopWords.Add(line.Trim());
        if (File.Exists("stopwords_en.txt"))
            foreach (var line in File.ReadAllLines("stopwords_en.txt")) stopWords.Add(line.Trim());

        processor.RemoveStopWords(stopWords);

        Console.WriteLine("\n--- Текст после удаления стоп-слов ---");
        foreach (var sentence in processor.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        processor.ExportToXml("result.xml");
        Console.WriteLine("\nУспешно экспортировано в result.xml");
    }
}