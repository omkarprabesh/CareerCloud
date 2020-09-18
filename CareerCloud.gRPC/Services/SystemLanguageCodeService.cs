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
using static CareerCloud.gRPC.Protos.SystemLanguageCode;

namespace CareerCloud.gRPC.Services
{
    public class SystemLanguageCodeService: SystemLanguageCodeBase
    {
        private readonly SystemLanguageCodeLogic _logic;

        public SystemLanguageCodeService()
        {
            _logic = new SystemLanguageCodeLogic(new EFGenericRepository<SystemLanguageCodePoco>());
        }



        private SystemLanguageCodeReply FromPoco(SystemLanguageCodePoco poco)
        {
            return new SystemLanguageCodeReply()
            {
                LanguageId = poco.LanguageID,
                Name = poco.Name,
                NativeName = poco.NativeName,
            };

        }

        private List<SystemLanguageCodePoco> ToPoco(SystemLanguageCodes secLogs)
        {
            List<SystemLanguageCodePoco> pocos = new List<SystemLanguageCodePoco>();
            foreach (var reply in secLogs.SysCodes)
            {
                SystemLanguageCodePoco poco = new SystemLanguageCodePoco();

                poco.LanguageID = reply.LanguageId;
                poco.Name = reply.Name;
                poco.NativeName = reply.NativeName;
                
                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<SystemLanguageCodeReply> GetSystemLanguageCode(SLCIdRequest request, ServerCallContext context)
        {
            SystemLanguageCodePoco poco = _logic.Get(request.Id);
            return Task.FromResult(FromPoco(poco));
        }


        public override Task<SystemLanguageCodes> GetSystemLanguageCodes(Empty request, ServerCallContext context)
        {
            List<SystemLanguageCodePoco> pocos = _logic.GetAll();
            SystemLanguageCodes seclogs = new SystemLanguageCodes();
            foreach (var poco in pocos)
            {
                SystemLanguageCodeReply reply = FromPoco(poco);
                seclogs.SysCodes.Add(reply);
            }
            return Task.FromResult(seclogs);
        }

        public override Task<Empty> AddSystemLanguageCode(SystemLanguageCodes request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteSystemLanguageCode(SystemLanguageCodes request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateSystemLanguageCode(SystemLanguageCodes request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}
