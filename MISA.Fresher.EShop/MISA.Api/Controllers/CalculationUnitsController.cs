using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Api.Controllers
{

    public class CalculationUnitsController : BaseEntitiesController<CalculationUnit>
    {
        ICalculationUnitService _baseService;
        public CalculationUnitsController(ICalculationUnitService baseService) : base(baseService)
        {
            _baseService = baseService;
        }
    }
}
