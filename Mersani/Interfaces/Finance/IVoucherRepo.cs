using Mersani.models.Finance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace Mersani.Interfaces.Finance
{
    public interface IVoucherRepo
    {
        Task<DataSet> GetVoucher(int id, string authParms);
        Task<DataSet> GetVoucherIn(int id, string authParms); 
        Task<DataSet> GetVoucherOut(int id, string authParms);
        Task<DataSet> GetALLVoucher(int id, string authParms);
        Task<DataSet> GetUnPostedVoucher(int id, char postedType, string authParms);
        Task<DataSet> GetPostedVoucher(int id, string authParms);
        Task<DataSet> GetVoucherTrans(int voucherMaster, string authParms);
        Task<DataSet> PostVoucher(Voucher entities, string authParms);
        Task<DataSet> DeletVoucher(List<Voucher> entities, string authParms);
        Task<DataSet> PostedVoucherHdr(List<VoucherHdr> entities, string authParms);
    }
}
