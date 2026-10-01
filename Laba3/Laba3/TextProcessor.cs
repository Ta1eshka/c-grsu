using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

public class TextProcessor
{
    public Text TextData { get; set; }

    public TextProcessor(Text textData)
    {
        TextData = textData;
    }

    // 1. Предложения по возрастанию количества слов
    public void PrintSentencesByWordCount()
    {
        var sorted = TextData.Sentences.OrderBy(s => s.Items.OfType<Word>().Count());
        Console.WriteLine("--- По возрастанию количества слов ---");
        foreach (var s in sorted) Console.WriteLine(s.ToString());
    }

    // 2. Предложения по возрастанию длины
    public void PrintSentencesByLength()
    {
        var sorted = TextData.Sentences.OrderBy(s => s.ToString().Length);
        Console.WriteLine("--- По возрастанию длины ---");
        foreach (var s in sorted) Console.WriteLine(s.ToString());
    }

    // 3. Слова заданной длины в вопросительных предложениях
    public IEnumerable<string> FindWordsInInterrogative(int wordLength)
    {
        return TextData.Sentences
            .Where(s => s.Items.OfType<Punctuation>().Any(p => p.Value.Contains("?")))
            .SelectMany(s => s.Items.OfType<Word>())
            .Where(w => w.Value.Length == wordLength)
            .Select(w => w.Value.ToLower())
            .Distinct();
    }
}