using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.SecurityLoginLog;

namespace CareerCloud.gRPC.Services
{
    public class SecurityLoginsLogService: SecurityLoginLogBase
    {
        private readonly SecurityLoginsLogLogic _logic;

        public SecurityLoginsLogService()
        {
            _logic = new SecurityLoginsLogLogic(new EFGenericRepository<SecurityLoginsLogPoco>());
        }



        private SecurityLoginLogReply FromPoco(SecurityLoginsLogPoco poco)
        {
            return new SecurityLoginLogReply()
            {
                Id = poco.Id.ToString(),
                Login = poco.Login.ToString(),
                SourceIP= poco.SourceIP,
                LogonDate = poco.LogonDate.ToTimestamp(),
                IsSuccessful = poco.IsSuccesful,
                           };

        }

        private List<SecurityLoginsLogPoco> ToPoco(SecurityLoginLogs secLogs)
        {
            List<SecurityLoginsLogPoco> pocos = new List<SecurityLoginsLogPoco>();
            foreach (var reply in secLogs.SecLoginLogs)
            {
                SecurityLoginsLogPoco poco = new SecurityLoginsLogPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Login = Guid.Parse(reply.Login);
                poco.SourceIP = reply.SourceIP;
                poco.LogonDate = reply.LogonDate.ToDateTime();
                poco.IsSuccesful = reply.IsSuccessful;
                
                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<SecurityLoginLogReply> GetSecurityLoginLog(SLLIdRequest request, ServerCallContext context)
        {
            SecurityLoginsLogPoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }
        

        public override Task<SecurityLoginLogs> GetSecurityLoginLogs(Empty request, ServerCallContext context)
        {
            List<SecurityLoginsLogPoco> pocos = _logic.GetAll();
            SecurityLoginLogs seclogs = new SecurityLoginLogs();
            foreach (var poco in pocos)
            {
                SecurityLoginLogReply reply = FromPoco(poco);
                seclogs.SecLoginLogs.Add(reply);
            }
            return Task.FromResult(seclogs);
        }

        public override Task<Empty> AddSecurityLoginLog(SecurityLoginLogs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteSecurityLoginLog(SecurityLoginLogs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateSecurityLoginLog(SecurityLoginLogs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}
