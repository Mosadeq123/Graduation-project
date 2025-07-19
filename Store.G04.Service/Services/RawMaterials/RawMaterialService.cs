using AutoMapper;
using Store.G04.Core;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Helper;
using Store.G04.Core.Services.Contract;
using Store.G04.Core.Specifications;
using Store.G04.Core.Specifications.RawMaterialss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Service.Services.RawMaterials
{
    public class RawMaterialService : IRawMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RawMaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all raw materials
        public async Task<PaginationResponse<RawMaterialDtos>> GetAllRawMaterialsAsync(RawMaterialssSpecParams rawMaterialssSpec)
        {
            var spec = new RawMaterialSpecifications(rawMaterialssSpec);
            var rawMaterials = await _unitOfWork.Repository<RawMaterial, int>().GetAllWithSpecAsync(spec);
            var rawMaterialsAll = _mapper.Map<IEnumerable<RawMaterialDtos>>(rawMaterials);
            var totalCount = (await _unitOfWork.Repository<RawMaterial, int>().GetAllAsync()).Count();
            var count = await _unitOfWork.Repository<RawMaterial, int>().GetCountAsync(spec);
            return new PaginationResponse<RawMaterialDtos>(rawMaterialssSpec.pageSize, rawMaterialssSpec.pageIndex, count, rawMaterialsAll,totalCount);
        }

        // Get raw material by ID
        public async Task<RawMaterialDtos> GetRawMaterialById(int id)
        {
            var spec = new RawMaterialSpecifications(id);
            var rawMaterial = await _unitOfWork.Repository<RawMaterial, int>().GetWithSpecAsync(spec);
            return _mapper.Map<RawMaterialDtos>(rawMaterial);
        }
    }
}
