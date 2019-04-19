using System;
using System.Linq;
using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSuppliersDto, Supplier>();

            this.CreateMap<ImportPartsDto, Part>();

            this.CreateMap<ImportCustomersDto, Customer>();

            this.CreateMap<ImportSalesDto, Sale>();

            this.CreateMap<Car, ExportCarsWithDistanceDto>();

            this.CreateMap<Car, ExportCarsFromMakeBmwDto>();

            this.CreateMap<Supplier, ExportLocalSuppliersDto>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count));

            this.CreateMap<Customer, ExportTotalSalesByCustomerDto>()
                .ForMember(x => x.BoughtCars, y => y.MapFrom(s => s.Sales.Count))
                .ForMember(x => x.SpentMoney,
                    y => y.MapFrom(x => x.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))));
        }
    }
}