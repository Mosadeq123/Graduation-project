using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;
using Store.G04.Core.Entities.Order;
using Store.G04.Repositpory.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G04.Repositpory.Data
{
    public class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context)
        {
            if (_context.Machine.Count() == 0)
            {
                //Machine
                //1. Read Data From json File
                var MachinesData = File.ReadAllText(@"../Store.G04.Repositpory/Data/DataSeed/Machines.json");
                //2. Convert json To List<T>
                var Machines = JsonSerializer.Deserialize<List<MachineEntity>>(MachinesData);
                //3.Seed Data To DB
                if (Machines is not null && Machines.Count() > 0)
                {
                    await _context.Machine.AddRangeAsync(Machines);
                    await _context.SaveChangesAsync();
                }
            }
            if (_context.RawMaterial.Count() == 0)
            {
                //Machine
                //1. Read Data From json File
                var RawMaterialData = File.ReadAllText(@"../Store.G04.Repositpory/Data/DataSeed/RawMaterials.json");
                //2. Convert json To List<T>
                var RawMaterials = JsonSerializer.Deserialize<List<RawMaterial>>(RawMaterialData);
                //3.Seed Data To DB
                if (RawMaterials is not null && RawMaterials.Count() > 0)
                {
                    await _context.RawMaterial.AddRangeAsync(RawMaterials);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.DeliveryMethods.Count() == 0)
            {
                //Machine
                //1. Read Data From json File
                var deliveryData = File.ReadAllText(@"../Store.G04.Repositpory/Data/DataSeed/delivery.json");
                //2. Convert json To List<T>
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                //3.Seed Data To DB
                if (deliveryMethods is not null && deliveryMethods.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
