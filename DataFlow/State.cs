using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXT_Hermes
{
    public class Context
    {
        private State _state = null;

        private List<State> states = new List<State>();
        public Context(State state)
        {
            this.TransitionTo(state);
        }
        public State GetState()
        {
            return this._state;
        }
        public void TransitionTo(State state)
        {
            if(!Equals(_state, default) && !states.Contains(state))
               states.Add(this._state);
            
            this._state = state;
            this._state.SetContext(this);
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " " + state);
        }
        public void Request<T>() 
        {
            this._state.Handle<T>();
        }
        public T Query<T>() where T : State
        {
            return (T)states.FirstOrDefault(__state => __state.GetType() == typeof(T));
            //return !Equals(states.FirstOrDefault(__state => __state.GetType() == typeof(T)), default);
        }
        public void Remove<T>() where T : State
        {
            State state = states.FirstOrDefault(__state => __state.GetType() == typeof(T));
            if (!Equals(state, default))
                states.Remove(state);
        }
        public void Clear()
        {
            states.Clear();
        }
    }

    public abstract class State
    {
        protected Context _context;
        public void SetContext(Context context)
        {
            this._context = context;
        }
        public abstract void Handle<T>();
    }

    public class NotConnectedStatus : State
    {
        public override void Handle<T>()
        {
            if(typeof(T) == typeof(ServiceDescription))
            {
                this._context.Clear();
                this._context.TransitionTo(new ServiceDescriptionStatus());
            }    
        }
    }

    public class ServiceDescriptionStatus : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(ServiceDescription))
            {
                this._context.Clear();
                this._context.TransitionTo(new Reconnect());
                //this._context.TransitionTo(new NotAvailableNotReadyStatus());
            }
            else if(typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class Reconnect : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(UpstreamLogicControlReset))
                this._context.TransitionTo(new NotAvailableNotReadyStatus());
            else if (typeof(T) == typeof(DownstreamLogicControlReset))
                this._context.TransitionTo(new NotAvailableNotReadyStatus());
        }
    }

    public class NotAvailableNotReadyStatus : State
    {
        public override void Handle<T>()
        {
            if(typeof(T) == typeof(MachineReady))
               this._context.TransitionTo(new MachineReadyStatus());
            else if(typeof(T) == typeof(BoardAvailable))
               this._context.TransitionTo(new BoardAvailableStatus());
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class BoardAvailableStatus : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(RevokeBoardAvailable))
            {
                this._context.Remove<BoardAvailableStatus>();
                State state = this._context.Query<MachineReadyStatus>();
                if (!Equals(state, default))
                    this._context.TransitionTo(state);  
                else
                    this._context.TransitionTo(new NotAvailableNotReadyStatus());
            }
            else if(typeof(T) == typeof(MachineReady))
                this._context.TransitionTo(new AvailableAndReadyStatus());
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class MachineReadyStatus : State
    {
        public override void Handle<T>()
        {
            if(typeof(T) == typeof(BoardAvailable))
               this._context.TransitionTo(new AvailableAndReadyStatus());
            else if(typeof(T) == typeof(RevokeMachineReady))
            {
                this._context.Remove<MachineReadyStatus>();
                State state = this._context.Query<BoardAvailableStatus>();
                if (!Equals(state, default))
                    this._context.TransitionTo(state);                
                else
                    this._context.TransitionTo(new NotAvailableNotReadyStatus());
            }
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class AvailableAndReadyStatus : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(RevokeMachineReady))
            {
                this._context.Remove<MachineReadyStatus>();
                this._context.TransitionTo(new BoardAvailableStatus());
            }
            else if(typeof(T) == typeof(RevokeBoardAvailable))
            {
                this._context.Remove<BoardAvailableStatus>();
                this._context.TransitionTo(new MachineReadyStatus());
            }
            else if(typeof(T) == typeof(StartTransport))
                this._context.TransitionTo(new TransportingStatus());
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class TransportingStatus : State
    {
        public override void Handle<T>()
        {
            if(typeof(T) == typeof(StopTransport))
               this._context.TransitionTo(new TransportStoppedStatus());
            else if (typeof(T) == typeof(TransportFinished))
                this._context.TransitionTo(new TransportFinishedStatus());
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class TransportFinishedStatus : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(StopTransport))
            {
                this._context.Clear();
                this._context.TransitionTo(new NotAvailableNotReadyStatus());
            }
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }

    public class TransportStoppedStatus : State
    {
        public override void Handle<T>()
        {
            if (typeof(T) == typeof(TransportFinished))
            {
                this._context.Clear();
                this._context.TransitionTo(new NotAvailableNotReadyStatus());
            }
            else if (typeof(T) == typeof(Notification))
                this._context.TransitionTo(new NotConnectedStatus());
        }
    }
}
