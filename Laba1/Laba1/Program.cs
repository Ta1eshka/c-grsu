using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneticSearch
{
    struct GeneticData
    {
        public string protein;
        public string organism;
        public string amino_acids;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string seqFile = "sequences.txt";
            string cmdFile = "commands.txt";
            string outFile = "genedata.txt";

            if (!File.Exists(seqFile) || !File.Exists(cmdFile))
            {
                Console.WriteLine("Отсутствуют входные файлы (sequences.txt или commands.txt).");
                return;
            }

            List<GeneticData> sequences = new List<GeneticData>();
            foreach (string line in File.ReadLines(seqFile))
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 3)
                {
                    sequences.Add(new GeneticData
                    {
                        protein = parts[0].Trim(),
                        organism = parts[1].Trim(),
                        amino_acids = RLDecoding(parts[2].Trim())
                    });
                }
            }

            using (StreamWriter writer = new StreamWriter(outFile, false, Encoding.UTF8))
            {
                writer.WriteLine("Шмат Кирилл");
                writer.WriteLine("Genetic Searching");
                string separator = new string('-', 74);
                writer.WriteLine(separator);

                int opNumber = 1;

                foreach (string line in File.ReadLines(cmdFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');
                    string command = parts[0].Trim().ToLower();
                    string opNumStr = opNumber.ToString("D3");

                    if (command == "search" && parts.Length >= 2)
                    {
                        ExecuteSearch(writer, opNumStr, parts, sequences);
                    }

                    else if (command == "diff" && parts.Length >= 3)
                    {
                        ExecuteDiff(writer, opNumStr, parts, sequences);
                    }

                    else if (command == "mode" && parts.Length >= 2)
                    {
                        ExecuteMode(writer, opNumStr, parts, sequences);
                    }

                    writer.WriteLine(separator);
                    opNumber++;
                }
            }
            Console.WriteLine("Анализ завершен. Результаты сохранены в genedata.txt.");
        }

        static void ExecuteSearch(StreamWriter writer, string opNumStr, string[] parts, List<GeneticData> sequences)
        {
            string rawParam = parts[1].Trim();
            string searchSeq = RLDecoding(rawParam);

            writer.WriteLine($"{opNumStr}\tsearch\t{rawParam}");
            writer.WriteLine("organism\tprotein");

            bool found = false;
            foreach (var data in sequences)
            {
                if (data.amino_acids.Contains(searchSeq))
                {
                    writer.WriteLine($"{data.organism}\t{data.protein}");
                    found = true;
                }
            }

            if (!found)
            {
                writer.WriteLine("NOT FOUND");
            }
        }

        static void ExecuteDiff(StreamWriter writer, string opNumStr, string[] parts, List<GeneticData> sequences)
        {
            string p1Name = parts[1].Trim();
            string p2Name = parts[2].Trim();

            writer.WriteLine($"{opNumStr}\tdiff\t{p1Name}\t{p2Name}");
            writer.WriteLine("amino-acids difference:");

            int index1 = sequences.FindIndex(s => s.protein == p1Name);
            int index2 = sequences.FindIndex(s => s.protein == p2Name);

            if (index1 == -1 && index2 == -1)
            {
                writer.WriteLine($"MISSING: {p1Name}, {p2Name}");
            }
            else if (index1 == -1)
            {
                writer.WriteLine($"MISSING: {p1Name}");
            }
            else if (index2 == -1)
            {
                writer.WriteLine($"MISSING: {p2Name}");
            }
            else
            {
                string seq1 = sequences[index1].amino_acids;
                string seq2 = sequences[index2].amino_acids;

                int diffCount = 0;
                int minLen = Math.Min(seq1.Length, seq2.Length);

                for (int i = 0; i < minLen; i++)
                {
                    if (seq1[i] != seq2[i]) diffCount++;
                }
                diffCount += Math.Abs(seq1.Length - seq2.Length);

                writer.WriteLine(diffCount);
            }
        }

        static void ExecuteMode(StreamWriter writer, string opNumStr, string[] parts, List<GeneticData> sequences)
        {
            string pName = parts[1].Trim();

            writer.WriteLine($"{opNumStr}\tmode\t{pName}");
            writer.WriteLine("amino-acid occurs:");

            int index = sequences.FindIndex(s => s.protein == pName);

            if (index == -1)
            {
                writer.WriteLine($"MISSING: {pName}");
            }
            else
            {
                string seq = sequences[index].amino_acids;

                Dictionary<char, int> counts = new Dictionary<char, int>();
                foreach (char c in seq)
                {
                    if (counts.ContainsKey(c)) counts[c]++;
                    else counts[c] = 1;
                }

                int maxCount = 0;
                char bestChar = 'Z';

                foreach (var kvp in counts)
                {
                    if (kvp.Value > maxCount)
                    {
                        maxCount = kvp.Value;
                        bestChar = kvp.Key;
                    }
                    else if (kvp.Value == maxCount && kvp.Key < bestChar)
                    {
                        bestChar = kvp.Key;
                    }
                }

                writer.WriteLine($"{bestChar}\t{maxCount}");
            }
        }
        static string RLDecoding(string amino_acids)
        {
            if (string.IsNullOrEmpty(amino_acids)) return "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < amino_acids.Length; i++)
            {
                if (char.IsDigit(amino_acids[i]))
                {
                    int count = amino_acids[i] - '0';
                    i++;
                    if (i < amino_acids.Length)
                    {
                        char c = amino_acids[i];
                        for (int j = 0; j < count; j++) sb.Append(c);
                    }
                }
                else
                {
                    sb.Append(amino_acids[i]);
                }
            }
            return sb.ToString();
        }
    }
}