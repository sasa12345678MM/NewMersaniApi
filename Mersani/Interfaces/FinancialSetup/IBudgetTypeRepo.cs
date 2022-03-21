using Mersani.models.FinancialSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IBudgetTypeRepo
    {
        Task<List<BudgetType>> GetBudgetType(int id, string authParms);
        Task<bool> PostNewBudgetType(BudgetType budgetType, string authParms);
        Task<bool> DeleteBudgetType(int id, string authParms);
    }
}
