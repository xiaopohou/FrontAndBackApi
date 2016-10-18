using System.Collections.Generic;

namespace Script.I200.Entity.Api.BasicData
{
    //省市区地址数据
    public class Address
    {
        public class Province
        {
            /// <summary>
            /// 省Id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// 省名称
            /// </summary>
            public string Name { get; set; }
        }

        public class City
        {
            /// <summary>
            /// 省Id
            /// </summary>
            public int ProvinceId { get; set; }

            /// <summary>
            /// 城市列表
            /// </summary>
            public List<CityValues> CityValues { get; set; }
        }

        public class CityValues
        {
            /// <summary>
            /// 城市Id
            /// </summary>
            public int CityId { get; set; }

            /// <summary>
            /// 城市名称
            /// </summary>
            public string CityName { get; set; }
        }

        public class RetAddressData
        {
            public RetAddressData()
            {
                ProvinceList = new  List<Province>();
                CityList = new List<City>();
            }

            /// <summary>
            /// 省列表
            /// </summary>
            public List<Province> ProvinceList { get; set; }

            /// <summary>
            /// 城市列表
            /// </summary>
            public List<City> CityList { get; set; }
        }


        public class ProvinceAndCityName
        {
            public int ProvinceId { get; set; }

            public string ProvinceName { get; set; }

            public int CityId { get; set; }

            public string CityName { get; set; }
        }
    }
}