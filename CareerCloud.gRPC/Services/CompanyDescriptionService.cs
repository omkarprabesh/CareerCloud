using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.CompanyDescription;

namespace CareerCloud.gRPC.Services
{
    public class CompanyDescriptionService: CompanyDescriptionBase

    {
        private readonly CompanyDescriptionLogic _logic;

        public CompanyDescriptionService()
        {
            _logic = new CompanyDescriptionLogic(new EFGenericRepository<CompanyDescriptionPoco>());
        }



        private CompanyDescriptionReply FromPoco(CompanyDescriptionPoco poco)
        {
            return new CompanyDescriptionReply()
            {
                Id = poco.Id.ToString(),
                Company = poco.Company.ToString(),
                LanguageId = poco.LanguageId,
                CompanyName = poco.CompanyName,
                CompanyDescription = poco.CompanyDescription,

               TimeStamp = ByteString.CopyFrom(poco.TimeStamp)
            };

        }

        private List<CompanyDescriptionPoco> ToPoco(CompanyDescriptions comDes)
        {
            List<CompanyDescriptionPoco> pocos = new List<CompanyDescriptionPoco>();
            foreach (var reply in comDes.ComDes)
            {
                CompanyDescriptionPoco poco = new CompanyDescriptionPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Company = Guid.Parse(reply.Company);
                poco.LanguageId = reply.LanguageId;
                poco.CompanyName = reply.CompanyName;
                poco.CompanyDescription = reply.CompanyDescription;
                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<CompanyDescriptionReply> GetCompanyDescription(Protos.CDIdRequest request, ServerCallContext context)
        {
            CompanyDescriptionPoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<CompanyDescriptions> GetCompanyDescriptions(Empty request, ServerCallContext context)
        {
            List<CompanyDescriptionPoco> pocos = _logic.GetAll();
            CompanyDescriptions comDes = new CompanyDescriptions();
            foreach (var poco in pocos)
            {
                CompanyDescriptionReply reply = FromPoco(poco);
                comDes.ComDes.Add(reply);
            }
            return Task.FromResult(comDes);
        }

        public override Task<Empty> AddCompanyDescription(CompanyDescriptions request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteCompanyDescription(CompanyDescriptions request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateCompanyDescription(CompanyDescriptions request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}

