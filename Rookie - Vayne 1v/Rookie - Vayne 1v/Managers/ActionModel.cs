using System;

namespace EOQVayne.Managers
{
    struct ActionModel
    {
        public float Time;
        public Func<bool> PreConditionFunc;
        public Func<bool> ConditionToRemoveFunc;
        public Action ComboAction;
    }
}
