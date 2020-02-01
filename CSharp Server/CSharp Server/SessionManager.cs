using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
namespace CSharp_Server
{


    public class SessionManager
    {

        readonly public static SessionManager instance = new SessionManager();

        private Dictionary<int, Session> SessionDic = new Dictionary<int, Session>();
        private int sessionCount = 0;

        private ConcurrentBag<Session> SessionPool = new ConcurrentBag<Session>();
        private SessionManager()
        {
            
        }

        private void IncreaseSessionPool()
        {
            for (int i = 0; i < 100; i++)
            {
                SessionPool.Add(new Session());
            }
        }

        public Session Alloc()
        {
            if(!SessionPool.TryTake(out var session)){
                IncreaseSessionPool();
                Alloc();
            }

            AddSession(session);
            return session;
        }

        public Session FindSession(int id)
        {
            SessionDic.TryGetValue(id, out var session);
            return session;
        }
        public void AddSession(Session session)
        {
            Interlocked.Increment(ref sessionCount);
            session.id = sessionCount;
            SessionDic.Add(session.id, session);
        }

        public void Return2SessionPool(Session session)
        {
            SessionDic.Remove(session.id);
            SessionPool.Add(session);
        }



    }
}
