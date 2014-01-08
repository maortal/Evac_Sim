using System;

namespace Evac_Sim.DataStructures
{
    public interface IBinaryHeapItem : IComparable<IBinaryHeapItem>
    {
        /// <summary>
        /// the index of the item in the binary heap
        /// </summary>
        int getIndexInHeap();
        void setIndexInHeap(int index);
    }
}
