using System;

namespace TerminalEngine {

    public interface IUpdatable {
        bool Update(TimeSpan deltaTime);
    }

}
