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
using static CareerCloud.gRPC.Protos.ApplicantJobApplication;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantJobApplicationService: ApplicantJobApplicationBase
    {
        private readonly ApplicantJobApplicationLogic _logic;

        public ApplicantJobApplicationService()
        {
            _logic = new ApplicantJobApplicationLogic(new EFGenericRepository<ApplicantJobApplicationPoco>());
        }



        private ApplicantJobApplicationReply FromPoco(ApplicantJobApplicationPoco poco)
        {
            return new ApplicantJobApplicationReply()
            {
                Id = poco.Id.ToString(),
                Applicant = poco.Applicant.ToString(),
                Job = poco.Job.ToString(),
                
                ApplicationDate = ((DateTime?)(poco.ApplicationDate)).ToTimeStamp(),
               

                Timestamp = ByteString.CopyFrom(poco.TimeStamp)
            };

        }

        private List<ApplicantJobApplicationPoco> ToPoco(ApplicantJobApplications appJob)
        {
            List<ApplicantJobApplicationPoco> pocos = new List<ApplicantJobApplicationPoco>();
            foreach (var reply in appJob.AppJobs)
            {
                ApplicantJobApplicationPoco poco = new ApplicantJobApplicationPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Applicant = Guid.Parse(reply.Applicant);
                poco.Job = Guid.Parse(reply.Job);
                poco.ApplicationDate = reply.ApplicationDate.ToDateTime();
                
                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<ApplicantJobApplicationReply> GetApplicantJobApplication(Protos.IdRequest1 request, ServerCallContext context)
        {
            ApplicantJobApplicationPoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<ApplicantJobApplications> GetApplicantJobApplications(Empty request, ServerCallContext context)
        {
            List<ApplicantJobApplicationPoco> pocos = _logic.GetAll();
            ApplicantJobApplications appjobs = new ApplicantJobApplications();
            foreach (var poco in pocos)
            {
                ApplicantJobApplicationReply reply = FromPoco(poco);
                appjobs.AppJobs.Add(reply);
            }
            return Task.FromResult(appjobs);
        }

        public override Task<Empty> AddApplicantJobApplication(ApplicantJobApplications request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteApplicantJobApplication(ApplicantJobApplications request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateApplicantJobApplication(ApplicantJobApplications request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}
