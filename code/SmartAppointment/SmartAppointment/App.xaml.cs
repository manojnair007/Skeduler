using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Autofac;
using Microsoft.Identity.Client;
using SmartAppointment.Configurations;
using SmartAppointment.Core;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Services;
using SmartAppointment.Services;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartAppointment
{
    public partial class App : Application
    {
        public static object UIParent { get; set; } = null;
        public App()
        {
            InitializeComponent();
           
            AppContainerService.GetInstance().Resolve<IAuthenticationService>(
                                                            new NamedParameter("clientId", GlobalConfig.ClientId),
                                                            new NamedParameter("iosKeychainSecurityGroups", GlobalConfig.IosKeychainSecurityGroups),
                                                            new NamedParameter("authoritySignIn", GlobalConfig.AuthoritySignin));

            // DependencyService.Register<MockDataStore>();
            
            #region Temp code to insert tenant info
            //ICategoriesRepository categoriesRepository = AppContainerService.GetInstance().Resolve<ICategoriesRepository>();

           // categoriesRepository.GetServiceProviderCategoryItemsAsync();
            //categoriesRepository.InsertItemAsync(new Core.Entities.ServiceProviderCategory()
            //{
            //    Category = "Doctor",
            //    SubCategories = new List<SubCategory>()
            //    {
            //        new SubCategory() { Name = "Pediatrician" },
            //        new SubCategory() { Name = "ENT Specialist" },
            //        new SubCategory() { Name = "Dentist" },
            //        new SubCategory() { Name = "Orthopedist" },
            //        new SubCategory() { Name = "Dermatologist" },
            //        new SubCategory() { Name = "Cardiologist" },
            //        new SubCategory() { Name = "Gynaecologist" },
            //        new SubCategory() { Name = "General Physician" },
            //        new SubCategory() { Name = "Diabetologist" },
            //        new SubCategory() { Name = "Ophthalmologist" },
            //        new SubCategory() { Name = "Ayurveda" },
            //        new SubCategory() { Name = "Homeopathic" },
            //         new SubCategory() { Name = "Dietitian/Nutritionist" },
            //         new SubCategory() { Name = "Veterinary Physician" }
            //    }
            //});

            //categoriesRepository.InsertItemAsync(new Core.Entities.ServiceProviderCategory()
            //{
            //    Category = "Beauty Parlour",
            //    SubCategories = new List<SubCategory>()
            //    {
            //        new SubCategory() { Name = "Unisex" },
            //        new SubCategory() { Name = "Men" },
            //        new SubCategory() { Name = "Women" }
            //    }
            //});

            //tenantsRepository.InsertItemAsync(new DoctorTenant()
            //{
            //    Category = "Doctor",
            //    Description = "Dr. Anay a. Deshmukh is a pediatrician in Wakad, Pune and has an experience of 9 years in this field. Dr. Anay a. Deshmukh practices at tiny steps child care and vaccination clinic in wWakad, Pune. He completed MBBS from Topiwala national medical college and nair hospital,mumbai in 2002, diploma in child health (DCH) from a college of physicians and surgeons Mumbai in 2006 and DNB (pediatrics) from the national board of examinations, new Delhi in 2010.",
            //    PhoneNo = "02029707440",
            //    SubCategory = "Pediatrician",
            //    DoctorName = "Dr. Anay Deshmukh",
            //    TenantName = "Tiny Steps child care and vaccination clinic",
            //    AddressDetails = new AddressDetails()
            //    {
            //        BuildingName = "",
            //        City = "Pune",
            //        Country = "India",
            //        Number = "188",
            //        State = "Maharashtra",
            //        PinCode = "411057",
            //        Address = "Green Dr Rd, Shankar Kalat Nagar, Wakad, Pimpri-Chinchwad",
            //        GeoCordinates = new LongLat()
            //        {
            //           Latitude = 18.59878M,
            //           Longitude = 73.764088M,
            //        }
            //    },
            //    BusinessTimings = new BusinessTimings()
            //    {
            //        AverageServiceTime = new TimeSpan(0, 10, 0),
            //        TimeSlots = new Dictionary<WeekDays, TimeSlotDetails[]>()
            //        {
            //            {
            //                WeekDays.Monday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Tuesday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Wednesday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Thursday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Friday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Saturday,new TimeSlotDetails[]
            //                {
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Morning,
            //                        StartTime= new TimeSpan(10,0,0),
            //                        EndTime = new TimeSpan(14,0,0)
            //                    },
            //                    new TimeSlotDetails()
            //                    {
            //                        Slot=TimeSlots.Evening,
            //                        StartTime= new TimeSpan(18,0,0),
            //                        EndTime = new TimeSpan(20,0,0)
            //                    }
            //                }
            //            },
            //            {
            //                WeekDays.Sunday,null
            //            },

            //        }

            //    }
            //});
            #endregion

            MainPage = new AppShell();

        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
