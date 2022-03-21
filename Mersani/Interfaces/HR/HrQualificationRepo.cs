using Mersani.models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
    public interface HrQualificationRepo
    {
        Task<DataSet> GetHrQualificationData(int HrQualification, string authParms);
       
        Task<DataSet> PostHrQualificationListData(List<HrQualification> HrQualification, string authParms);
        Task<DataSet> DeleteHrQualificationData(HrQualification HrQualification, string authParms);
    }
}