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

    // 4. Удалить слова заданной длины, начинающиеся с согласной
    public void RemoveWordsStartingWithConsonant(int length)
    {
        string consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ";
        foreach (var sentence in TextData.Sentences)
        {
            sentence.Items.RemoveAll(item =>
                item is Word word &&
                word.Value.Length == length &&
                consonants.Contains(char.ToLower(word.Value[0])));
        }
    }

    // 5. Замена слов заданной длины в указанном предложении
    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string substring)
    {
        if (sentenceIndex >= 0 && sentenceIndex < TextData.Sentences.Count)
        {
            var sentence = TextData.Sentences[sentenceIndex];
            foreach (var item in sentence.Items)
            {
                if (item is Word word && word.Value.Length == wordLength)
                {
                    word.Value = substring;
                }
            }
        }
    }

    // 6. Удаление стоп-слов
    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (var sentence in TextData.Sentences)
        {
            sentence.Items.RemoveAll(item =>
                item is Word word && stopWords.Contains(word.Value.ToLower()));
        }
    }
}