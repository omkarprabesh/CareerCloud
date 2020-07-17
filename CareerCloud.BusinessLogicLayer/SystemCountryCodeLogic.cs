using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class SystemCountryCodeLogic 
    {
        protected IDataRepository<SystemCountryCodePoco> _repository;
        public SystemCountryCodeLogic
         (
        IDataRepository<SystemCountryCodePoco> repository)
            //) : base(repository)
        {
            _repository = repository;
        }
        public void Update(SystemCountryCodePoco[] pocos)
        {
            _repository.Update(pocos);
        }

        public void Delete(SystemCountryCodePoco[] pocos)
        {
            _repository.Remove(pocos);
        }

        public void Add(SystemCountryCodePoco[] pocos)
        {
            Verify(pocos);
            _repository.Add(pocos);
        }
        public List<SystemCountryCodePoco> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public virtual SystemCountryCodePoco Get(string code)
        {
            return _repository.GetSingle(c => c.Code == code);

        }

        public void Verify(SystemCountryCodePoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();

            foreach (SystemCountryCodePoco poco in pocos)
            {
                if ((poco.Code ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(900, "Cannot be empty"));
                }
                if ((poco.Name?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(901, "Cannot be empty"));
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}
