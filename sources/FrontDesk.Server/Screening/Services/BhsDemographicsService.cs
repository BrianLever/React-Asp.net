using Common.Logging;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Services;

using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Services
{
    public interface IBhsDemographicsService
    {
        BhsDemographics Get(long id);
        void Update(BhsDemographics model, IUserPrincipal user);
        bool Exists(ScreeningResult screeningResult);
        long? Find(IScreeningPatientIdentity patient);
        long Create(ScreeningResult screeningResult);
    }

    public class BhsDemographicsService : IBhsDemographicsService
    {
        private readonly IBhsDemographicsRepository _bhsDemographicsRepository;
        private readonly IBhsDemographicsFactory _factory;
        private readonly ILog _logger = LogManager.GetLogger<BhsDemographicsService>();

        public BhsDemographicsService(IBhsDemographicsRepository bhsDemographicsRepository, IBhsDemographicsFactory factory)
        {
            _bhsDemographicsRepository = bhsDemographicsRepository ?? throw new ArgumentNullException(nameof(bhsDemographicsRepository));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public BhsDemographicsService(): this(new BhsDemographicsDb(), new BhsDemographicsFactory())
        {   

        }


        public BhsDemographics Get(long id)
        {
            var model = _bhsDemographicsRepository.Get(id);
            if (model == null) return null;

            return model;
        }

        public void Update(BhsDemographics model, IUserPrincipal user)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            model.BhsStaffNameCompleted = user.FullName;
            model.CompleteDate = DateTimeOffset.Now;

            _bhsDemographicsRepository.UpdateManualEntries(model);
        }

        public bool Exists(ScreeningResult screeningResult)
        {
            return _bhsDemographicsRepository.Exists(screeningResult);
        }

        public long Create(ScreeningResult screeningResult)
        {
            return _bhsDemographicsRepository.Add(_factory.Create(screeningResult));
        }

        public long? Find(IScreeningPatientIdentity patient)
        {
            return _bhsDemographicsRepository.Find(patient);
        }
    }
}
