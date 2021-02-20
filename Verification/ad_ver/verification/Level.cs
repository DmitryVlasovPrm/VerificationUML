using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.verification
{
    public enum Level
    {
        EASY,       // некоторые ошибки будут выдаваться с надписью WARNING!
        HARD,
        FATAL       // любая ошибка считается за серьезную
    }

        
    public class LevelAdapter
    {
        public String toString(Level level)
        {
            switch (level)
            {
                case Level.EASY: return "[WARNING!]";
                case Level.HARD: return "[EXCEPTION!] ";
                case Level.FATAL: return "[FATAL]";
                default: throw new ArgumentException();
            }
        }
    }
    
}

