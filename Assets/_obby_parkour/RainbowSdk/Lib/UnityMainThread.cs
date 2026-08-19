using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection; 

namespace DVAH
{   
    internal class UnityMainThread : MonoBehaviour
    {
        private static UnityMainThread _wkr; 
        Queue<Action> jobs = new Queue<Action>();
        #if UNITY_EDITOR
        [SerializeField]
        List<string> _callbackHistory = new List<string>();
        MethodInfo _trace;
        #endif

        Action currentInvoke;
      
        internal static UnityMainThread wkr
        {
            get
            {
                if (_wkr == null)
                {
                    Debug.LogWarning(CONSTANT.Prefix + "==> Create UnitymaintThread obj<==");
                    GameObject g = new GameObject();
                   
                    _wkr = g.AddComponent<UnityMainThread>();
                    g.name = "UnityMainTHread";

                    DontDestroyOnLoad(g);
                }
                
                return _wkr;
            }
        } 
       
        protected virtual void Awake()
        {
            if (_wkr != null && _wkr.GetInstanceID() != this.GetInstanceID())
                Destroy(this);
            else
                _wkr = this.GetComponent<UnityMainThread>();

            DontDestroyOnLoad(this);
        }

        void Update()
        {
            while (jobs.Count > 0)
                try
                {
                    currentInvoke = jobs.Dequeue(); 
                    #if UNITY_EDITOR  
                    _trace = currentInvoke.GetMethodInfo();
                    _callbackHistory.Add($"{_trace.DeclaringType}->{_trace}");
                    #endif
                    currentInvoke?.Invoke();
                }
                catch (Exception e)
                { 
                    Debug.LogError(CONSTANT.Prefix + $"==>: {e.GetBaseException()}<==");
                }
        }

        internal void AddJob(params Action[] newJobs)
        {
            lock (jobs)
            {
                foreach(Action job in newJobs){
                    jobs.Enqueue(job);
                } 
            }
        }
    }
}