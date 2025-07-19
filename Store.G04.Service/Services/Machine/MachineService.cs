using AutoMapper;
using Store.G04.Core;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;
using Store.G04.Core.Helper;
using Store.G04.Core.Services.Contract;
using Store.G04.Core.Specifications.Machinee;

namespace Store.G04.Service.Services.Machine;
public class MachineService : IMachinesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MachineService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Get all machines
    public async Task<PaginationResponse<MachineDtos>> GetAllMachinesAsync(MachineeParams machineeSpec)
    {
        var spec = new MachineSpecifications(machineeSpec);
        var machines = await _unitOfWork.Repository<MachineEntity, int>().GetAllWithSpecAsync(spec);
        var count = await _unitOfWork.Repository<MachineEntity, int>().GetCountAsync(spec);
        var totalCount = (await _unitOfWork.Repository<MachineEntity, int>().GetAllAsync()).Count();
        var machinesAll =_mapper.Map<IEnumerable<MachineDtos>>(machines);
        return new PaginationResponse<MachineDtos>(machineeSpec.pageSize, machineeSpec.pageIndex, count, machinesAll,totalCount);
    }

    // Get machine by ID
    public async Task<MachineDtos> GetMachineByIdAsync(int id)
    {
        var spec = new MachineSpecifications(id);
        var machine = await _unitOfWork.Repository<MachineEntity, int>().GetWithSpecAsync(spec);
        return _mapper.Map<MachineDtos>(machine);
    }
}