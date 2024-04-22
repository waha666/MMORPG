using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class HelloWorld:Singleton<HelloWorld>
    {
        public void Init()
        {
           
        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }

        void OnFirstTestRequest(NetConnection<NetSession> sender, FirstTestRequest firstTest) 
        {
            Log.InfoFormat("OnFirstTestRequest HelloWorld:{0}", firstTest.Helloworld);
        }
    }
}
