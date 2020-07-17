using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class SystemLanguageCodeLogic
    {
        protected IDataRepository<SystemLanguageCodePoco> _repository;
        public SystemLanguageCodeLogic(IDataRepository<SystemLanguageCodePoco> repository)
        //) : base(repository)
        {
        _repository= repository;
            }

        public  void Update(SystemLanguageCodePoco[] pocos)
        {
            _repository.Update(pocos);
        }

        public void Delete(SystemLanguageCodePoco[] pocos)
        {
            _repository.Remove(pocos);
        }

        public void Add(SystemLanguageCodePoco[] pocos)
        {
            Verify(pocos);
            _repository.Add(pocos);
        }
        public List<SystemLanguageCodePoco> GetAll()
        {
            return _repository.GetAll().ToList();
        }
        public virtual SystemLanguageCodePoco Get(string Id)
        {
            return _repository.GetSingle(c => c.LanguageID == Id);

        }

        public void Verify(SystemLanguageCodePoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();

            foreach (SystemLanguageCodePoco poco in pocos)
            {
                if ((poco.LanguageID ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(1000, "Cannot be empty"));
                }
                if ((poco.Name ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(1001, "Cannot be empty"));
                }
                if ((poco.NativeName ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(1002, "Cannot be empty"));
                }
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}