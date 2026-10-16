using System.Linq;
using System.Text.RegularExpressions;

public static class TextParser
{
    public static Text Parse(string rawText)
    {
        var textObject = new Text();

        var sentenceMatches = Regex.Matches(rawText, @"[^.!?]+[.!?]*");

        foreach (Match sentenceMatch in sentenceMatches)
        {
            var sentence = new Sentence();

            var tokenMatches = Regex.Matches(sentenceMatch.Value, @"[a-zA-Zа-яА-ЯёЁ0-9'-]+|[^\w\s]+");

            foreach (Match tokenMatch in tokenMatches)
            {
                int absoluteIndex = sentenceMatch.Index + tokenMatch.Index;
                int lineNumber = 1 + rawText.Substring(0, absoluteIndex).Count(c => c == '\n');

                if (Regex.IsMatch(tokenMatch.Value, @"^[a-zA-Zа-яА-ЯёЁ0-9'-]+$"))
                {
                    sentence.Items.Add(new Word { Value = tokenMatch.Value, LineNumber = lineNumber });
                }
                else
                {
                    sentence.Items.Add(new Punctuation { Value = tokenMatch.Value, LineNumber = lineNumber });
                }
            }

            if (sentence.Items.Count > 0)
                textObject.Sentences.Add(sentence);
        }

        return textObject;
    }
}