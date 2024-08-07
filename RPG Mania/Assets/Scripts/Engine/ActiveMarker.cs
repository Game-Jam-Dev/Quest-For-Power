using System;

namespace Engine
{
    public class ActiveMarker
    {
        private readonly Action _onSet;
        private readonly Action _onCancel;
        private bool _active;

        public ActiveMarker(Action onSet, Action onCancel)
        {
            _onSet = onSet;
            _onCancel = onCancel;
        }
        
        public void Set(bool value)
        {
            if (!_active && value)
            {
                _active = true;
                _onSet.Invoke();
            }
        }
        
        public void Cancel()
        {
            if (_active)
            {
                _active = false;
                _onCancel.Invoke();
            }   
        }
    }
}