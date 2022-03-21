using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models.Stock;

namespace Mersani.Interfaces.Website.ItemGroups
{
  public  interface IWebItemGroup
    {

        Task<DataSet> GetItemsGroups(Mersani.models.Stock.ItemGroups entity, string authParms);
        public  Task<DataSet> GetItemsGroupsByLevel(string levels, string authParms);

        public Task<DataSet> GetItemsGroupChildren(int GroupId, string authParms);

       

    }
}
