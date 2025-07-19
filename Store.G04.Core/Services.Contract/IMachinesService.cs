using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Helper;
using Store.G04.Core.Specifications.Machinee;

namespace Store.G04.Core.Services.Contract;
public interface IMachinesService
{
    Task<PaginationResponse<MachineDtos>> GetAllMachinesAsync(MachineeParams machineeSpec);
    Task<MachineDtos> GetMachineByIdAsync(int id);
}