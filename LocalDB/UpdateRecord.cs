using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.LocalDB
{
    [SugarTable("UpdateRecord")]
    public class UpdateRecoreModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? ID { get; set; }
        public string CategoryId { get; set; }
        public string Time { get; set; }
        public eCategoryType Type { get; set; }

    }
    public enum eCategoryType
    {
        产品 = 0,
        材质 = 1
    }

    public class UpdateRecord : Dao
    {
        public void insert(UpdateRecoreModel u)
        {
            db.Insertable(u).ExecuteCommand();
        }


        public bool find(string CategoryId)
        {
            var d = db.Queryable<UpdateRecoreModel>().ToList();
            return db.Queryable<UpdateRecoreModel>().Where(X => X.CategoryId.Equals(CategoryId)).ToList().Count > 0;
        }



    }
}
