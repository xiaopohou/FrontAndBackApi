using System.Collections.Generic;

namespace Script.I200.Entity.Api.Expenses
{
    public class PayClassResult
    {
        public PayClassResult()
        {
            MainCategories = new List<MainCategory>();
            SubCategories = new List<SubCategory>();
        }

        public List<MainCategory> MainCategories { get; set; }

        public List<SubCategory> SubCategories { get; set; }
    }


    public class MainCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class SubCategory
    {
        public int MainCategoryId { get; set; }

        public List<SubCategoryValues> SubCategoryValues;
    }

    public class SubCategoryValues
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}