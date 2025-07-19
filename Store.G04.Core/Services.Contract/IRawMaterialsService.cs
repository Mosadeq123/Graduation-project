using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Helper;
using Store.G04.Core.Specifications.RawMaterialss;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.G04.Core.Services.Contract
{
    public interface IRawMaterialsService
    {
        Task<PaginationResponse<RawMaterialDtos>> GetAllRawMaterialsAsync(RawMaterialssSpecParams rawMaterialssSpec);
        Task<RawMaterialDtos> GetRawMaterialById(int id);
    }
}