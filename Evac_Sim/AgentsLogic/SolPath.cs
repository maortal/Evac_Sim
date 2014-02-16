using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AgentsLogic
{
    public class SolPath : IEnumerable<State>
    {
        private int _size;
        private List<State> AgentPath { get; set; }
        private State _start;
        private State _goal;

        public SolPath()
        {
            _size = 0;
            AgentPath = new List<State>();
        }

        public void Add(State st)
        {
            AgentPath.Add(new State(st));
            if (_size == 0)
                this._start = AgentPath[0];
            _goal = AgentPath[_size++];
        }

        public void Reverse()
        {
            State tmp = _goal;
            _goal = _start;
            _start = tmp;
            AgentPath.Reverse();
        }

        public int SolLength()
        {
            return _size;
        }
        public State Solstart()
        {
            return _start;
        }
        public State Solgoal()
        {
            return _goal;
        }

        public IEnumerator<State> GetEnumerator()
        {
           return AgentPath.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AgentPath.GetEnumerator();
        }

        public override string ToString()
        {
            string res = null;
            foreach (State state in AgentPath)
            {
                res += state.ToString() + ", ";
            }
            return res.ToString();
        }
    }
}
