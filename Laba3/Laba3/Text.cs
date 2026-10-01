using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}

public class Sentence
{
    [XmlElement("word", typeof(Word))]
    [XmlElement("punctuation", typeof(Punctuation))]
    public List<SentenceItem> Items { get; set; } = new List<SentenceItem>();

    public override string ToString()
    {
        return string.Join("", Items.Select((item, index) =>
            (item is Word && index > 0 && !(Items[index - 1] is Punctuation p && p.Value == "-") ? " " : "") + item.Value)).Trim();
    }
}

public abstract class SentenceItem
{
    [XmlText]
    public string Value { get; set; }
}

public class Word : SentenceItem { }

public class Punctuation : SentenceItem { }