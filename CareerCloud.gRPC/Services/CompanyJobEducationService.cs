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
using static CareerCloud.gRPC.Protos.CompanyJobEducation;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobEducationService: CompanyJobEducationBase
    {
        
        private readonly CompanyJobEducationLogic _logic;

        public CompanyJobEducationService()
        {
            _logic = new CompanyJobEducationLogic(new EFGenericRepository<CompanyJobEducationPoco>());
        }



        private CompanyJobEducationReply FromPoco(CompanyJobEducationPoco poco)
        {
            return new CompanyJobEducationReply()
            {
                Id = poco.Id.ToString(),
                Job = poco.Job.ToString(),
                Major = poco.Major,
                Importance = poco.Importance,

                TimeStamp = ByteString.CopyFrom(poco.TimeStamp)
            };

        }

        private List<CompanyJobEducationPoco> ToPoco(CompanyJobEducations comJobs)
        {
            List<CompanyJobEducationPoco> pocos = new List<CompanyJobEducationPoco>();
            foreach (var reply in comJobs.ComJob)
            {
                CompanyJobEducationPoco poco = new CompanyJobEducationPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Job = Guid.Parse(reply.Job);
                poco.Major = reply.Major;
                poco.Importance = (short)reply.Importance;
                
                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<CompanyJobEducationReply> GetCompanyJobEducation(Protos.CJIdRequest request, ServerCallContext context)
        {
            CompanyJobEducationPoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<CompanyJobEducations> GetCompanyJobEducations(Empty request, ServerCallContext context)
        {
            List<CompanyJobEducationPoco> pocos = _logic.GetAll();
            CompanyJobEducations comJobs = new CompanyJobEducations();
            foreach (var poco in pocos)
            {
                CompanyJobEducationReply reply = FromPoco(poco);
                comJobs.ComJob.Add(reply);
            }
            return Task.FromResult(comJobs);
        }

        public override Task<Empty> AddCompanyJobEducation(CompanyJobEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteCompanyJobEducation(CompanyJobEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateCompanyJobEducation(CompanyJobEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}

