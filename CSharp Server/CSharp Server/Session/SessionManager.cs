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

        public ConcurrentDictionary<long, Session> closedSessions = new ConcurrentDictionary<long, Session>();
        public ConcurrentDictionary<long, Session> openedSessions = new ConcurrentDictionary<long, Session>();

        public Dictionary<SessionType, Func<Session>> createSessionDic = new Dictionary<SessionType, Func<Session>>();

        private long sessionid = 0;

        private ConcurrentBag<Session> SessionPool = new ConcurrentBag<Session>();
        private ProtocolHandler protocolHandler;
        private SessionManager()
        {
        }

        public void Regist_Create_Session_Func(SessionType Type, Func<Session> createSessionFunc)
        {
            Func<Session> func = null;
            if (false == createSessionDic.TryGetValue(Type, out func))
            {
                createSessionDic.Add(Type, createSessionFunc);
            }
            else
            {
                Console.WriteLine("Create Session Func alreadt Exist !");
            }

        }

        public void Make_Session(SessionType sessionType, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Session closedSession = createSessionDic[sessionType]();
                long id = Interlocked.Increment(ref sessionid);
                closedSessions.TryAdd(id, closedSession);
            }
        }

        
        public void Push_OpenSessions(Session session)
        {
            openedSessions.TryAdd(session.id, session);
        }
        
        public void Push_CloseSessions(Session session)
        {
            closedSessions.TryAdd(session.id, session);
        }

        public Session Pop_OpenSessions(Session session)
        {
            Session remove_session = null;
            if(openedSessions.TryRemove(session.id, out remove_session))
            {

            }

            return remove_session;
        }

        public Session Pop_ClosedSessions(Session session)
        {

            Session remove_session = null;
            if(closedSessions.TryRemove(session.id, out remove_session))
            {

            }

            return remove_session;
        }
    }
}
