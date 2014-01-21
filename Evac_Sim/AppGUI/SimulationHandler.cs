using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AppGUI
{
    internal class SimulationHandler
    {
        private MainForm main;
        private Dictionary<Agent, IEnumerator<State>> ageEnumerators = new Dictionary<Agent, IEnumerator<State>>();


        public SimulationHandler(Dictionary<State, Agent>.ValueCollection agentsList, MainForm mainForm)
        {
            main = mainForm;
            foreach (Agent agent in agentsList.Where(agent => agent.visible))
            {
                ageEnumerators[agent] = agent.Agentsolution.GetEnumerator();
                ageEnumerators[agent].MoveNext();
            }
        }

        public void BeginSimulation()
        {
            Dictionary<Agent, State> agentPrevMove = new Dictionary<Agent, State>();
            int timestamp = 0;
            while (ageEnumerators.Count > 0)
            {
                main.backgroundWorker2.ReportProgress(1);
                System.Threading.Thread.Sleep(300);
                
                foreach (Agent agnent in agentPrevMove.Keys)
                {
                    if (agentPrevMove[agnent] == agnent.GoalState) Utils.fillState(agentPrevMove[agnent], Color.White);
                }
                Dictionary<Agent, IEnumerator<State>> tmpenuEnumerators = new Dictionary<Agent, IEnumerator<State>>();
                foreach (Agent agn in ageEnumerators.Keys)
                {
                    State currentState = ageEnumerators[agn].Current;
                    if (currentState == null) continue;
                    if (!tryPaint(currentState, agn.GetAgColor()))
                    {
                        tmpenuEnumerators[agn] = ageEnumerators[agn];
                        continue;
                    }
                    if (agentPrevMove.ContainsKey(agn) && agentPrevMove[agn] != null)
                        Utils.fillState(agentPrevMove[agn], Color.White);
                    agentPrevMove[agn] = currentState;
                    if (ageEnumerators[agn].MoveNext())
                    {
                        if (agentPrevMove[agn].Equals(ageEnumerators[agn].Current))
                            ageEnumerators[agn].MoveNext();
                        tmpenuEnumerators[agn] = ageEnumerators[agn];
                    }
                }
                ageEnumerators = tmpenuEnumerators;
            }
            main.backgroundWorker2.ReportProgress(1);
        }

        private bool tryPaint(State currentState, Color cellcolor)
        {
                if (!Utils.isStateFree(currentState)) 
                    return false;       //conflict in simulation!
                Utils.fillState(currentState, cellcolor);
            
            return true;
        }
    }
}
