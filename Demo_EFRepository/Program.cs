using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSF.EFRepository;

namespace Demo_EFRepository
{
    class tempOrder
    {
        public string CustomerID { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var UnitOfWork = new EFUnitOfWork(new NorthwindEntities());

            //建立 Customers repository
            var customers = new EFRepository<Customers>(UnitOfWork);
            
            //取得全部資料
            var result01 = customers.GetALL();
            foreach (var item in result01)
            {
                Console.WriteLine($"{item.CustomerID}\t{item.CompanyName}\t{item.City}");
            }

            //取得指定條件的資料
            var result02 = customers.GetSingle(x=>x.City.Equals("London"));
            Console.WriteLine($"所屬城市：{result02.City}");

            //判斷資料是否存在
            var result03 = customers.Exists(x => x.City == "Taipei");
            Console.WriteLine($"資料是否存在：{result03}");
            
            //操作 commandText 來取得資料
            var result04 = UnitOfWork.ExecuteCommandText<tempOrder>(CommandTextType.CommandText, "select CustomerID, OrderID, ORderDate from Orders where CustomerID=@CustomerID", new System.Data.SqlClient.SqlParameter("@CustomerID", "VINET"));
            foreach (var item in result04)
            {
                Console.WriteLine($"{item.CustomerID}\t{item.OrderID}\t{item.OrderDate.ToShortDateString()}");
            }

            //操作 procedure 來取得資料
            var result05 = UnitOfWork.ExecuteCommandText<tempOrder>(CommandTextType.CommandProcedure, "spGetOrderData", new System.Data.SqlClient.SqlParameter("@CustomerID", "VINET"));
            foreach (var item in result05)
            {
                Console.WriteLine($"{item.CustomerID}\t{item.OrderID}\t{item.OrderDate.ToShortDateString()}");
            }

            //操作 procedure 但不回傳資料
            var result06 = UnitOfWork.ExecuteStoreProcedure("spGetOrderData", new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@CustomerID", "VINET") });
            
            Console.ReadLine();
        }
    }
}
