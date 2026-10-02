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

        Text text1 = TextParser.Parse(rawText);
        var processor1 = new TextProcessor(text1);
        Text text2 = TextParser.Parse(rawText);
        var processor2 = new TextProcessor(text2);
        Text text3 = TextParser.Parse(rawText);
        var processor3 = new TextProcessor(text3);
        Text text4 = TextParser.Parse(rawText);
        var processor4 = new TextProcessor(text4);
        Text text5 = TextParser.Parse(rawText);
        var processor5 = new TextProcessor(text5);
        Text text6 = TextParser.Parse(rawText);
        var processor6 = new TextProcessor(text6);
        Text text7 = TextParser.Parse(rawText);
        var processor7 = new TextProcessor(text7);


        processor1.PrintSentencesByWordCount();

        processor2.PrintSentencesByLength();

        Console.WriteLine("\n--- Слова длины 4 в вопросах (без дублей) ---");
        foreach (var w in processor3.FindWordsInInterrogative(4))
        {
            Console.WriteLine(w);
        }

        processor4.RemoveWordsStartingWithConsonant(4);
        Console.WriteLine("\n--- Текст после удаления слов длины 4 на согласную ---");
        foreach (var sentence in processor4.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        processor5.ReplaceWordsInSentence(0, 4, "[ЗАМЕНА]");
        Console.WriteLine("\n--- Предложение 0 после замены ---");
        Console.WriteLine(processor5.TextData.Sentences[0]);

        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (File.Exists("stopwords_ru.txt"))
            foreach (var line in File.ReadAllLines("stopwords_ru.txt")) stopWords.Add(line.Trim());
        if (File.Exists("stopwords_en.txt"))
            foreach (var line in File.ReadAllLines("stopwords_en.txt")) stopWords.Add(line.Trim());

        processor6.RemoveStopWords(stopWords);

        Console.WriteLine("\n--- Текст после удаления стоп-слов ---");
        foreach (var sentence in processor6.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        processor7.ExportToXml("result.xml");
        Console.WriteLine("\nУспешно экспортировано в result.xml");
    }
}