using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Andrew.TagsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IReadOnlyTagsRepo tagrepo = new TagsRepo(8, "zh-TW"); // should be resolved from DI container.


            #region call search API

            List<(string value, int count)> search_results = new List<(string value, int count)>();

            // init search_results
            // todo: 輸入 momo page 的搜尋結果當作案例


            #endregion



            // todo: list tags cloud like momo's salespage
            // requirement: 在 tagvalue 的組合數量不大於 100 筆的前提下, 展開的效能必須壓在 10ms (?) 以下
            var x = new Dictionary<TagGroup, List<(TagKey, int)>>();
            foreach (var result in tagrepo.BulkLoadTagKeysByValue(search_results))
            {
                if (x.ContainsKey(result.group) == false) x.Add(result.group, new List<(TagKey, int)>());
                x[result.group].Add((result.key, result.count));
            }

            foreach(var g in x.Keys)
            {
                Console.WriteLine($"Group: {g.DisplayName}:");
                foreach(var k in x[g])
                {
                    Console.WriteLine($"- {k.Item1.DisplayKey}({k.Item2})");
                }
            }
            
        }
    }

    public interface IReadOnlyTagsRepo
    {
        public IEnumerable<string> GetTagGroupNames();
        public IEnumerable<string> GetTagKeyNames(string tagGroupName);

        public TagGroup GetTagGroupByName(string tagGroupName);

        public TagKey GetTagKeyByValue(string tagValue);


        public IEnumerable<TagKey> GetTagKeysByGroupName(string tagGroupName)
        {
            foreach(var key in this.GetTagKeyNames(tagGroupName))
            {
                yield return this.GetTagKeyByValue($"{tagGroupName}::{key}");
            }
        }

        public IEnumerable<(TagGroup group, TagKey key, int count)> BulkLoadTagKeysByValue(IEnumerable<(string value, int count)> search_results)
        {
            foreach (var result in search_results)
            {
                yield return
                    (
                        this.GetTagGroupByName(result.value.Split("::")[0]),
                        this.GetTagKeyByValue(result.value),
                        result.count
                    );
            }
        }
    }

    public class TagsRepo : IReadOnlyTagsRepo
    {
        private int _shopId = 0;
        private string _langId = null;

        public TagsRepo(int shopId, string langId)
        {

        }

        public IEnumerable<string> GetTagGroupNames()
        {
            throw new NotImplementedException();
        }

        public TagGroup GetTagGroupByName(string tagGroupName)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<string> GetTagKeyNames(string tagGroupName)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<TagKey> GetTagKeysByGroupName(string tagGroupName)
        //{
        //    throw new NotImplementedException();
        //}

        public TagKey GetTagKeyByValue(string tagValue)
        {
            throw new NotImplementedException();
        }


        #region Helper Extensions

        //public IEnumerable<(TagGroup group, TagKey key, int count)> BulkLoadTagKeysByValue(IEnumerable<(string value, int count)> search_results)
        //{
        //    foreach(var result in search_results)
        //    {
        //        yield return
        //            (
        //                this.GetTagGroupByName(result.value.Split("::")[0]),
        //                this.GetTagKeyByValue(result.value),
        //                result.count
        //            );
        //    }
        //}


        #endregion
    }



    public class TagGroup
    {
        public string Name { get; private set; }


        // 使用範圍: SYSTEM (系統內部使用) | TENANT (商店維度使用)
        public string Scope;

        public string Market; // TW | MY | PX | HK

        public long ShopID; // 0 if not specified

        public bool IsArchived;

        // 是否支援多國語顯示?
        public bool IsMUI = false;

        // 如果支援自訂 tags, 則啟用 tags 的 CRUD helper method, 同時支援 tags CRUD events
        // description
        public MUIText Description;

        // display
        public MUIText DisplayName;




        #region custom feature(s)
        // 是否允許自訂 tags? (若不支援，則只能使用 pre-defined tags)
        public bool AllowCustTags = false;

        public bool IsDisplay = false;

        public int MaxOccurance; // 同一個主體 (entity) 最多能標示幾個該群組的標籤?

        protected Dictionary<string, object> Features = null; // new Dictionary<string, object>();
        #endregion

        #region extension property(ies)
        protected Dictionary<string, object> Extensions = null; // new Dictionary<string, object>();
        #endregion

    }




    //  多國語 (只有 tag group 允許才支援)
    //  列舉檢查 (只有 tag group 不允許自訂 value 才支援)
    public class TagKey
    {
        // need detect current culture info

        // need detect current track context


        private readonly TagGroup _group;

        public string Group { get { return this._group.Name; } }

        public string Name { get; private set; }

        // 能直接附加在 entity 上的 tag.value, 例如 "color::RED", "size::XL"
        public string Value { get; private set; }

        public bool IsArchived;

        // description
        public MUIText Description;

        // display
        public MUIText DisplayKey;



        #region extension property(ies)
        protected Dictionary<string, object> Extensions = null; // new Dictionary<string, object>();
        #endregion
    }

    public class MUIText
    {
        private string _default;
        private Dictionary<string, string> _mapping = new Dictionary<string, string>();

        public MUIText()
        {

        }

        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        public string ToString(CultureInfo culture)
        {
            if (this._mapping.ContainsKey(culture.Name))
            {
                return this._mapping[culture.Name];
            }
            else if (this._mapping.ContainsKey(culture.Parent.Name))
            {
                return this._mapping[culture.Parent.Name];
            }
            else
            {
                return this._default;
            }
        }
    }
}
