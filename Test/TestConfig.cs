using HConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    [Config("testconfig.cfg", Header = "Test Config", UseAllProperties = true)]
    [Config.Comment("Test Config")]
    [Config.Comment("A Region", Region = "Region")]
    class TestConfig
    {
        [Config.Option]
        public int IntProperty { get; set; }
        
        [Config.Option]
        public string StringProperty { get; set; }

        [Config.Option]
        public double DoubleProperty { get; set; }

        [Config.Option]
        public float FloatProperty { get; set; }

        [Config.Option]
        public long LongProperty { get; set; }

        [Config.Option]
        public bool BoolProperty { get; set; }

        [Config.Option(Comment = "A property with a comment", Name = "Commented Property")]
        public string StringWithComment { get; set; }

        [Config.Option(Name = "1", Region = "Region")]
        public string RegionField1 { get; set; }

        [Config.Option(Name = "2", Region = "Region")]
        public string RegionField2 { get; set; }

        [Config.Option(Name = "3", Region = "Region")]
        public string RegionField3 { get; set; }

        public string UnmarkedProperty { get; set; }
    }
}
