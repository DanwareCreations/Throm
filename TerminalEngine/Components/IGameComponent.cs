using System.Collections;
using System.Collections.Generic;

namespace TerminalEngine.Components {

    public interface IGameComponent { }

    public class ComponentList<C> : IReadOnlyList<C> where C : IGameComponent {

        private IList<C> _comps = new List<C>();

        public ComponentList(IList list) : this((IList<C>)list) { }
        public ComponentList(IList<C> list) => _comps = list;

        public C this[int index] {
            get => _comps[index];
            set => _comps[index] = value;
        }
        public int Count => _comps.Count;
        public IEnumerator<C> GetEnumerator() => _comps.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _comps.GetEnumerator();
    }


}
