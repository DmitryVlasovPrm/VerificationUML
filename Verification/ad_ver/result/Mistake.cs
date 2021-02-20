using ActivityDiagramVer.verification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer
{
    class Mistake
    {
        private String mistake;         // ошибка и описание эл-та, в кот она совершена
        private Level level;            // серьезность ошибки
        private int id;                 // ид ошибки
        private int seriousness;        // серьезность ошибки (в int)
        private int x = -1, y = -1;     // координаты отсутствуют

        public Mistake(String mistake, Level level, int id)
        {
            this.mistake = mistake;
            this.level = level;
            this.id = id;
        }
        public Mistake(String mistake, Level level, int id, int x, int y)
        {
            this.mistake = mistake;
            this.level = level;
            this.id = id;

            
        }

        public String getMistake()
        {
            return mistake;
        }

        public Level getLevel()
        {
            return level;
        }

        public int getId()
        {
            return id;
        }

        // 1 - warning, 2 - error, 3 - fatal
        public int getSeriousness() { return level == Level.EASY ? 1 : level == Level.HARD ? 2 : 3; }
    }
}
