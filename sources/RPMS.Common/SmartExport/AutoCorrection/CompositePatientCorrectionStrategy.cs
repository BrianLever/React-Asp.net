using System;
using System.Collections.Generic;
using System.Linq;
using RPMS.Common.Models;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    /// <summary>
    /// Compound correction strategy that product all options from given several strategies
    /// </summary>
    public abstract class CompositePatientCorrectionStrategy : IPatientAutoCorrectionStrategy
    {
        public abstract IReadOnlyList<IPatientAutoCorrectionStrategy> Strategies { get; }

        public IEnumerable<PatientSearch> Apply(PatientSearch patient)
        {
            List<PatientSearch> results = new List<PatientSearch>();
            var modifiedPatient = patient;
            

            // proceed all stategies in recursive approach
            var strategy = Strategies.FirstOrDefault();

            if(strategy == null)
            {
                throw new ArgumentException("Strategies cannot be empty");
            }

            return ApplyStrategy(strategy, patient, 0);
        }

        private IEnumerable<PatientSearch> ApplyStrategy(
            IPatientAutoCorrectionStrategy strategy,
            PatientSearch patient,
            int currentStrategyIndex
            )
        {
            List<PatientSearch> results = new List<PatientSearch>();

            var nextStrategyIndex = currentStrategyIndex + 1;

            // when no more strategies
            if (nextStrategyIndex >= Strategies.Count)
            {
                foreach (var patientPattern in strategy.Apply(patient))
                {
                    yield return patientPattern;
                }
            }
            else
            {
                var nextStrategy = Strategies[nextStrategyIndex];
                // get all options from the current strategy
                foreach (var patientItem in strategy.Apply(patient))
                {
                    var nestedResults = ApplyStrategy(nextStrategy, patientItem, nextStrategyIndex);

                    foreach (var patientPattern in nestedResults)
                    {
                        yield return patientPattern;
                    }
                }
            }

            //return results;
        }

        public IEnumerable<string> GetModificationsLogDescription()
        {
            return Strategies.SelectMany(x => x.GetModificationsLogDescription());
        }
    }
}
