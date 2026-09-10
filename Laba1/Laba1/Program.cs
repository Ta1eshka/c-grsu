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

        static string RLEncoding(string amino_acids)
        {
            if (string.IsNullOrEmpty(amino_acids)) return "";
            StringBuilder sb = new StringBuilder();

            int count = 1;
            for (int i = 1; i <= amino_acids.Length; i++)
            {
                if (i < amino_acids.Length && amino_acids[i] == amino_acids[i - 1] && count < 9)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        sb.Append(count).Append(amino_acids[i - 1]);
                    }
                    else
                    {
                        for (int j = 0; j < count; j++) sb.Append(amino_acids[i - 1]);
                    }
                    count = 1;
                }
            }
            return sb.ToString();
        }
    }
}
