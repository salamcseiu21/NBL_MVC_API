using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.DAL
{
   public class BarCodeGateway: DbGateway, IBarCodeGateway
    {
        public int GenerateBarCode(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public int Add(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public ViewCreateBarCodeModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ViewCreateBarCodeModel> GetAll()
        {
            throw new NotImplementedException();
        }

       
    }
}
