using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CalculationUnitService : BaseService<CalculationUnit>, ICalculationUnitService
    {
        #region declare

        ICalculationUnitRepository _calculationUnitRepository;

        #endregion

        #region contructor
        public CalculationUnitService(ICalculationUnitRepository calculationUnitRepository) : base(calculationUnitRepository)
        {
            _calculationUnitRepository = calculationUnitRepository;
        }

        #endregion
    }
}
