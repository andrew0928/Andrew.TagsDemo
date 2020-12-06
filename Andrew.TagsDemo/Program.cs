using System;
using System.Collections.Generic;
using System.Globalization;

namespace Andrew.TagsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }


    public class TagsHelper
    {

    }



    public class TagGroup
    {
        public string Name { get; private set; }


        // 使用範圍: SYSTEM (系統內部使用) | TENANT (商店維度使用)
        public string Scope;

        public string Market; // TW | MY | PX | HK

        public long ShopID; // 0 if not specified

        public bool IsArchived;

        public int MaxOccurance; // 同一個主體 (entity) 最多能標示幾個該群組的標籤?


        #region configuration

        // 是否允許自訂 tags? (若不支援，則只能使用 pre-defined tags)
        public bool AllowCustTags = false;

        public bool IsDisplay = false;

        // 是否支援多國語顯示?
        public bool IsMUI = false;

        #endregion


        // 如果支援自訂 tags, 則啟用 tags 的 CRUD helper method, 同時支援 tags CRUD events
        // description
        public MUIText Description;

        // display
        public MUIText DisplayName;
    }




    //  多國語 (只有 tag group 允許才支援)
    //  列舉檢查 (只有 tag group 不允許自訂 value 才支援)
    public class TagKey
    {
        // need detect current culture info

        // need detect current track context


        private readonly TagGroup _group;

        public string Group { get { return this._group.Name; } }

        public string Key { get; private set; }

        // 能直接附加在 entity 上的 tag.value, 例如 "color::RED", "size::XL"
        public string Value { get; private set; }

        public bool IsArchived;

        // description
        public MUIText Description;

        // display
        public MUIText DisplayKey;

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
