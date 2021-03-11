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
}

