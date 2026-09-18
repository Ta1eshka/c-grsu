using System;

namespace CatAndMouse
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Game.InputFile = ChooseInputFile();
            Game game = new Game();
            game.Run();
        }

        private static string ChooseInputFile()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*ChaseData*");
            if (files.Length == 0)
            {
                Console.WriteLine("Файлы ChaseData не найдены, использую {0}", Game.InputFile);
                return Game.InputFile;
            }

            Console.WriteLine("Выберите входной файл:");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("  {0}. {1}", i + 1, Path.GetFileName(files[i]));
            }
            Console.WriteLine("  0. По умолчанию ({0})", Game.InputFile);
            Console.Write("Ваш выбор: ");

            string answer = Console.ReadLine();
            int choice;
            if (int.TryParse(answer, out choice) && choice >= 1 && choice <= files.Length)
            {
                return files[choice - 1];
            }

            return Game.InputFile;
        }
    }
}