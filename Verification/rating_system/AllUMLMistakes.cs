using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.rating_system {
    public enum ALL_MISTAKES {
        // AD
        MORE_THAN_ONE_INIT,
        NO_FINAL,
        NO_INITIAL,
        NO_ACTIVITIES,
        MORE_THAN_ONE_OUT,
        DO_NOT_HAVE_ALT,
        ONLY_ONE_ALT,
        NO_OUT,
        NO_IN,
        NO_PARTION,
        REPEATED_NAME,
        SAME_TARGET,
        OUT_NOT_IN_ACT,
        NEXT_DECISION,
        HAS_1_IN,
        // лексические
        FORBIDDEN_ELEMENT,
        SMALL_LETTER,
        NO_NAME,
        NOT_NOUN,
        END_WITH_QUEST,
        HAVE_NOT_QUEST,
        REPEATED_ALT,
        HAVE_EMPTY_ALT,
        HAVE_MARK,
        STRANGE_SYMBOL,
        EMPTY_SWIMLANE,
        // синтаксис 2
        TWO_TOKENS,
        DEAD_ROAD,
        MANY_TOKENS_IN_END,
        COULD_NOT_REACH_FINAL,
        FINAL_COLOR_TOKEN
        //CD

        //UCD
    }
}
