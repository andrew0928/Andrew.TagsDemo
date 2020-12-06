using System;
using System.Collections.Generic;

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



        // 是否允許自訂 tags? (若不支援，則只能使用 pre-defined tags)
        public bool AllowCustTags = false;

        // 是否支援多國語顯示?

        // 用途說明

        public IEnumerable<TagKey> GetTags()
        {
            throw new ArgumentException();
        }
    }

    //  多國語 (只有 tag group 允許才支援)
    //  列舉檢查 (只有 tag group 不允許自訂 value 才支援)
    public class TagKey
    {
        private readonly TagGroup _group;

        public string Name { get { return this._group.Name; } }
        public string Value { get; private set; }

        public string FullName
        {
            get
            {
                return $"{this.Name}::{this.Value}";
            }
        }

        // option(s)

        // 1. 直接顯示 value (預設值)
        // 2. 顯示 display 屬性 (固定語系)
        // 3. 顯示 display 屬性 (多國語系)
        public string Display { get; }  // show multilingual if available


        // 1. 顯示 description (固定語系)
        // 2. 顯示 description (多國語系)
        public string Description { get; }
    }
}
