using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarDealer
{
    public class StartUp
    {
        public const string successMassage = "Successfully imported {0}";

        public static void Main(string[] args)
        {
            Mapper.Initialize(conf => conf.AddProfile<CarDealerProfile>());

            string suppliersXml = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\XmlProcessingExercises\CarDealer\Datasets\suppliers.xml");
            string partsXml = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\XmlProcessingExercises\CarDealer\Datasets\parts.xml");
            string carsXml = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\XmlProcessingExercises\CarDealer\Datasets\cars.xml");
            string customersXml = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\XmlProcessingExercises\CarDealer\Datasets\customers.xml");
            string salesXml = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\XmlProcessingExercises\CarDealer\Datasets\sales.xml");

            using (var context = new CarDealerContext())
            {
                //Console.WriteLine(ImportSuppliers(context, suppliersXml));
                //Console.WriteLine(ImportParts(context, partsXml));
                //Console.WriteLine(ImportCars(context, carsXml));
                //Console.WriteLine(ImportCustomers(context, customersXml));
                //Console.WriteLine(ImportSales(context, salesXml));

                //Console.WriteLine(GetCarsWithDistance(context));
                //Console.WriteLine(GetCarsFromMakeBmw(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer(context));
                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        //9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSuppliersDto[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (ImportSuppliersDto[])serializer.Deserialize(new StringReader(inputXml));

            var suppliers = Mapper.Map<Supplier[]>(suppliersDto);

            context.Suppliers.AddRange(suppliers);

            var suppliersImported = context.SaveChanges();

            return string.Format(successMassage, suppliersImported);
        }

        //10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartsDto[]), new XmlRootAttribute("Parts"));

            var partsDto = ((ImportPartsDto[])serializer.Deserialize(new StringReader(inputXml)))
                .Where(p => context.Suppliers.Find(p.SupplierId) != null);

            var parts = Mapper.Map<Part[]>(partsDto);

            context.Parts.AddRange(parts);

            var partsImported = context.SaveChanges();

            return string.Format(successMassage, partsImported);
        }

        //11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCarsDto[]), new XmlRootAttribute("Cars"));

            var carsDto = (ImportCarsDto[])serializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();

            foreach (var carDto in carsDto)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                int[] parts = carDto.Parts.Select(p => p.Id)
                    .Where(p => context.Parts.Find(p) != null)
                    .Distinct()
                    .ToArray();

                car.PartCars = parts.Select(p => new PartCar()
                {
                    Car = car,
                    PartId = p
                }).ToArray();

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return string.Format(successMassage, cars.Count);
        }

        //12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomersDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomersDto[])serializer.Deserialize(new StringReader(inputXml));

            var customers = Mapper.Map<Customer[]>(customersDto);

            context.Customers.AddRange(customers);

            var customersImported = context.SaveChanges();

            return string.Format(successMassage, customersImported);
        }

        //13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSalesDto[]), new XmlRootAttribute("Sales"));

            var salesDto = ((ImportSalesDto[])serializer.Deserialize(new StringReader(inputXml)))
                .Where(s => context.Cars.Find(s.CarId) != null);

            var sales = Mapper.Map<Sale[]>(salesDto);

            context.Sales.AddRange(sales);

            var salesImported = context.SaveChanges();

            return string.Format(successMassage, salesImported);
        }

        //14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsDto = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => Mapper.Map<ExportCarsWithDistanceDto>(c))
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), carsDto, namespaces);

            return result.ToString().TrimEnd();
        }

        //15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsDto = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => Mapper.Map<ExportCarsFromMakeBmwDto>(c))
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsFromMakeBmwDto[]), new XmlRootAttribute("cars"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
               XmlQualifiedName.Empty,
           });

            serializer.Serialize(new StringWriter(result), carsDto, namespaces);

            return result.ToString().TrimEnd();
        }

        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersDto = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => Mapper.Map<ExportLocalSuppliersDto>(s))
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), suppliersDto, namespaces);

            return result.ToString().TrimEnd();
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsDto = context.Cars
                .Select(c => new ExportCarsWithListOfPartsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new ExportPartsDto()
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsWithListOfPartsDto[]), new XmlRootAttribute("cars"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), carsDto, namespaces);

            return result.ToString().TrimEnd();
        }

        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersDto = context.Customers
                .Include(c => c.Sales)
                .ThenInclude(s => s.Car)
                .ThenInclude(c => c.PartCars)
                .ThenInclude(pc => pc.Part)
                .Where(c => c.Sales.Count > 0)
                .Select(c => Mapper.Map<ExportTotalSalesByCustomerDto>(c))
                .ToArray()
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportTotalSalesByCustomerDto[]), new XmlRootAttribute("customers"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), customersDto, namespaces);

            return result.ToString().TrimEnd();
        }

        //19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesDto = context.Sales
                .Include(s => s.Car)
                .ThenInclude(c => c.PartCars)
                .ThenInclude(pc => pc.Part)
                .Include(s => s.Customer)
                .Select(s => new ExportSalesWithAppliedDiscountDto()
                {
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Car = new ExportCarsWithDistanceDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                        (s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100)
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportSalesWithAppliedDiscountDto[]), new XmlRootAttribute("sales"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(result), salesDto, namespaces);

            return result.ToString().TrimEnd();
        }
    }
}
