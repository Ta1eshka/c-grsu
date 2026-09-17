using System;
using System.IO;

namespace CatAndMouse
{
    public enum GameState
    {
        Start,
        End
    }

    public class Game
    {
        public static string InputFile = "1.ChaseData.txt";
        public static string OutputFile = "PursuitLog.txt";

        private int _size;
        private readonly Player _cat;
        private readonly Player _mouse;
        private GameState _state;

        public Game() : this(0)
        {
        }

        public Game(int size)
        {
            _size = size;
            _cat = new Player("Cat", size);
            _mouse = new Player("Mouse", size);
            _state = GameState.Start;
        }

        public int Size
        {
            get { return _size; }
        }

        public Player Cat
        {
            get { return _cat; }
        }

        public Player Mouse
        {
            get { return _mouse; }
        }



        public void Run()
        {
            using (StreamReader reader = new StreamReader(InputFile))
            using (StreamWriter writer = new StreamWriter(OutputFile))
            {
                ReadBoardSize(reader);
                WriteHeader(writer);
                ProcessCommands(reader, writer);
                WriteSummary(writer);
            }
        }

        private void ReadBoardSize(StreamReader reader)
        {
            string line = reader.ReadLine();
            int size;
            if (line != null && int.TryParse(line.Trim(), out size) && size > 0)
            {
                _size = size;
                _cat.SetBoardSize(size);
                _mouse.SetBoardSize(size);
            }
        }

        private void ProcessCommands(StreamReader reader, StreamWriter writer)
        {
            string line;
            while (_state != GameState.End && (line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                {
                    continue;
                }

                char command = parts[0][0];
                if (command == 'P')
                {
                    DoPrintCommand(writer);
                }
                else
                {
                    int steps = int.Parse(parts[1]);
                    DoMoveCommand(command, steps);

                    if (IsCatch())
                    {
                        EndGameWithCatch();
                    }
                }
            }

            if (_state != GameState.End)
            {
                EndGameEvaded();
            }
        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("Cat and Mouse");
            writer.WriteLine();
            writer.WriteLine("Cat Mouse  Distance");
            writer.WriteLine(new string('-', 19));
        }

        private void DoMoveCommand(char command, int steps)
        {
            switch (command)
            {
                case 'M':
                    Mouse.Move(steps);
                    break;
                case 'C':
                    Cat.Move(steps);
                    break;
            }
        }

        private void DoPrintCommand(StreamWriter writer)
        {
            string catColumn = " " + (Cat.IsInGame ? Cat.Location.ToString() : "??").PadLeft(2);
            string mouseColumn = (Mouse.IsInGame ? Mouse.Location.ToString() : "??").PadLeft(2);

            string row = catColumn + "    " + mouseColumn;
            if (Cat.IsInGame && Mouse.IsInGame)
            {
                row += GetDistance().ToString().PadLeft(10);
            }

            writer.WriteLine(row);
        }

        private int GetDistance()
        {
            return Math.Abs(Cat.Location - Mouse.Location);
        }

        private bool IsCatch()
        {
            return Cat.IsInGame && Mouse.IsInGame && Cat.Location == Mouse.Location;
        }

        private void EndGameWithCatch()
        {
            Cat.MarkWinner();
            Mouse.MarkLoser();
            _state = GameState.End;
        }

        private void EndGameEvaded()
        {
            Cat.MarkLoser();
            Mouse.MarkWinner();
            _state = GameState.End;
        }

        private void WriteSummary(StreamWriter writer)
        {
            writer.WriteLine(new string('-', 19));
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("Distance traveled:   Mouse    Cat");
            writer.WriteLine(new string(' ', 21) + Mouse.DistanceTraveled.ToString().PadLeft(5)
                + new string(' ', 4) + Cat.DistanceTraveled.ToString().PadLeft(3));
            writer.WriteLine();

            if (Cat.State == State.Winner)
            {
                writer.WriteLine("Mouse caught at: " + Mouse.Location);
            }
            else
            {
                writer.WriteLine("Mouse evaded Cat");
            }
        }
    }
}