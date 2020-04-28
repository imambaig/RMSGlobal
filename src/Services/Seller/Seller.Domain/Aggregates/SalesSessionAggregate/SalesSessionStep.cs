using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Domain.Aggregates.SalesSessionAggregate
{    


    public class SalesSessionStep: Entity
    {

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate() => _startDate;
        private DateTime? _startDate;
        public void setStartDate(DateTime startDate)
        {
            _startDate = startDate;
        }

        private int _stepNumber;
        private int StepNumber;

        private int _saleTypeId;
        protected SalesSessionStep()
        {

        }
        public SalesSessionStep(int saleType, int stepNumber)
        {
            _saleTypeId = saleType;
            _stepNumber = stepNumber;
        }

        public int SaleType() => _saleTypeId;

        public void setStepNumber(int stepNumber)
        {
            if (stepNumber < 1)
            {
                // throw domain exception
            }
            _stepNumber = stepNumber;
        }
    }
}
